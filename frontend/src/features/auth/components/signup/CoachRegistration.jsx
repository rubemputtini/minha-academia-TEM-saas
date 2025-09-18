import { Form } from "@/components/ui/form";
import { Card, CardHeader, CardTitle, CardDescription, CardContent } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Progress } from "@/components/ui/progress";
import { Button } from "@/components/ui/button";
import { Separator } from "@/components/ui/separator";
import Header from "@/shared/layout/Header";
import Footer from "@/marketing/components/Footer";
import AlertBanner from "@/shared/components/AlertBanner";
import StepPersonal from "./StepPersonal";
import StepAddress from "./StepAddress";

export default function CoachRegistration({
    form, step, canContinue, canSubmit, goNext, goBack, onSubmit, isSubmitting,
    inputClass, submitError, setSubmitError, prefillError, prefilled, planInfo,
}) {
    return (
        <>
            <Header />
            <section className="mx-auto max-w-3xl px-4 py-6">
                <Card className="relative overflow-hidden border border-white/14 bg-card/88 backdrop-blur-sm shadow-[0_12px_40px_-18px_rgba(0,0,0,0.65),inset_0_1px_0_rgba(255,255,255,0.06)] before:pointer-events-none before:absolute before:inset-0 before:rounded-[inherit] before:bg-[linear-gradient(115deg,transparent,rgba(255,255,255,0.10)_15%,rgba(255,255,255,0.03)_40%,transparent_60%)] before:opacity-35 after:pointer-events-none after:absolute after:inset-x-0 after:top-0 after:h-10 after:rounded-t-[inherit] after:bg-[linear-gradient(to_bottom,rgba(255,255,255,0.05),transparent)]">
                    <CardHeader className="space-y-4">
                        <div className="flex items-center justify-between gap-4">
                            <div>
                                <CardTitle className="text-2xl md:text-3xl tracking-tight">
                                    Criar conta de Treinador
                                </CardTitle>
                                <CardDescription className="text-base md:text-lg mt-0.5">
                                    Plano{" "}
                                    <span className="font-semibold uppercase">
                                        {planInfo.planId === "unlimited"
                                            ? "Unlimited"
                                            : planInfo.planId === "basic"
                                                ? "Basic"
                                                : "—"}
                                    </span>

                                    <br className="sm:hidden" />

                                    {prefilled && !prefillError &&
                                        <span className="ml-0 sm:ml-2 text-emerald-400/90 uppercase">
                                            • pagamento confirmado
                                        </span>}
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
                            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-10">

                                {step === 1 && (
                                    <>
                                        {prefillError && (
                                            <AlertBanner
                                                variant="error"
                                                title="Não foi possível carregar seus dados da compra"
                                                message={prefillError}
                                                onClose={() => { }}
                                            />
                                        )}

                                        <StepPersonal control={form.control} inputClass={inputClass} watch={form.watch} />

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
                                        <StepAddress control={form.control} inputClass={inputClass} watch={form.watch} setValue={form.setValue} />

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
