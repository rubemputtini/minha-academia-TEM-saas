import { useState } from "react";
import { Link } from "react-router-dom";

import Header from "@/shared/layout/Header";
import Footer from "@/marketing/components/Footer";

import { useForm } from "react-hook-form";
import { Form, FormField, FormItem, FormLabel, FormControl } from "@/components/ui/form";
import { Card, CardHeader, CardTitle, CardDescription, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Separator } from "@/components/ui/separator";
import { Input } from "@/components/ui/input";
import AlertBanner from "@/shared/components/AlertBanner";

import { forgotPassword } from "@/features/auth/services/authService";
import { ROUTES } from "@/shared/routes/routes";

export default function ForgotPasswordPage() {
    const [submitError, setSubmitError] = useState("");
    const [submitSuccess, setSubmitSuccess] = useState("");
    const [isSubmitting, setIsSubmitting] = useState(false);

    const form = useForm({
        defaultValues: { email: "" },
        mode: "onSubmit",
    });

    const email = form.watch("email");
    const canSubmit = !!email?.trim();

    const onSubmit = async ({ email }) => {
        if (!canSubmit) return;
        setSubmitError("");
        setSubmitSuccess("");
        setIsSubmitting(true);

        try {
            const message = await forgotPassword(email);
            setSubmitSuccess(message || "Se o e-mail for válido, enviaremos um link para redefinir a senha.");
        } catch (err) {
            setSubmitError(err?.response?.data?.message || err?.message || "Não foi possível enviar agora.");
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <>
            <Header />

            <section className="mx-auto px-4 py-6">
                <Card variant="glass" className="w-full max-w-sm mx-auto">
                    <CardHeader className="space-y-2">
                        <CardTitle className="text-2xl tracking-tight">Recuperar acesso</CardTitle>
                        <CardDescription className="text-base mt-0.5">
                            Informe seu e-mail para receber o link de redefinição.
                        </CardDescription>
                    </CardHeader>

                    <CardContent>
                        <Form {...form}>
                            <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6" noValidate>
                                <Separator className="bg-foreground/10" />

                                <div className="grid grid-cols-1 gap-5">
                                    <FormField
                                        control={form.control}
                                        name="email"
                                        render={({ field }) => (
                                            <FormItem>
                                                <FormLabel className="block">E-mail</FormLabel>
                                                <FormControl>
                                                    <Input
                                                        {...field}
                                                        type="email"
                                                        inputMode="email"
                                                        autoComplete="email"
                                                        autoCapitalize="off"
                                                        spellCheck={false}
                                                        autoFocus
                                                        placeholder="voce@exemplo.com"
                                                        look="soft"
                                                        size="lg"
                                                    />
                                                </FormControl>
                                            </FormItem>
                                        )}
                                    />
                                </div>

                                <AlertBanner
                                    variant="error"
                                    title="Não foi possível enviar o e-mail"
                                    message={submitError}
                                    onClose={() => setSubmitError("")}
                                />

                                <AlertBanner
                                    variant="success"
                                    title="Tudo certo"
                                    message={submitSuccess}
                                />

                                <div className="flex flex-col items-center gap-3">
                                    <Button
                                        type="submit"
                                        loading={isSubmitting}
                                        disabled={isSubmitting || !canSubmit}
                                        className="h-10 rounded-xl px-5 text-sm md:text-base font-medium"
                                    >
                                        Enviar link
                                    </Button>

                                    <Link
                                        to={ROUTES.login}
                                        className="text-sm text-foreground/70 hover:text-foreground underline underline-offset-4"
                                    >
                                        Voltar ao login
                                    </Link>
                                </div>
                            </form>
                        </Form>
                    </CardContent>
                </Card>
            </section>

            <Footer />
        </>
    );
}
