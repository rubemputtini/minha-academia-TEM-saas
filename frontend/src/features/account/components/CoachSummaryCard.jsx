import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import CopyField from "@/shared/components/CopyField";
import { getInitials } from "@/shared/utils/getInitials";

export default function CoachSummaryCard({ name, email, coachCode }) {
    const initials = getInitials(name);

    return (
        <Card className="relative overflow-hidden bg-card border-border/40 shadow-md">
            <div className="absolute inset-0 bg-gradient-to-r from-primary/20 via-transparent to-transparent pointer-events-none" />

            <CardHeader className="flex flex-row items-center gap-4 relative z-10">
                <div className="h-12 w-12 rounded-full bg-primary/30 flex items-center justify-center text-lg font-semibold">
                    {initials}
                </div>

                <div className="space-y-1">
                    <CardTitle className="text-base">
                        {name || "Seu nome"}
                    </CardTitle>
                    <p className="text-xs text-muted-foreground">
                        {email || "seuemail@exemplo.com"}
                    </p>
                </div>
            </CardHeader>

            {coachCode && (
                <CardContent className="relative z-10 border-t border-border/60 pt-4 pb-5">
                    <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
                        <div className="space-y-1 max-w-md">
                            <p className="text-xs uppercase tracking-[0.18em] text-muted-foreground">
                                Código do treinador
                            </p>
                            <p className="text-xs text-muted-foreground">
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
