import {
    Card,
    CardHeader,
    CardTitle,
    CardContent,
} from "@/components/ui/card";

import { Input } from "@/components/ui/input";

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
        <Card
            className={`bg-card border-border transition-colors ${isEditing ? "border-primary/40" : ""
                }`}
        >
            <CardHeader>
                <CardTitle className="text-lg font-semibold tracking-wide">
                    Endereço
                </CardTitle>
            </CardHeader>

            <CardContent className="grid gap-4 md:grid-cols-3">
                {/* Rua */}
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

                {/* Número */}
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

                {/* Complemento */}
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

                {/* Bairro */}
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

                {/* Cidade */}
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

                {/* Estado */}
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

                {/* Código postal */}
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

                {/* País */}
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
