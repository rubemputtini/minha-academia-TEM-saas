import { Input } from "@/components/ui/input";
import { FormField, FormItem, FormLabel, FormControl, FormMessage } from "@/components/ui/form";
import CountrySelect from "@/features/auth/components/CountrySelect";
import { Button } from "@/components/ui/button";
import RequiredMark from "./RequiredMark";
import { GYM_SUGGESTIONS } from "../../utils/gymSuggestion";

export default function StepGymUser({ control, setValue }) {
    const apply = (name) =>
        setValue?.("gymName", name, { shouldValidate: true, shouldDirty: true });

    return (
        <div className="grid grid-cols-1 gap-5 md:grid-cols-12">
            <FormField
                control={control}
                name="gymName"
                render={({ field }) => (
                    <FormItem className="md:col-span-12 space-y-2">
                        <FormLabel className="block">
                            Nome da academia<RequiredMark />
                        </FormLabel>

                        <FormControl>
                            <Input
                                {...field}
                                placeholder="ex.: Smart Fit"
                                maxLength={100}
                                look="soft"
                                size="lg"
                            />
                        </FormControl>

                        <div className="mt-3">
                            <div className="
                                mx-auto
                                max-w-[22rem] sm:max-w-[28rem] md:max-w-2xl
                                flex flex-wrap justify-center
                                gap-x-1.5 gap-y-2 md:gap-x-2 md:gap-y-2.5
                            ">
                                {GYM_SUGGESTIONS.map((g) => {
                                    const active = (field.value || "").trim().toLowerCase() === g.toLowerCase();

                                    return (
                                        <Button
                                            key={g}
                                            type="button"
                                            variant="outline"
                                            size="sm"
                                            onClick={() => apply(g)}
                                            className={[
                                                "rounded-full transition",
                                                "h-7 px-2.5 text-[11px] md:h-8 md:px-3 md:text-xs",
                                                "border-white/10 bg-white/[0.03] hover:bg-white/[0.06]",
                                                "focus-visible:ring-2 focus-visible:ring-amber-400/70",
                                                active
                                                    ? "border-amber-400/40 bg-amber-400/10 text-amber-300"
                                                    : "text-foreground/80"
                                            ].join(" ")}
                                            aria-pressed={active}
                                        >
                                            {g}
                                        </Button>
                                    );
                                })}
                            </div>

                            <FormMessage className="mt-2 text-center text-xs" />
                        </div>
                    </FormItem>
                )}
            />

            <FormField
                control={control}
                name="gymCity"
                render={({ field }) => (
                    <FormItem className="md:col-span-6 space-y-2">
                        <FormLabel className="block">
                            Cidade<RequiredMark />
                        </FormLabel>

                        <FormControl>
                            <Input {...field} placeholder="ex.: Lisboa" maxLength={60} look="soft" size="lg" />
                        </FormControl>
                        <FormMessage className="text-xs" />
                    </FormItem>
                )}
            />

            <FormField
                control={control}
                name="gymCountry"
                render={({ field }) => (
                    <FormItem className="md:col-span-6 space-y-2">
                        <FormLabel className="block">
                            País<RequiredMark />
                        </FormLabel>

                        <FormControl>
                            <CountrySelect value={field.value} onChange={field.onChange} look="soft" size="lg" />
                        </FormControl>
                        <FormMessage className="text-xs" />
                    </FormItem>
                )}
            />
        </div>
    );
}
