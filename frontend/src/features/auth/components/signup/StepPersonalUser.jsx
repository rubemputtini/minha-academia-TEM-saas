import { useState } from "react";
import { Input } from "@/components/ui/input";
import { FormField, FormItem, FormLabel, FormControl, FormMessage } from "@/components/ui/form";
import PasswordHintPopover from "@/features/auth/components/PasswordHintPopover";
import RequiredMark from "./RequiredMark";

export default function StepPersonalUser({ control, watch }) {
    const pwd = watch("password");
    const [focused, setFocused] = useState(false);

    return (
        <div className="grid grid-cols-1 gap-5 md:grid-cols-12">
            <FormField control={control} name="name" render={({ field }) => (
                <FormItem className="md:col-span-6 space-y-2">
                    <FormLabel className="block">Nome completo<RequiredMark /></FormLabel>

                    <FormControl>
                        <Input {...field} placeholder="Seu nome" maxLength={80} look="soft" size="lg" />
                    </FormControl>

                    <FormMessage className="text-xs" />
                </FormItem>
            )} />

            <FormField control={control} name="email" render={({ field }) => (
                <FormItem className="md:col-span-6 space-y-2">
                    <FormLabel className="block">E-mail<RequiredMark /></FormLabel>

                    <FormControl>
                        <Input type="email" {...field} placeholder="voce@exemplo.com" maxLength={100} look="soft" size="lg" />
                    </FormControl>

                    <FormMessage className="text-xs" />
                </FormItem>
            )} />

            <FormField control={control} name="password" render={({ field }) => (
                <FormItem className="md:col-span-4 space-y-2">
                    <FormLabel className="block">Senha<RequiredMark /></FormLabel>

                    <FormControl>
                        <PasswordHintPopover isOpen={focused && !!pwd} password={pwd}>
                            <Input
                                type="password"
                                autoComplete="new-password"
                                {...field}
                                onFocus={() => setFocused(true)}
                                onBlur={() => setFocused(false)}
                                placeholder="••••••••"
                                look="soft" size="lg"
                            />
                        </PasswordHintPopover>
                    </FormControl>

                    <FormMessage className="text-xs" />
                </FormItem>
            )} />

            <FormField control={control} name="confirmPassword" render={({ field }) => (
                <FormItem className="md:col-span-4 space-y-2">
                    <FormLabel className="block">Confirmar senha<RequiredMark /></FormLabel>

                    <FormControl>
                        <Input type="password" {...field} placeholder="Repita a senha" look="soft" size="lg" />
                    </FormControl>

                    <FormMessage className="text-xs" />
                </FormItem>
            )} />

            <FormField control={control} name="coachCode" render={({ field }) => (
                <FormItem className="md:col-span-4 space-y-2">
                    <FormLabel className="block">Código do treinador<RequiredMark /></FormLabel>

                    <FormControl>
                        <Input {...field} placeholder="codigo-treinador" look="soft" size="lg" />
                    </FormControl>

                    <FormMessage className="text-xs" />
                </FormItem>
            )} />
        </div>
    );
}
