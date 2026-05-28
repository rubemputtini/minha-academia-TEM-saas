import { Link } from "react-router-dom";
import { Users, CreditCard } from "lucide-react";
import { Card, CardContent } from "@/components/ui/card";
import { cn } from "@/lib/utils";
import { CARD_BASE } from "@/shared/styles/cards";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { ROUTES } from "@/shared/routes/routes";

export function PlanCard({ planName, usersLimit, currentUsers, loading }) {
  const isUnlimited = usersLimit == null;
  const pct = !isUnlimited && usersLimit > 0
    ? Math.round((currentUsers / usersLimit) * 100)
    : 0;
  const atLimit = !isUnlimited && pct >= 100;
  const nearLimit = !isUnlimited && !atLimit && pct >= 80;

  return (
    <Card className={cn(CARD_BASE, "relative overflow-hidden")}>
      <div className="pointer-events-none absolute -right-3 -top-5 opacity-[0.04]">
        <Users className="h-28 w-28" />
      </div>
      <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-primary/40 to-transparent" />

      <CardContent className="relative z-10 space-y-4 px-6 py-5">
        <div className="flex items-start justify-between">
          <p className="text-xs font-semibold uppercase tracking-[0.12em] text-muted-foreground/90">
            Seu plano atual
          </p>
          {planName && (
            <Badge className="rounded-full bg-primary/90 px-4 py-1 text-xs font-semibold tracking-[0.1em] text-black">
              {planName.toUpperCase()}
            </Badge>
          )}
        </div>

        {loading ? (
          <div className="space-y-2">
            <Skeleton className="h-12 w-20 bg-white/6" />
            <Skeleton className="h-1.5 w-full bg-white/4" />
          </div>
        ) : (
          <>
            <div className="flex items-end gap-2 font-mono">
              <span className="text-5xl font-bold leading-none text-foreground">
                {currentUsers}
              </span>
              <span className="pb-1 text-lg text-muted-foreground/80">
                / {isUnlimited ? "∞" : usersLimit}
              </span>
              <span className="ml-1 pb-1 text-xs tracking-[0.08em] text-muted-foreground/75">
                {currentUsers === 1 ? "aluno" : "alunos"}
              </span>
            </div>

            {!isUnlimited && (
              <div className="space-y-1.5">
                <div className="h-1.5 w-full rounded-full bg-white/5">
                  <div
                    className={cn(
                      "h-1.5 rounded-full transition-all",
                      atLimit ? "bg-red-400" : nearLimit ? "bg-amber-400" : "bg-primary"
                    )}
                    style={{ width: `${Math.min(pct, 100)}%` }}
                  />
                </div>
                <p
                  className={cn(
                    "text-xs tracking-[0.06em]",
                    atLimit ? "text-red-400/90" : nearLimit ? "text-amber-400/90" : "text-muted-foreground/75"
                  )}
                >
                  {atLimit
                    ? "Limite atingido"
                    : nearLimit
                      ? "Quase no limite"
                      : usersLimit != null
                        ? `${usersLimit - currentUsers} vagas disponíveis`
                        : ""}
                </p>


              </div>
            )}
          </>
        )}

        {!atLimit && (
          <Link to={ROUTES.coachSubscription}>
            <Button
              size="sm"
              variant={nearLimit ? "default" : "ghost"}
              className={cn(
                "h-7 w-full rounded-lg text-xs tracking-[0.06em]",
                nearLimit
                  ? "font-semibold"
                  : "border border-white/10 text-muted-foreground hover:bg-white/5 hover:text-foreground"
              )}
            >
              <CreditCard className="mr-1.5 h-3 w-3" />
              {nearLimit ? "Ver planos" : "Gerenciar assinatura"}
            </Button>
          </Link>
        )}
      </CardContent>
    </Card>
  );
}
