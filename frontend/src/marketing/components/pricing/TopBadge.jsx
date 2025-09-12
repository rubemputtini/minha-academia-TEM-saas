import { Crown } from "lucide-react";
import { Badge } from "@/components/ui/badge";
import { cn } from "@/lib/utils";

export default function TopBadge({ children }) {
    return (
        <Badge
            className={cn(
                "absolute -top-3 left-1/2 -translate-x-1/2",
                "bg-amber-400 text-black px-3 py-1 text-[11px] font-bold uppercase tracking-wide shadow-sm",
                "pointer-events-none !transition-none"
            )}
        >
            <Crown className="mr-1 h-3.5 w-3.5" />
            {children}
        </Badge>
    );
}