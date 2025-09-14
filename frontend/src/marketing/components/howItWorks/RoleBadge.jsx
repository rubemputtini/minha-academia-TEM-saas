import { Badge } from "@/components/ui/badge";

export default function RoleBadge({ role, Icon, compact = false }) {
    return (
        <div className="flex items-center gap-2">
            <Badge
                variant="outline"
                className={[
                    "rounded-full border-amber-300/40 bg-amber-300/10 text-amber-100/90",
                    "uppercase tracking-[0.08em]",
                    compact ? "h-6 px-2 text-[10px]" : "h-7 px-2.5 text-[11px]",
                ].join(" ")}
            >
                {role}
            </Badge>
            <Icon className={compact ? "h-4 w-4 text-amber-300/85" : "h-[18px] w-[18px] text-amber-300/85"} aria-hidden />
        </div>
    );
}