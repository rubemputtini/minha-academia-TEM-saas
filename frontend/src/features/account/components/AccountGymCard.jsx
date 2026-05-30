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

export default function AccountGymCard({
    control,
    isEditing,
    fieldEditable,
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
                    Dados da academia
                </CardTitle>
            </CardHeader>

            <CardContent className="grid gap-4 md:grid-cols-2 relative z-10">
                <FormField
                    control={control}
                    name="gymName"
                    render={({ field }) => (
                        <FormItem className="space-y-1.5 md:col-span-2">
                            <FormLabel className="mb-1.5 block">
                                Nome da academia
                            </FormLabel>
                            <FormControl>
                                <Input
                                    {...field}
                                    disabled={!fieldEditable}
                                    className={inputClass(fieldEditable)}
                                />
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    )}
                />

                <FormField
                    control={control}
                    name="gymCity"
                    render={({ field }) => (
                        <FormItem className="space-y-1.5">
                            <FormLabel className="mb-1.5 block">Cidade</FormLabel>
                            <FormControl>
                                <Input
                                    {...field}
                                    disabled={!fieldEditable}
                                    className={inputClass(fieldEditable)}
                                />
                            </FormControl>
                            <FormMessage />
                        </FormItem>
                    )}
                />

                <FormField
                    control={control}
                    name="gymCountry"
                    render={({ field }) => (
                        <FormItem className="space-y-1.5">
                            <FormLabel className="mb-1.5 block">País</FormLabel>
                            <FormControl>
                                <Input
                                    {...field}
                                    disabled={!fieldEditable}
                                    className={inputClass(fieldEditable)}
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
