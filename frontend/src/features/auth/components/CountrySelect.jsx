import { useMemo } from "react";
import countries from "i18n-iso-countries";
import pt from "i18n-iso-countries/langs/pt.json";
import {
    Select,
    SelectTrigger,
    SelectContent,
    SelectItem,
    SelectValue,
} from "@/components/ui/select";

countries.registerLocale(pt);

export default function CountrySelect({
    value,
    onChange,
    placeholder = "Selecionar país",
    className = "",
}) {
    const options = useMemo(() => {
        const names = countries.getNames("pt", { select: "official" });

        return Object.values(names).sort((a, b) => a.localeCompare(b, "pt"));
    }, []);

    const triggerBase =
        "w-full h-10 md:h-11 rounded-xl bg-white/[0.03] border border-white/12 " +
        "placeholder:text-foreground/40 text-left " +
        "focus-visible:ring-2 focus-visible:ring-amber-400/80 focus-visible:border-transparent transition";

    const contentBase =
        "w-[--radix-select-trigger-width] rounded-xl border border-white/12 bg-card/95 shadow-lg overflow-hidden";

    const itemBase =
        "px-3 py-2 text-sm outline-none cursor-pointer " +
        "data-[highlighted]:bg-white/10 data-[highlighted]:text-white " +
        "data-[state=checked]:bg-amber-500/20 data-[state=checked]:text-amber-300";

    return (
        <Select value={value || ""} onValueChange={onChange}>
            <SelectTrigger className={`${triggerBase} ${className}`}>
                <SelectValue placeholder={placeholder} />
            </SelectTrigger>

            <SelectContent
                position="popper"
                align="start"
                className={`${contentBase} max-h-72 overflow-y-auto`}
            >

                {options.map((label) => (
                    <SelectItem key={label} value={label} className={itemBase}>
                        {label}
                    </SelectItem>
                ))}
            </SelectContent>
        </Select>
    );
}
