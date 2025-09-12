import { Users } from "lucide-react";
import { Badge } from "@/components/ui/badge";
import { cn } from "@/lib/utils";

export default function ClientsChip({ text, muted = false }) {
    return (
        <Badge
            variant="secondary"
            className={cn(
                "inline-flex items-center gap-1.5 rounded-full px-2.5 py-1",
                "text-[11px] font-semibold uppercase tracking-wide select-none",
                muted
                    ? "bg-white/10 text-white/85 ring-1 ring-white/15"
                    : "bg-indigo-500/15 text-indigo-200 ring-1 ring-indigo-400/20",
                "pointer-events-none !transition-none"
            )}
        >
            <Users className="h-3.5 w-3.5" />
            {text}
        </Badge>
    );
}