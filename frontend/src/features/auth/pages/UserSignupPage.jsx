import Header from "@/shared/layout/Header";
import Footer from "@/marketing/components/Footer";

import { Form } from "@/components/ui/form";
import { Card, CardHeader, CardTitle, CardDescription, CardContent } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Progress } from "@/components/ui/progress";
import { Button } from "@/components/ui/button";
import { Separator } from "@/components/ui/separator";
import AlertBanner from "@/shared/components/AlertBanner";

import { ROUTES } from "@/shared/routes/routes";
import { Link } from "react-router-dom";

import StepPersonalUser from "../components/signup/StepPersonalUser";
import StepGymUser from "../components/signup/StepGymUser";
import { useUserSignup } from "@/features/auth/hooks/useUserSignup";

export default function UserSignupPage() {
    const {
        form, control, handleSubmit,
        step, goNext, goBack, canContinue, canSubmit,
        isSubmitting, submitError, setSubmitError,
        onSubmit,
    } = useUserSignup();

    return (
        <>
            <Header />

            <section className="mx-auto max-w-3xl px-4 py-6">
                <Card variant="glass">
                    <CardHeader className="space-y-4">
                        <div className="flex items-center justify-between gap-4">
                            <div>
                                <CardTitle className="text-2xl md:text-3xl tracking-tight">
                                    Criar conta de Aluno
                                </CardTitle>

                                <CardDescription className="text-base md:text-lg mt-0.5">
                                    Informe seus dados e os da sua academia.
                                </CardDescription>
                            </div>

                            <div className="hidden md:flex items-center gap-2">
                                <Badge variant={step === 1 ? "default" : "secondary"} className="px-3 py-1">
                                    1. Dados pessoais
                                </Badge>

                                <Badge variant={step === 2 ? "default" : "secondary"} className="px-3 py-1">
                                    2. Academia
                                </Badge>
                            </div>
                        </div>

                        <Progress value={step === 1 ? 50 : 100} className="h-2 bg-white/10" />
                    </CardHeader>

                    <CardContent>
                        <Form {...form}>
                            <form onSubmit={handleSubmit(onSubmit)} className="space-y-6" noValidate>
                                {step === 1 && (
                                    <>
                                        <Separator className="bg-foreground/10" />

                                        <StepPersonalUser control={control} watch={form.watch} />

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
                                        <StepGymUser control={control} setValue={form.setValue} />

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
