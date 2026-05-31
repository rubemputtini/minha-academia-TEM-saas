import { useMemo, useState } from "react";
import { useLocation, Link } from "react-router-dom";
import { KeyRound } from "lucide-react";

import Header from "@/shared/layout/Header";
import Footer from "@/shared/layout/Footer";

import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";

import { Form, FormField, FormItem, FormLabel, FormControl, FormMessage } from "@/components/ui/form";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import AlertBanner from "@/shared/components/AlertBanner";
import PasswordHintPopover from "@/features/auth/components/PasswordHintPopover";

import { useResetPassword } from "@/features/auth/hooks/useResetPassword";
import { ROUTES } from "@/shared/routes/routes";
import { resetPasswordSchema } from "../schemas/resetPassword.schema";
import { CARD_BASE } from "@/shared/styles/cards";

export default function ResetPasswordPage() {
    const location = useLocation();
    const { submitError, setSubmitError, submitSuccess, isSubmitting, submit } = useResetPassword();

    const params = useMemo(() => new URLSearchParams(location.search), [location.search]);
    const initialEmail = params.get("email") || "";
    const token = params.get("token") || "";

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

    return (
        <div className="flex min-h-screen flex-col">
            <Header />

            <section className="flex flex-1 items-center justify-center px-4 py-10">
                <div className={`${CARD_BASE} relative w-full max-w-sm overflow-hidden`}>
                    <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-primary/40 to-transparent" />
                    <KeyRound className="pointer-events-none absolute right-5 top-5 h-24 w-24 -rotate-6 text-white opacity-[0.035]" />

                    <div className="px-7 pb-8 pt-7">
                        <div className="mb-6 space-y-1">
                            <p className="text-[10px] font-semibold uppercase tracking-[0.14em] text-muted-foreground/50">
                                Segurança
                            </p>
                            <h1 className="text-2xl font-bold tracking-tight">Redefinir senha</h1>
                            <p className="text-sm text-muted-foreground/70">
                                Crie uma nova senha para sua conta.
                            </p>
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

                                <Button
                                    type="submit"
                                    loading={isSubmitting}
                                    disabled={isSubmitting || !canSubmit || missingToken}
                                    className="w-full h-10 rounded-xl text-sm font-medium"
                                >
                                    Redefinir senha
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
