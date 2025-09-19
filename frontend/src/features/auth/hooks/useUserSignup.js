import { useEffect, useMemo, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { useLocation, useNavigate } from "react-router-dom";

import { clientSignupSchema } from "@/features/auth/schemas/clientSignup.schema";
import { registerUser, login as loginApi } from "@/features/auth/services/authService";
import { useAuth } from "@/features/auth/hooks/useAuth";
import { ROUTES } from "@/shared/routes/routes";

const STEP1_FIELDS = ["name", "email", "password", "confirmPassword", "coachCode"];
const STEP2_FIELDS = ["gymName", "gymCity", "gymCountry"];

export function useUserSignup() {
    const navigate = useNavigate();
    const location = useLocation();
    const { login: applyToken } = useAuth();

    const [step, setStep] = useState(1);
    const [submitError, setSubmitError] = useState("");
    const [isSubmitting, setIsSubmitting] = useState(false);

    const form = useForm({
        resolver: zodResolver(clientSignupSchema),
        defaultValues: {
        name: "",
        email: "",
        password: "",
        confirmPassword: "",
        coachCode: "",
        gymName: "",
        gymCity: "",
        gymCountry: "",
        },
        mode: "onSubmit",
    });

        const { watch, trigger, setValue, handleSubmit, control,  formState } = form;

        const pwd = watch("password");
        const confirm = watch("confirmPassword");

        useEffect(() => {
            if ((confirm ?? "").length > 0) {
                trigger("confirmPassword");
            }
        }, [pwd, confirm, trigger]);

        // Pré-preenche ?coach=slug 
        useEffect(() => {
            const params = new URLSearchParams(location.search);
            const coach = params.get("coach");

            if (coach) setValue("coachCode", coach, { shouldValidate: true });
            // eslint-disable-next-line react-hooks/exhaustive-deps
        }, []);

        const step1Values = watch(STEP1_FIELDS);
        const step2Values = watch(STEP2_FIELDS);

        const canContinue = useMemo(() => {
            const [name, email, pass, confirm, coachCode] = step1Values;
            const filled = [name, email, pass, confirm, coachCode]
                .every(v => (v ?? "").toString().trim().length > 0);

            return filled && pass === confirm;
        }, [step1Values]);

        const canSubmit = useMemo(() => {
            return step2Values.every(v => (v ?? "").toString().trim().length > 0);
        }, [step2Values]);

        const goNext = async () => {
            const ok = await trigger(STEP1_FIELDS, { shouldFocus: true });

            if (ok) setStep(2);
        };

        const goBack = () => setStep(1);

        const onSubmit = async (values) => {
            setSubmitError("");
            setIsSubmitting(true);

            try {
                const res = await registerUser({
                    name: values.name,
                    email: values.email,
                    password: values.password,
                    coachCode: values.coachCode.trim(),
                    gymName: values.gymName,
                    gymCity: values.gymCity,
                    gymCountry: values.gymCountry,
                });

                // se o backend já devolver token no register, aplica e navega
                if (res?.token) {
                    applyToken(res.token);
                    navigate(ROUTES.app);

                    return res;
                }

                // fallback: login após registro
                const loginRes = await loginApi(values.email, values.password);
                if (loginRes?.token) {
                    applyToken(loginRes.token);
                    navigate(ROUTES.app);
                    
                    return loginRes;
                }

                setSubmitError("Cadastro concluído, mas não foi possível autenticar automaticamente.");

                return res;
            } catch (err) {
                setSubmitError(err?.message || "Erro inesperado. Tente novamente.");
            } finally {
                setIsSubmitting(false);
            }
        };

        return {
            // react-hook-form
            form, control, handleSubmit, watch, setValue,
            isSubmitting: isSubmitting || formState.isSubmitting,

            // ui/state
            step, setStep, goNext, goBack, canContinue, canSubmit,
            submitError, setSubmitError,

            // actions
            onSubmit,
        };
}
