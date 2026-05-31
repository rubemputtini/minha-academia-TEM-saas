import { Link } from "react-router-dom";
import { Mail } from "lucide-react";

import Header from "@/shared/layout/Header";
import Footer from "@/shared/layout/Footer";

import { useForm } from "react-hook-form";
import { Form, FormField, FormItem, FormLabel, FormControl } from "@/components/ui/form";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import AlertBanner from "@/shared/components/AlertBanner";

import { useForgotPassword } from "@/features/auth/hooks/useForgotPassword";
import { ROUTES } from "@/shared/routes/routes";
import { CARD_BASE } from "@/shared/styles/cards";

export default function ForgotPasswordPage() {
    const { submitError, setSubmitError, submitSuccess, isSubmitting, submit } = useForgotPassword();

    const form = useForm({
        defaultValues: { email: "" },
        mode: "onSubmit",
    });

    const email = form.watch("email");
    const canSubmit = !!email?.trim();

    return (
        <div className="flex min-h-screen flex-col">
            <Header />

            <section className="flex flex-1 items-center justify-center px-4 py-10">
                <div className={`${CARD_BASE} relative w-full max-w-sm overflow-hidden`}>
                    <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-primary/40 to-transparent" />
                    <Mail className="pointer-events-none absolute right-5 top-5 h-24 w-24 -rotate-6 text-white opacity-[0.035]" />

                    <div className="px-7 pb-8 pt-7">
                        <div className="mb-6 space-y-1">
                            <p className="text-[10px] font-semibold uppercase tracking-[0.14em] text-muted-foreground/50">
                                Recuperação
                            </p>
                            <h1 className="text-2xl font-bold tracking-tight">Recuperar acesso</h1>
                            <p className="text-sm text-muted-foreground/70">
                                Informe seu e-mail para receber o link de redefinição.
                            </p>
                        </div>

                        <Form {...form}>
                            <form onSubmit={form.handleSubmit(submit)} className="space-y-5" noValidate>
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

                                <Button
                                    type="submit"
                                    loading={isSubmitting}
                                    disabled={isSubmitting || !canSubmit}
                                    className="w-full h-10 rounded-xl text-sm font-medium"
                                >
                                    Enviar link
                                </Button>

                                <div className="text-center">
                                    <Link
                                        to={ROUTES.login}
                                        className="text-xs text-muted-foreground/60 hover:text-foreground transition-colors"
                                    >
                                        Voltar ao login
                                    </Link>
                                </div>
                            </form>
                        </Form>
                    </div>
                </div>
            </section>

            <Footer />
        </div>
    );
}
