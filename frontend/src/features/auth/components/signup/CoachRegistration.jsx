import { UserRoundPlus } from "lucide-react";
import { Form } from "@/components/ui/form";
import { Button } from "@/components/ui/button";
import Header from "@/shared/layout/Header";
import Footer from "@/shared/layout/Footer";
import AlertBanner from "@/shared/components/AlertBanner";
import StepPersonal from "./StepPersonal";
import StepAddress from "./StepAddress";
import { ROUTES } from "@/shared/routes/routes";
import { Link } from "react-router-dom";
import { CARD_BASE } from "@/shared/styles/cards";

const STEP_LABELS = ["Dados pessoais", "Endereço"];

export default function CoachRegistration({
    form, step, canContinue, canSubmit, goNext, goBack, onSubmit, isSubmitting,
    submitError, setSubmitError, prefillError, prefilled, planInfo,
}) {
    const planLabel =
        planInfo.planId === "unlimited" ? "Unlimited" :
        planInfo.planId === "basic" ? "Basic" : "Free";

    return (
        <div className="flex min-h-screen flex-col">
            <Header />

            <section className="flex flex-1 items-start justify-center px-4 py-10">
                <div className={`${CARD_BASE} relative w-full max-w-3xl overflow-hidden`}>
                    <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-primary/40 to-transparent" />
                    <UserRoundPlus className="pointer-events-none absolute right-6 top-6 h-28 w-28 -rotate-6 text-white opacity-[0.035]" />

                    <div className="px-7 pb-8 pt-7">
                        <div className="mb-5 space-y-1">
                            <p className="text-[10px] font-semibold uppercase tracking-[0.14em] text-muted-foreground/50">
                                Cadastro · Plano <span className="text-primary/70">{planLabel}</span>
                            </p>
                            <h1 className="text-2xl font-bold tracking-tight md:text-3xl">
                                Criar conta de Treinador
                            </h1>
                            {prefilled && !prefillError && (
                                <p className="text-sm text-emerald-400/80">Pagamento confirmado — preencha seus dados.</p>
                            )}
                            {!prefilled && (
                                <p className="text-sm text-muted-foreground/70">Informe seus dados pessoais e endereço.</p>
                            )}
                        </div>

                        <div className="mb-6 flex gap-1.5">
                            {STEP_LABELS.map((_, i) => (
                                <div
                                    key={i}
                                    className={`h-1 flex-1 rounded-full transition-colors duration-300 ${
                                        step > i ? "bg-primary" : step === i + 1 ? "bg-primary/60" : "bg-white/10"
                                    }`}
                                />
                            ))}
                        </div>

                        <div className="mb-5 flex items-center gap-2 border-t border-white/6 pt-5">
                            <span className="text-[10px] font-semibold uppercase tracking-[0.14em] text-muted-foreground/40">
                                Passo {step} de {STEP_LABELS.length}
                            </span>
                            <span className="text-[10px] text-muted-foreground/30">·</span>
                            <span className="text-[10px] text-muted-foreground/40">{STEP_LABELS[step - 1]}</span>
                        </div>

                        <Form {...form}>
                            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6" noValidate>
                                {step === 1 && (
                                    <>
                                        {prefillError && (
                                            <AlertBanner
                                                variant="error"
                                                title="Não foi possível carregar seus dados da compra"
                                                message={prefillError}
                                                onClose={() => {}}
                                            />
                                        )}

                                        <StepPersonal control={form.control} watch={form.watch} />

                                        <AlertBanner
                                            variant="error"
                                            title="Não foi possível criar sua conta"
                                            message={submitError}
                                            onClose={() => setSubmitError("")}
                                        />

                                        <div className="flex flex-col items-center gap-3">
                                            <Button
                                                type="button"
                                                onClick={goNext}
                                                disabled={!canContinue || isSubmitting}
                                                loading={isSubmitting}
                                                className="h-10 w-full max-w-xs rounded-xl text-sm font-medium"
                                            >
                                                Continuar
                                            </Button>
                                            <p className="text-xs text-muted-foreground/60">
                                                Já tem conta?{" "}
                                                <Link to={ROUTES.login} className="underline underline-offset-4 hover:text-foreground transition-colors">
                                                    Entrar
                                                </Link>
                                            </p>
                                        </div>
                                    </>
                                )}

                                {step === 2 && (
                                    <>
                                        <StepAddress control={form.control} watch={form.watch} setValue={form.setValue} />

                                        <AlertBanner
                                            variant="error"
                                            title="Não foi possível criar sua conta"
                                            message={submitError}
                                            onClose={() => setSubmitError("")}
                                        />

                                        <div className="flex flex-col items-center gap-3">
                                            <div className="flex w-full max-w-xs gap-3">
                                                <Button
                                                    type="button"
                                                    variant="outline"
                                                    onClick={goBack}
                                                    className="h-10 flex-1 rounded-xl text-sm font-medium"
                                                >
                                                    Voltar
                                                </Button>
                                                <Button
                                                    type="submit"
                                                    disabled={!canSubmit || isSubmitting}
                                                    loading={isSubmitting}
                                                    className="h-10 flex-1 rounded-xl text-sm font-medium"
                                                >
                                                    Criar conta
                                                </Button>
                                            </div>
                                            <p className="text-xs text-muted-foreground/60">
                                                Já tem conta?{" "}
                                                <Link to={ROUTES.login} className="underline underline-offset-4 hover:text-foreground transition-colors">
                                                    Entrar
                                                </Link>
                                            </p>
                                        </div>
                                    </>
                                )}
                            </form>
                        </Form>
                    </div>
                </div>
            </section>

            <Footer />
        </div>
    );
}
