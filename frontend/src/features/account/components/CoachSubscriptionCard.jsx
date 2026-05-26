import { useState } from "react";
import { useCurrency } from "@/shared/hooks/useCurrency";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { CalendarDaysIcon, CheckIcon, MinusIcon } from "@heroicons/react/24/outline";
import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { PLANS, FEATURES } from "@/marketing/data/plansDescription";

const UPGRADE_PLANS = PLANS.filter((p) => p.id === "basic" || p.id === "unlimited");

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

function UpgradePlanCard({ plan, currency, loading, onSelect }) {
  const isUnlimited = plan.id === "unlimited";
  const price = plan.prices?.[currency];

  return (
    <div
      className={cn(
        "relative rounded-xl p-px",
        isUnlimited
          ? "bg-gradient-to-b from-amber-400/70 to-amber-600/40"
          : "bg-white/10"
      )}
    >
      <div className="flex h-full flex-col rounded-xl bg-card p-5 gap-4">
        {isUnlimited && (
          <span className="absolute -top-3 left-1/2 -translate-x-1/2 whitespace-nowrap rounded-full bg-amber-400 px-3 py-0.5 text-[10px] font-semibold uppercase tracking-wider text-black">
            Melhor escolha
          </span>
        )}

        <div>
          <p className="text-sm font-semibold tracking-wide">{plan.name}</p>
          <div className="mt-1 flex items-end gap-1">
            <span className="text-3xl font-bold leading-none">{price}</span>
            <span className="pb-0.5 text-xs text-muted-foreground">{plan.cadence}</span>
          </div>
          <p className="mt-1 text-xs text-muted-foreground">{plan.clients}</p>
        </div>

        <ul className="flex-1 space-y-2">
          {FEATURES.map((f) => {
            const available = f.plans.includes(plan.id);
            return (
              <li key={f.key} className="flex items-start gap-2 text-xs">
                <span
                  className={cn(
                    "mt-0.5 flex h-4 w-4 flex-none items-center justify-center rounded-full",
                    available ? "bg-emerald-400/90 text-black" : "bg-white/8"
                  )}
                >
                  {available
                    ? <CheckIcon className="h-2.5 w-2.5" />
                    : <MinusIcon className="h-2.5 w-2.5 opacity-50" />}
                </span>
                <span className={available ? "text-foreground/90" : "text-muted-foreground/50 line-through"}>
                  {f.label}
                </span>
              </li>
            );
          })}
        </ul>

        <Button
          size="sm"
          loading={loading}
          disabled={loading}
          onClick={() => onSelect(plan.code)}
          className={cn(
            "w-full font-semibold",
            isUnlimited
              ? "bg-amber-400 text-black hover:bg-amber-300 active:bg-amber-500"
              : "bg-white/90 text-black hover:bg-white active:bg-white/70"
          )}
        >
          {plan.cta}
        </Button>
      </div>
    </div>
  );
}

export default function CoachSubscriptionCard({
  subscription,
  onManageSubscription,
  managingSubscription,
  upgradingPlan,
  onUpgrade,
}) {
  const [dialogOpen, setDialogOpen] = useState(false);
  const currency = useCurrency();
  const { status, plan, endAt } = subscription || {};

  const isActive = status?.toLowerCase() === "active";
  const isTrial = status?.toLowerCase() === "trial";

  async function handleSelect(planName) {
    await onUpgrade(planName);
    setDialogOpen(false);
  }

  return (
    <>
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
                  "mt-1 shrink-0 whitespace-nowrap rounded-full px-3.5 py-1 text-xs font-semibold tracking-wide",
                  isActive
                    ? "border-emerald-500/50 bg-emerald-500/15 text-emerald-300 hover:bg-emerald-500/15 hover:text-emerald-300"
                    : isTrial
                      ? "border-amber-500/50 bg-amber-500/10 text-amber-300 hover:bg-amber-500/10 hover:text-amber-300"
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

            {isTrial ? (
              <Button
                type="button"
                size="sm"
                variant="outline"
                onClick={() => setDialogOpen(true)}
                className="w-full border-primary/40 text-primary hover:bg-primary/10 sm:w-auto"
              >
                Fazer upgrade
              </Button>
            ) : (
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
            )}
          </div>
        </CardContent>
      </Card>

      <Dialog open={dialogOpen} onOpenChange={setDialogOpen}>
        <DialogContent className="sm:max-w-lg">
          <DialogHeader>
            <DialogTitle>Escolha seu plano</DialogTitle>
          </DialogHeader>
          <div className="grid grid-cols-1 gap-4 pt-2 sm:grid-cols-2">
            {UPGRADE_PLANS.map((p) => (
              <UpgradePlanCard
                key={p.id}
                plan={p}
                currency={currency}
                loading={upgradingPlan === p.code}
                onSelect={handleSelect}
              />
            ))}
          </div>
        </DialogContent>
      </Dialog>
    </>
  );
}
