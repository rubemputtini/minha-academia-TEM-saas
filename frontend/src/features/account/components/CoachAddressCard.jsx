import {
    Card,
    CardHeader,
    CardTitle,
    CardContent,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { cn } from "@/lib/utils";
import { CARD_BASE } from "@/shared/styles/cards";

import {
    FormField,
    FormItem,
    FormLabel,
    FormControl,
    FormMessage,
} from "@/components/ui/form";

export default function CoachAddressCard({
    control,
    isEditing,
    isEditable,
    inputClass,
}) {
    return (
        <Card className={cn(CARD_BASE, "relative overflow-hidden")}>
            <div
                className={cn(
                    "pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r transition-all",
                    isEditing
                        ? "from-transparent via-primary/60 to-transparent"
                        : "from-transparent via-white/10 to-transparent"
                )}
            />

            <CardHeader className="relative z-10">
                <CardTitle className="text-lg font-semibold tracking-wide">
                    Endereço
                </CardTitle>
            </CardHeader>

            <CardContent className="grid gap-4 md:grid-cols-3 relative z-10">
                <FormField
                    control={control}
                    name="street"
                    render={({ field }) => (
                        <FormItem className="space-y-1.5 md:col-span-3">
                            <FormLabel className="mb-1.5 block">Rua</FormLabel>
                            <FormControl>
                                <Input
                                    {...field}
                                    disabled={!isEditable}
                                    className={inputClass(isEditable)}
                                />
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    )}
                />

                <FormField
                    control={control}
                    name="number"
                    render={({ field }) => (
                        <FormItem className="space-y-1.5 md:col-span-1">
                            <FormLabel className="mb-1.5 block">Número</FormLabel>
                            <FormControl>
                                <Input
                                    {...field}
                                    disabled={!isEditable}
                                    className={inputClass(isEditable)}
                                />
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    )}
                />

                <FormField
                    control={control}
                    name="complement"
                    render={({ field }) => (
                        <FormItem className="space-y-1.5 md:col-span-2">
                            <FormLabel className="mb-1.5 block">Complemento</FormLabel>
                            <FormControl>
                                <Input
                                    {...field}
                                    disabled={!isEditable}
                                    className={inputClass(isEditable)}
                                />
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    )}
                />

                <FormField
                    control={control}
                    name="neighborhood"
                    render={({ field }) => (
                        <FormItem className="space-y-1.5 md:col-span-1">
                            <FormLabel className="mb-1.5 block">Bairro</FormLabel>
                            <FormControl>
                                <Input
                                    {...field}
                                    disabled={!isEditable}
                                    className={inputClass(isEditable)}
                                />
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    )}
                />

                <FormField
                    control={control}
                    name="city"
                    render={({ field }) => (
                        <FormItem className="space-y-1.5 md:col-span-2">
                            <FormLabel className="mb-1.5 block">Cidade</FormLabel>
                            <FormControl>
                                <Input
                                    {...field}
                                    disabled={!isEditable}
                                    className={inputClass(isEditable)}
                                />
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    )}
                />

                <FormField
                    control={control}
                    name="state"
                    render={({ field }) => (
                        <FormItem className="space-y-1.5 md:col-span-1">
                            <FormLabel className="mb-1.5 block">Estado</FormLabel>
                            <FormControl>
                                <Input
                                    {...field}
                                    disabled={!isEditable}
                                    className={inputClass(isEditable)}
                                />
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    )}
                />

                <FormField
                    control={control}
                    name="postalCode"
                    render={({ field }) => (
                        <FormItem className="space-y-1.5 md:col-span-1">
                            <FormLabel className="mb-1.5 block">Código postal</FormLabel>
                            <FormControl>
                                <Input
                                    {...field}
                                    disabled={!isEditable}
                                    className={inputClass(isEditable)}
                                />
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    )}
                />

                <FormField
                    control={control}
                    name="country"
                    render={({ field }) => (
                        <FormItem className="space-y-1.5 md:col-span-1">
                            <FormLabel className="mb-1.5 block">País</FormLabel>
                            <FormControl>
                                <Input
                                    {...field}
                                    disabled={!isEditable}
                                    className={inputClass(isEditable)}
                                />
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    )}
                />
            </CardContent>
        </Card>
    );
}
