import { Form } from "@/components/ui/form";
import { Card, CardHeader, CardTitle, CardDescription, CardContent } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Progress } from "@/components/ui/progress";
import { Button } from "@/components/ui/button";
import { Separator } from "@/components/ui/separator";
import Header from "@/shared/layout/Header";
import Footer from "@/shared/layout/Footer";
import AlertBanner from "@/shared/components/AlertBanner";
import StepPersonal from "./StepPersonal";
import StepAddress from "./StepAddress";
import { ROUTES } from "@/shared/routes/routes";
import { Link } from "react-router-dom";

export default function CoachRegistration({
    form, step, canContinue, canSubmit, goNext, goBack, onSubmit, isSubmitting,
    submitError, setSubmitError, prefillError, prefilled, planInfo,
}) {
    return (
        <>
            <Header />
            <section className="mx-auto max-w-3xl px-4 py-6">
                <Card variant="glass">
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
                                                : "Free"}
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
                            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6" noValidate>

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

                                        <Separator className="bg-foreground/10" />
                                        <StepPersonal control={form.control} watch={form.watch} />

                                        <AlertBanner
                                            variant="error"
                                            title="Não foi possível criar sua conta"
                                            message={submitError}
                                            onClose={() => setSubmitError("")}
                                        />

                                        <div className="flex flex-col gap-3 justify-center items-center">
                                            <Button
                                                type="button"
                                                onClick={goNext}
                                                disabled={!canContinue || isSubmitting}
                                                loading={isSubmitting}
                                                className="h-10 rounded-xl px-5 text-sm md:text-base font-medium"
                                            >
                                                Continuar
                                            </Button>

                                            <p className="text-sm text-foreground/60">
                                                Já tem conta?{" "}
                                                <Link to={ROUTES.login} className="underline underline-offset-4 hover:opacity-90">
                                                    Entrar
                                                </Link>
                                            </p>
                                        </div>

                                    </>
                                )}

                                {step === 2 && (
                                    <>
                                        <Separator className="bg-foreground/10" />
                                        <StepAddress control={form.control} watch={form.watch} setValue={form.setValue} />

                                        <AlertBanner
                                            variant="error"
                                            title="Não foi possível criar sua conta"
                                            message={submitError}
                                            onClose={() => setSubmitError("")}
                                        />

                                        <div className="flex flex-col gap-3 justify-center items-center">
                                            <div className="flex gap-3">
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

                                            <p className="text-sm text-foreground/60">
                                                Já tem conta?{" "}
                                                <Link to={ROUTES.login} className="underline underline-offset-4 hover:opacity-90">
                                                    Entrar
                                                </Link>
                                            </p>
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
