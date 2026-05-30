import { UserRound } from "lucide-react";
import { Card, CardHeader, CardTitle } from "@/components/ui/card";
import { cn } from "@/lib/utils";
import { CARD_BASE } from "@/shared/styles/cards";
import { getInitials } from "@/shared/utils/getInitials";

export default function AccountSummaryCard({ name, email }) {
    const initials = getInitials(name);

    return (
        <Card className={cn(CARD_BASE, "relative overflow-hidden")}>
            <div className="pointer-events-none absolute -right-5 -top-6 opacity-[0.035]">
                <UserRound className="h-36 w-36" />
            </div>
            <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-primary/40 to-transparent" />

            <CardHeader className="flex flex-row items-center gap-4 relative z-10">
                <div className="h-14 w-14 shrink-0 rounded-full border border-primary/35 bg-primary/15 shadow-[0_0_40px_rgba(253,186,27,0.25)] flex items-center justify-center text-lg font-bold text-primary select-none">
                    {initials}
                </div>

                <div className="space-y-1 min-w-0">
                    <CardTitle className="text-base">
                        {name || "Seu nome"}
                    </CardTitle>
                    <p className="text-xs tracking-[0.04em] text-muted-foreground/75 truncate">
                        {email || "seuemail@exemplo.com"}
                    </p>
                </div>
            </CardHeader>
        </Card>
    );
}
