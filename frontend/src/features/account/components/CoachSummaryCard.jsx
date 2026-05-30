import { UserRound } from "lucide-react";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { cn } from "@/lib/utils";
import { CARD_BASE } from "@/shared/styles/cards";
import CopyField from "@/shared/components/CopyField";
import { getInitials } from "@/shared/utils/getInitials";

export default function CoachSummaryCard({ name, email, coachCode }) {
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

            {coachCode && (
                <CardContent className="relative z-10 border-t border-white/6 pt-4 pb-5">
                    <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
                        <div className="space-y-1 max-w-md">
                            <p className="text-[10px] font-semibold uppercase tracking-[0.14em] text-muted-foreground/50">
                                Código do treinador
                            </p>
                            <p className="text-xs text-muted-foreground/70">
                                Envie este código aos seus alunos para que o cadastro deles seja automaticamente vinculado a você.
                            </p>
                        </div>

                        <CopyField
                            label="Seu código"
                            value={coachCode.toUpperCase()}
                        />
                    </div>
                </CardContent>
            )}
        </Card>
    );
}
