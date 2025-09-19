import { useState } from "react";
import { useLocation, useNavigate, Link } from "react-router-dom";

import Header from "@/shared/layout/Header";
import Footer from "@/marketing/components/Footer";

import { useForm } from "react-hook-form";
import { Form, FormField, FormItem, FormLabel, FormControl } from "@/components/ui/form";
import { Card, CardHeader, CardTitle, CardDescription, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Separator } from "@/components/ui/separator";
import { Input } from "@/components/ui/input";
import AlertBanner from "@/shared/components/AlertBanner";

import { login as loginApi } from "@/features/auth/services/authService";
import { useAuth } from "@/features/auth/hooks/useAuth";
import { ROUTES } from "@/shared/routes/routes";
import { getHomeForRole } from "@/shared/routes/getHomeForRole";

export default function LoginPage() {
    const navigate = useNavigate();
    const location = useLocation();
    const { login: applyToken } = useAuth();

    const [submitError, setSubmitError] = useState("");
    const [isSubmitting, setIsSubmitting] = useState(false);

    const form = useForm({
        defaultValues: { email: "", password: "" },
        mode: "onSubmit",
    });

    // desabilitar "Entrar" enquanto vazio
    const email = form.watch("email");
    const password = form.watch("password");
    const canSubmit = !!email?.trim() && !!password?.trim();

    const onSubmit = async ({ email, password }) => {
        setSubmitError("");
        setIsSubmitting(true);

        try {
            const response = await loginApi(email, password);
            await applyToken(response?.token);

            const role = response?.role;

            const target =
                location.state?.from ||
                response?.redirectTo ||
                getHomeForRole(role);

            navigate(target, { replace: true });
        } catch (err) {
            setSubmitError(err?.response?.data?.message || err?.message || "Não foi possível entrar agora.");
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
                        <CardTitle className="text-2xl tracking-tight">Bem-vindo de volta!</CardTitle>
                        <CardDescription className="text-base mt-0.5">Acesse sua conta.</CardDescription>
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
                                                        className="text-sm text-foreground/70 hover:text-foreground underline underline-offset-4"
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

                                <div className="flex justify-center">
                                    <Button
                                        type="submit"
                                        loading={isSubmitting}
                                        disabled={isSubmitting || !canSubmit}
                                        className="h-10 rounded-xl px-5 text-sm md:text-base font-medium"
                                    >
                                        Entrar
                                    </Button>
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
