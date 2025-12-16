import {
    Card,
    CardHeader,
    CardTitle,
    CardContent,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";

import {
    FormField,
    FormItem,
    FormLabel,
    FormControl,
    FormMessage,
} from "@/components/ui/form";

import {
    PencilSquareIcon,
    XMarkIcon,
    CheckIcon,
} from "@heroicons/react/24/outline";

export default function CoachPersonalCard({
    control,
    isEditing,
    saving,
    isEditable,
    inputClass,
    onStartEdit,
    onCancelEdit,
}) {
    return (
        <Card
            className={`bg-card border-border transition-colors ${isEditing ? "border-primary/40" : ""
                }`}
        >
            <CardHeader className="flex flex-row items-start justify-between">
                <CardTitle className="text-lg font-semibold tracking-wide">
                    Dados pessoais
                </CardTitle>

                <div className="flex items-center gap-2">
                    {isEditing ? (
                        <>
                            <Button
                                type="button"
                                size="icon"
                                variant="outline"
                                disabled={saving}
                                onClick={onCancelEdit}
                                className="border-red-500/70 text-red-400 hover:bg-red-500/10 hover:text-red-300"
                            >
                                <XMarkIcon className="h-4 w-4" />
                            </Button>

                            <Button
                                type="submit"
                                size="icon"
                                disabled={saving}
                                className="border-emerald-500/70 bg-emerald-500/10 text-emerald-400 hover:bg-emerald-500/20 hover:text-emerald-300"
                            >
                                <CheckIcon className="h-4 w-4" />
                            </Button>
                        </>
                    ) : (
                        <Button
                            type="button"
                            size="icon"
                            variant="ghost"
                            onClick={onStartEdit}
                            className="rounded-full text-primary hover:text-primary-foreground hover:bg-primary/20"
                        >
                            <PencilSquareIcon className="size-5" />
                        </Button>
                    )}
                </div>
            </CardHeader>

            <CardContent className="grid gap-4 md:grid-cols-2">
                <FormField
                    control={control}
                    name="name"
                    render={({ field }) => (
                        <FormItem className="space-y-1.5">
                            <FormLabel className="mb-1.5 block">
                                Nome completo
                            </FormLabel>
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
                    name="email"
                    render={({ field }) => (
                        <FormItem className="space-y-1.5">
                            <FormLabel className="mb-1.5 block">E-mail</FormLabel>
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
                    name="phoneNumber"
                    render={({ field }) => (
                        <FormItem className="space-y-1.5 md:col-span-2">
                            <FormLabel className="mb-1.5 block">Telefone</FormLabel>
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
