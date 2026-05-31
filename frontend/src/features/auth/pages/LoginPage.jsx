import { Link } from "react-router-dom";
import { LogIn } from "lucide-react";

import Header from "@/shared/layout/Header";
import Footer from "@/shared/layout/Footer";

import { useForm } from "react-hook-form";
import { Form, FormField, FormItem, FormLabel, FormControl } from "@/components/ui/form";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import AlertBanner from "@/shared/components/AlertBanner";

import { useLogin } from "@/features/auth/hooks/useLogin";
import { ROUTES } from "@/shared/routes/routes";
import { CARD_BASE } from "@/shared/styles/cards";

export default function LoginPage() {
    const { submitError, setSubmitError, isSubmitting, submit } = useLogin();

    const form = useForm({
        defaultValues: { email: "", password: "" },
        mode: "onSubmit",
    });

    const email = form.watch("email");
    const password = form.watch("password");
    const canSubmit = !!email?.trim() && !!password?.trim();

    return (
        <div className="flex min-h-screen flex-col">
            <Header />

            <section className="flex flex-1 items-center justify-center px-4 py-10">
                <div className={`${CARD_BASE} relative w-full max-w-sm overflow-hidden`}>
                    <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-primary/40 to-transparent" />
                    <LogIn className="pointer-events-none absolute right-5 top-5 h-24 w-24 -rotate-6 text-white opacity-[0.035]" />

                    <div className="px-7 pb-8 pt-7">
                        <div className="mb-6 space-y-1">
                            <p className="text-[10px] font-semibold uppercase tracking-[0.14em] text-muted-foreground/50">
                                Acesso
                            </p>
                            <h1 className="text-2xl font-bold tracking-tight">Bem-vindo de volta!</h1>
                            <p className="text-sm text-muted-foreground/70">Acesse sua conta.</p>
                        </div>

                        <Form {...form}>
                            <form onSubmit={form.handleSubmit(submit)} className="space-y-5" noValidate>
                                <div className="grid grid-cols-1 gap-4">
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
                                                        autoFocus
                                                        placeholder="seuemail@exemplo.com"
                                                        look="soft"
                                                        size="lg"
                                                    />
                                                </FormControl>
                                            </FormItem>
                                        )}
                                    />

                                    <FormField
                                        control={form.control}
                                        name="password"
                                        render={({ field }) => (
                                            <FormItem>
                                                <div className="flex items-center justify-between">
                                                    <FormLabel>Senha</FormLabel>
                                                    <Link
                                                        to={ROUTES.forgotPassword}
                                                        className="text-xs text-muted-foreground/60 hover:text-foreground transition-colors"
                                                    >
                                                        Esqueci minha senha
                                                    </Link>
                                                </div>

                                                <FormControl>
                                                    <Input
                                                        {...field}
                                                        type="password"
                                                        autoComplete="current-password"
                                                        placeholder="••••••••"
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
                                    title="Não foi possível entrar"
                                    message={submitError}
                                    onClose={() => setSubmitError("")}
                                />

                                <Button
                                    type="submit"
                                    loading={isSubmitting}
                                    disabled={isSubmitting || !canSubmit}
                                    className="w-full h-10 rounded-xl text-sm font-medium"
                                >
                                    Entrar
                                </Button>
                            </form>
                        </Form>
                    </div>
                </div>
            </section>

            <Footer />
        </div>
    );
}
