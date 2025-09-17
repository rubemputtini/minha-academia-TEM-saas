import { useState, useMemo } from "react";
import { useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";

import { coachSignupSchema } from "@/features/auth/schemas/coachSignup.schema";
import { registerCoach } from "@/features/auth/services/authService";
import { useAuth } from "@/features/auth/hooks/useAuth";
import { ROUTES } from "@/shared/routes/routes";

import {
    Card, CardHeader, CardTitle, CardDescription, CardContent,
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Separator } from "@/components/ui/separator";
import { Badge } from "@/components/ui/badge";
import { Progress } from "@/components/ui/progress";
import { Form } from "@/components/ui/form";

import Header from "@/shared/layout/Header";
import Footer from "@/marketing/components/Footer";

import StepPersonal from "@/features/auth/components/signup/StepPersonal";
import StepAddress from "@/features/auth/components/signup/StepAddress";
import AlertBanner from "@/shared/components/AlertBanner";

const STEP1_FIELDS = ["name", "email", "phoneNumber", "password", "confirmPassword"];
const STEP2_FIELDS = ["street", "number", "city", "state", "country", "postalCode"];

export default function CoachSignupPage() {
    const navigate = useNavigate();
    const { login: applyToken } = useAuth();

    const [submitError, setSubmitError] = useState("");
    const [step, setStep] = useState(1);

    const form = useForm({
        resolver: zodResolver(coachSignupSchema),
        defaultValues: {
            name: "", email: "", phoneNumber: "",
            password: "", confirmPassword: "",
            street: "", number: "", complement: "",
            neighborhood: "", city: "", state: "",
            country: "", postalCode: "",
            latitude: null, longitude: null,
        },
        mode: "onSubmit",
    });

    const {
        control, handleSubmit, trigger, setValue, watch,
        formState: { isSubmitting },
    } = form;

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

    const onSubmit = async (values) => {
        setSubmitError("");
        try {
            const { confirmPassword: _CONFIRM, ...payload } = values;
            const res = await registerCoach(payload);

            applyToken(res.token);
            navigate(ROUTES.dashboard);
        } catch (err) {
            setSubmitError(err?.message || "Erro inesperado. Tente novamente.");
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

    return (
        <>
            <Header />

            <section className="mx-auto max-w-3xl px-4 py-6">
                <Card
                    className="
            relative overflow-hidden
            border border-white/14
            bg-card/88
            backdrop-blur-sm
            shadow-[0_12px_40px_-18px_rgba(0,0,0,0.65),inset_0_1px_0_rgba(255,255,255,0.06)]
            before:pointer-events-none before:absolute before:inset-0 before:rounded-[inherit]
            before:bg-[linear-gradient(115deg,transparent,rgba(255,255,255,0.10)_15%,rgba(255,255,255,0.03)_40%,transparent_60%)]
            before:opacity-35
            after:pointer-events-none after:absolute after:inset-x-0 after:top-0 after:h-10 after:rounded-t-[inherit]
            after:bg-[linear-gradient(to_bottom,rgba(255,255,255,0.05),transparent)]
          "
                >
                    <CardHeader className="space-y-4">
                        <div className="flex items-center justify-between gap-4">
                            <div>
                                <CardTitle className="text-2xl md:text-3xl tracking-tight">
                                    Criar conta de Treinador
                                </CardTitle>
                                <CardDescription className="text-base md:text-lg mt-0.5">
                                    Plano <span className="font-semibold uppercase">Free</span>
                                </CardDescription>
                            </div>

                            <div className="hidden md:flex items-center gap-2">
                                <Badge variant={step === 1 ? "default" : "secondary"} className="px-3 py-1">1. Dados pessoais</Badge>
                                <Badge variant={step === 2 ? "default" : "secondary"} className="px-3 py-1">2. Endereço</Badge>
                            </div>
                        </div>

                        <Progress value={step === 1 ? 50 : 100} className="h-2 bg-white/10" />
                    </CardHeader>

                    <CardContent>
                        <Form {...form}>
                            <form onSubmit={handleSubmit(onSubmit)} className="space-y-10">

                                {step === 1 && (
                                    <>
                                        <StepPersonal control={control} inputClass={inputClass} watch={watch} />

                                        <AlertBanner
                                            variant="error"
                                            title="Não foi possível criar sua conta"
                                            message={submitError}
                                            onClose={() => setSubmitError("")}
                                        />

                                        <div className="flex justify-end">
                                            <Button
                                                type="button"
                                                onClick={goNext}
                                                disabled={!canContinue || isSubmitting}
                                                className="h-10 rounded-xl px-5 text-sm md:text-base font-medium"
                                            >
                                                Continuar
                                            </Button>
                                        </div>
                                    </>
                                )}

                                {step === 2 && (
                                    <>
                                        <Separator className="bg-foreground/10" />
                                        <StepAddress
                                            control={control}
                                            inputClass={inputClass}
                                            watch={watch}
                                            setValue={setValue}
                                        />

                                        <AlertBanner
                                            variant="error"
                                            title="Não foi possível criar sua conta"
                                            message={submitError}
                                            onClose={() => setSubmitError("")}
                                        />

                                        <div className="flex flex-col gap-3 sm:flex-row sm:justify-between">
                                            <Button
                                                type="button"
                                                variant="outline"
                                                onClick={goBack}
                                                className="h-10 rounded-xl px-5 text-sm md:text-base font-medium"
                                            >
                                                Voltar
                                            </Button>
                                            <Button
                                                type="submit"
                                                disabled={!canSubmit || isSubmitting}
                                                loading={isSubmitting}
                                                className="h-10 rounded-xl px-5 text-sm md:text-base font-medium"
                                            >
                                                Criar conta
                                            </Button>
                                        </div>
                                    </>
                                )}
                            </form>
                        </Form>
                    </CardContent>
                </Card>
            </section>

            <Footer />
        </>
    );
}
