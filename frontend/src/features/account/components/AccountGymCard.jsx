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

export default function AccountGymCard({
    control,
    isEditing,
    fieldEditable,
    inputClass,
}) {
    return (
        <Card
            className={`bg-card border-border transition-colors ${isEditing ? "border-primary/40" : ""}`}
        >
            <CardHeader>
                <CardTitle className="text-lg font-semibold tracking-wide">
                    Dados da academia
                </CardTitle>
            </CardHeader>

            <CardContent className="grid gap-4 md:grid-cols-2">
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
