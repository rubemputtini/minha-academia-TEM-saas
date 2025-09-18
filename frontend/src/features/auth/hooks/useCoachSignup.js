import { useEffect, useMemo, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { coachSignupSchema } from "@/features/auth/schemas/coachSignup.schema";
import { login as loginApi, registerCoach, registerCoachAfterPayment } from "@/features/auth/services/authService";
import { getSignupPrefill } from "@/features/billing/services/checkoutSessionsService";
import { useNavigate } from "react-router-dom";
import { useAuth } from "@/features/auth/hooks/useAuth";
import { ROUTES } from "@/shared/routes/routes";

const STEP1_FIELDS = ["name","email","phoneNumber","password","confirmPassword"];
const STEP2_FIELDS = ["street","number","city","state","country","postalCode"];

export function useCoachSignup({ sessionId }) {
    const [submitError, setSubmitError] = useState("");
    const [prefillError, setPrefillError] = useState("");
    const [prefilled, setPrefilled] = useState(false);
    const [planInfo, setPlanInfo] = useState({ planId: "", currency: "" });
    const [step, setStep] = useState(1);
    const navigate = useNavigate();
    const { login: applyToken } = useAuth();

    const form = useForm({
        resolver: zodResolver(coachSignupSchema),
        defaultValues: {
            name:"", email:"", phoneNumber:"",
            password:"", confirmPassword:"",
            street:"", number:"", complement:"",
            neighborhood:"", city:"", state:"",
            country:"", postalCode:"",
            latitude:null, longitude:null,
        },
        mode: "onSubmit",
    });

    const { control, handleSubmit, trigger, setValue, watch, formState:{ isSubmitting } } = form;

    useEffect(() => {
        let cancelled = false;

        async function load() {
        if (!sessionId) return;

        setPrefillError("");

        try {
            const data = await getSignupPrefill(sessionId);

            if (cancelled) return;

            if (data?.name != null) setValue("name", data.name ?? "");
            if (data?.email != null) setValue("email", data.email ?? "");
            if (data?.phoneNumber != null) setValue("phoneNumber", data.phoneNumber ?? "");

            const a = data?.address || {};

            if (a.street != null) setValue("street", a.street ?? "");
            if (a.number != null) setValue("number", a.number ?? "");
            if (a.complement != null) setValue("complement", a.complement ?? "");
            if (a.neighborhood != null) setValue("neighborhood", a.neighborhood ?? "");
            if (a.city != null) setValue("city", a.city ?? "");
            if (a.state != null) setValue("state", a.state ?? "");
            if (a.country != null) setValue("country", a.country ?? "");
            if (a.postalCode != null) setValue("postalCode", a.postalCode ?? "");

            if (data?.latitude != null)  setValue("latitude", data.latitude);
            if (data?.longitude != null) setValue("longitude", data.longitude);

            const plan = (data?.subscriptionPlan || "").toLowerCase();

            setPlanInfo({ planId: plan, currency: data?.currency || "EUR" });
            setPrefilled(true);
        } catch {
            setPrefillError("Não foi possível carregar seus dados da compra. Você pode preencher manualmente.");
        }
        }
        load();

        return () => { cancelled = true; };
    }, [sessionId, setValue]);

    const step1Values = watch(STEP1_FIELDS);
    const step2Values = watch(STEP2_FIELDS);

    const canContinue = (() => {
        const [name, email, phone, pass, confirm] = step1Values;
        const filled = [name, email, phone, pass, confirm].every(v => (v ?? "").toString().trim().length > 0);

        return filled && pass === confirm;
    })();

    const canSubmit = step2Values.every(v => (v ?? "").toString().trim().length > 0);

    const goNext = async () => {
        const ok = await trigger(STEP1_FIELDS, { shouldFocus: true });
        if (ok) setStep(2);
    };
    
    const goBack = () => setStep(1);

    const onSubmit = async ({ ...values }) => {
        const payload = { ...values };
        delete payload.confirmPassword;

        setSubmitError("");
        try {
            const res = sessionId
                ? await registerCoachAfterPayment({ sessionId, password: payload.password })
                : await registerCoach(payload);

            // Se o backend já devolver token no register:
            if (res?.token) {
                applyToken(res.token);
                navigate(ROUTES.dashboard);

                return res;
            }

            // Fallback: login
            const loginRes = await loginApi(payload.email, payload.password);

            if (loginRes?.token) {
                applyToken(loginRes.token);
                navigate(ROUTES.dashboard);

                return loginRes;
            }

            setSubmitError("Cadastro concluído, mas não foi possível autenticar automaticamente.");

            return res;
            } catch (err) {
                setSubmitError(err?.message || "Erro inesperado. Tente novamente.");
                throw err;
            }
        };

    const inputClass = useMemo(() => [
        "h-10 md:h-11 rounded-xl",
        "bg-white/[0.03]",
        "border border-white/12",
        "placeholder:text-foreground/40",
        "focus-visible:ring-2 focus-visible:ring-amber-400/80 focus-visible:border-transparent",
        "transition",
    ].join(" "), []);

    return {
        // react-hook-form
        form, control, handleSubmit, watch, setValue, isSubmitting,
        // ui/state
        step, setStep, goNext, goBack, canContinue, canSubmit,
        submitError, setSubmitError, prefillError, prefilled, planInfo,
        inputClass,
        onSubmit,
    };
}
