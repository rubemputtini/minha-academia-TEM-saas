import { useState } from "react";
import { Input } from "@/components/ui/input";
import { FormField, FormItem, FormLabel, FormControl, FormMessage } from "@/components/ui/form";
import PasswordHintPopover from "@/features/auth/components/PasswordHintPopover";
import RequiredMark from "./RequiredMark";

export default function StepPersonal({ control, inputClass, watch }) {
    const pwd = watch("password");
    const [focused, setFocused] = useState(false);

    return (
        <div className="grid grid-cols-1 gap-5 md:grid-cols-12">
            <FormField control={control} name="name" render={({ field }) => (
                <FormItem className="md:col-span-6 space-y-2">
                    <FormLabel className="block">Nome completo<RequiredMark /></FormLabel>
                    <FormControl>
                        <Input {...field} placeholder="Seu nome" maxLength={80} className={inputClass} />
                    </FormControl>
                    <FormMessage className="text-xs" />
                </FormItem>
            )} />

            <FormField control={control} name="email" render={({ field }) => (
                <FormItem className="md:col-span-6 space-y-2">
                    <FormLabel className="block">E-mail<RequiredMark /></FormLabel>
                    <FormControl>
                        <Input type="email" {...field} placeholder="voce@exemplo.com" maxLength={100} className={inputClass} />
                    </FormControl>
                    <FormMessage className="text-xs" />
                </FormItem>
            )} />

            <FormField control={control} name="phoneNumber" render={({ field }) => (
                <FormItem className="md:col-span-4 space-y-2">
                    <FormLabel className="block">Telefone<RequiredMark /></FormLabel>
                    <FormControl>
                        <Input type="tel" {...field} placeholder="+55 61 99999-9999" className={inputClass} />
                    </FormControl>
                    <p className="mt-1 text-[11px] text-foreground/60">Inclua DDI e DDD.</p>
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
                                className={inputClass}
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
                        <Input type="password" {...field} placeholder="Repita a senha" className={inputClass} />
                    </FormControl>
                    <FormMessage className="text-xs" />
                </FormItem>
            )} />
        </div>
    );
}
