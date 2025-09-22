import { useMemo, useState } from "react";
import { useLocation, Link, useNavigate } from "react-router-dom";

import Header from "@/shared/layout/Header";
import Footer from "@/shared/layout/Footer";

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";

import { Form, FormField, FormItem, FormLabel, FormControl, FormMessage } from "@/components/ui/form";
import { Card, CardHeader, CardTitle, CardDescription, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Separator } from "@/components/ui/separator";
import { Input } from "@/components/ui/input";
import AlertBanner from "@/shared/components/AlertBanner";
import PasswordHintPopover from "@/features/auth/components/PasswordHintPopover";

import { resetPassword, login } from "@/features/auth/services/authService";
import { useAuth } from "@/features/auth/hooks/useAuth";
import { ROUTES } from "@/shared/routes/routes";
import { getHomeForRole } from "@/shared/routes/getHomeForRole";
import { resetPasswordSchema } from "../schemas/resetPassword.schema";

export default function ResetPasswordPage() {
    const location = useLocation();
    const navigate = useNavigate();
    const { login: applyToken } = useAuth();

    const params = useMemo(() => new URLSearchParams(location.search), [location.search]);
    const initialEmail = params.get("email") || "";
    const token = params.get("token") || "";

    const [submitError, setSubmitError] = useState("");
    const [submitSuccess, setSubmitSuccess] = useState("");
    const [isSubmitting, setIsSubmitting] = useState(false);

    const form = useForm({
        resolver: zodResolver(resetPasswordSchema),
        mode: "onChange",
        defaultValues: {
            email: initialEmail,
            token,
            newPassword: "",
            confirmPassword: "",
        },
    });

    const canSubmit = form.formState.isValid;
    const missingToken = !token;

    const pwd = form.watch("newPassword");
    const [pwdFocused, setPwdFocused] = useState(false);

    const onSubmit = async (values) => {
        if (!canSubmit || missingToken) return;

        setSubmitError("");
        setSubmitSuccess("");
        setIsSubmitting(true);

        // 1) Redefinir senha
        let resetMsg = "";
        try {
            resetMsg = await resetPassword({
                email: values.email,
                token: values.token,
                newPassword: values.newPassword,
            });
        } catch (err) {
            setSubmitError(err?.response?.data?.message || err?.message || "Não foi possível redefinir agora.");
            setIsSubmitting(false);

            return;
        }

        // 2) Auto-login (se falhar, mostra sucesso do reset + erro do login)
        try {
            const response = await login(values.email, values.newPassword);
            await applyToken(response?.token);

            const role = response?.role;

            const target =
                location.state?.from ||
                response?.redirectTo ||
                getHomeForRole(role);

            navigate(target, { replace: true });
        } catch (err) {
            setSubmitSuccess(resetMsg);
            setSubmitError(
                err?.response?.data?.message ||
                err?.message ||
                "Senha redefinida, mas não foi possível entrar automaticamente. Faça login."
            );
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
                        <CardTitle className="text-2xl tracking-tight">Redefinir senha</CardTitle>
                        <CardDescription className="text-base mt-0.5">
                            Crie uma nova senha para sua conta.
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
                                                        readOnly={!!initialEmail}
                                                        placeholder="voce@exemplo.com"
                                                        look="soft"
                                                        size="lg"
                                                    />
                                                </FormControl>
                                                <FormMessage />
                                            </FormItem>
                                        )}
                                    />

                                    {/* token oculto, mas validado (entra no RHF/Zod e no submit) */}
                                    <input type="hidden" value={token} {...form.register("token")} />

                                    <FormField
                                        control={form.control}
                                        name="newPassword"
                                        render={({ field }) => (
                                            <FormItem>
                                                <FormLabel className="block">Nova senha</FormLabel>
                                                <FormControl>
                                                    <PasswordHintPopover isOpen={pwdFocused && !!pwd} password={pwd}>
                                                        <Input
                                                            {...field}
                                                            type="password"
                                                            autoComplete="new-password"
                                                            placeholder="••••••••"
                                                            onFocus={() => setPwdFocused(true)}
                                                            onBlur={() => setPwdFocused(false)}
                                                            look="soft"
                                                            size="lg"
                                                        />
                                                    </PasswordHintPopover>
                                                </FormControl>
                                                <FormMessage />
                                            </FormItem>
                                        )}
                                    />

                                    <FormField
                                        control={form.control}
                                        name="confirmPassword"
                                        render={({ field }) => (
                                            <FormItem>
                                                <FormLabel className="block">Confirmar nova senha</FormLabel>
                                                <FormControl>
                                                    <Input
                                                        {...field}
                                                        type="password"
                                                        autoComplete="new-password"
                                                        placeholder="Repita a nova senha"
                                                        look="soft"
                                                        size="lg"
                                                    />
                                                </FormControl>
                                                <FormMessage />
                                            </FormItem>
                                        )}
                                    />
                                </div>

                                <AlertBanner
                                    variant="warning"
                                    title="Link de redefinição"
                                    message={missingToken ? "Link inválido ou expirado. Solicite um novo e-mail de redefinição." : ""}
                                    compact
                                />

                                <AlertBanner
                                    variant="error"
                                    title="Não foi possível concluir"
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
                                        disabled={isSubmitting || !canSubmit || missingToken}
                                        className="h-10 rounded-xl px-5 text-sm md:text-base font-medium"
                                    >
                                        Redefinir senha
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
