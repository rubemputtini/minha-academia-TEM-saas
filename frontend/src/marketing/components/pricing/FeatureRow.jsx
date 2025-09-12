import { Check, Minus } from "lucide-react";
import { cn } from "@/lib/utils";

export default function FeatureRow({ label, available }) {
    return (
        <li className="flex items-start gap-3 text-sm leading-relaxed">
            <span
                className={cn(
                    "mt-0.5 inline-flex h-5 w-5 flex-none items-center justify-center rounded-full",
                    available ? "bg-emerald-400/90 text-black" : "bg-white/8"
                )}
            >
                {available ? <Check className="h-3.5 w-3.5" /> : <Minus className="h-3.5 w-3.5 opacity-60" />}
            </span>
            <span className={available ? "text-white/90" : "text-white/45 line-through"}>{label}</span>
        </li>
    );
}
