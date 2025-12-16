import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { CalendarDaysIcon } from "@heroicons/react/24/outline";
import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";

function formatEndDate(endAt) {
  if (!endAt) return "Sem data de término";

  const date = new Date(endAt);

  return date.toLocaleDateString(undefined, {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
    timeZone: "UTC",
  });
}

export default function CoachSubscriptionCard({
  subscription,
  onManageSubscription,
  managingSubscription,
}) {
  const { status, plan, endAt } = subscription || {};

  const isActive = status?.toLowerCase() === "active";

  return (
    <Card className="bg-card border-border/60">
      <CardHeader className="pb-3">
        <div className="flex items-start justify-between gap-4">
          <div>
            <CardTitle className="text-lg font-semibold tracking-wide">
              Assinatura
            </CardTitle>
            <p className="mt-1 text-xs text-muted-foreground">
              Detalhes do seu plano como treinador.
            </p>
          </div>

          {status && (
            <Badge
              variant="outline"
              className={cn(
                "mt-1 rounded-full px-3 py-0.5 text-xs font-medium",
                isActive
                  ? "border-emerald-500/50 bg-emerald-500/15 text-emerald-300 hover:bg-emerald-500/15 hover:text-emerald-300"
                  : "border-border/60 bg-transparent text-muted-foreground hover:bg-transparent hover:text-muted-foreground"
              )}
            >
              {status}
            </Badge>
          )}
        </div>
      </CardHeader>

      <CardContent>
        <div className="max-w-md space-y-6">
          <div>
            <span className="text-[11px] uppercase tracking-[0.12em] text-muted-foreground">
              Plano atual
            </span>
            <p className="mt-1 text-base font-semibold uppercase text-foreground">
              {plan || "-"}
            </p>
          </div>

          <div>
            <span className="flex items-center gap-1.5 text-[11px] uppercase tracking-[0.12em] text-muted-foreground">
              <CalendarDaysIcon className="h-4 w-4" />
              Término
            </span>
            <p className="mt-1 text-base font-semibold text-foreground/90">
              {formatEndDate(endAt)}
            </p>
          </div>

          <Button
            type="button"
            size="sm"
            variant="outline"
            loading={managingSubscription}
            disabled={managingSubscription}
            onClick={onManageSubscription}
            className="w-full border-primary/40 text-primary hover:bg-primary/10 sm:w-auto"
          >
            Gerenciar assinatura
          </Button>
        </div>
      </CardContent>
    </Card>
  );
}
