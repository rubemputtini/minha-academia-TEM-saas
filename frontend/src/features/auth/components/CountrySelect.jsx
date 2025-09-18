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
        // mantém code + label
        const entries = Object.entries(
            countries.getNames("pt", { select: "official" })
        ); // [["PT","Portugal"], ...]

        return entries
            .map(([code, label]) => ({ code: code.toUpperCase(), label }))
            .sort((a, b) => a.label.localeCompare(b.label, "pt"));
    }, []);

    // compat: aceita value vindo como nome (label) ou código
    const normalizedValue = useMemo(() => {
        if (!value) return "";

        const str = String(value);
        const upper = str.toUpperCase();

        // já é um código válido?
        if (options.some(o => o.code === upper)) return upper;

        // veio como label? converte pra code
        const byLabel = options.find(o => o.label === str);

        return byLabel?.code ?? "";
    }, [value, options]);

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
        <Select value={normalizedValue} onValueChange={onChange}>
            <SelectTrigger className={`${triggerBase} ${className}`}>
                <SelectValue placeholder={placeholder} />
            </SelectTrigger>

            <SelectContent
                position="popper"
                align="start"
                className={`${contentBase} max-h-72 overflow-y-auto`}
            >

                {options.map(({ code, label }) => (
                    <SelectItem key={code} value={code} className={itemBase}>
                        {label}
                    </SelectItem>
                ))}

            </SelectContent>
        </Select>
    );
}
