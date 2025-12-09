import { Card, CardHeader, CardTitle } from "@/components/ui/card";
import { getInitials } from "@/shared/utils/getInitials";

export default function AccountSummaryCard({ name, email }) {
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
        </Card>
    );
}
