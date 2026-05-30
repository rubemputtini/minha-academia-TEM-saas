import { useState } from "react";
import { CreditCard } from "lucide-react";
import { CalendarDaysIcon, CheckIcon, MinusIcon } from "@heroicons/react/24/outline";
import { Card, CardContent } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { cn } from "@/lib/utils";
import { CARD_BASE } from "@/shared/styles/cards";
import { useCurrency } from "@/shared/hooks/useCurrency";
import { PLANS, FEATURES } from "@/marketing/data/plansDescription";

const UPGRADE_PLANS = PLANS.filter((p) => p.id === "basic" || p.id === "unlimited");

function formatEndDate(endAt) {
  if (!endAt) return "—";
  return new Date(endAt).toLocaleDateString(undefined, {
    day: "2-digit",
    month: "2-digit",
    year: "numeric",
    timeZone: "UTC",
  });
}

function StatusBadge({ status }) {
  const lower = status?.toLowerCase();
  const isActive = lower === "active";
  const isTrial = lower === "trial";

  return (
    <Badge
      variant="outline"
      className={cn(
        "shrink-0 whitespace-nowrap rounded-full px-3.5 py-1 text-xs font-semibold tracking-wide",
        isActive
          ? "border-emerald-500/50 bg-emerald-500/15 text-emerald-300 hover:bg-emerald-500/15"
          : isTrial
            ? "border-amber-500/50 bg-amber-500/10 text-amber-300 hover:bg-amber-500/10"
            : "border-white/15 bg-white/5 text-muted-foreground"
      )}
    >
      {status}
    </Badge>
  );
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
      <div className="flex h-full flex-col rounded-xl bg-[rgba(10,12,20,0.98)] p-5 gap-4">
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
  const isTrial = status?.toLowerCase() === "trial";

  async function handleSelect(planName) {
    await onUpgrade(planName);
    setDialogOpen(false);
  }

  return (
    <>
      <Card className={cn(CARD_BASE, "relative overflow-hidden")}>
        <div className="pointer-events-none absolute -right-5 -top-6 opacity-[0.035]">
          <CreditCard className="h-36 w-36" />
        </div>
        <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-primary/40 to-transparent" />

        <CardContent className="relative z-10 px-6 py-5 space-y-6">
          <div className="flex items-start justify-between gap-4">
            <div>
              <p className="text-[10px] font-semibold uppercase tracking-[0.14em] text-muted-foreground/50">
                Plano atual
              </p>
              <p className="mt-1.5 text-2xl font-bold tracking-tight uppercase">
                {plan || "—"}
              </p>
            </div>
            {status && <StatusBadge status={status} />}
          </div>

          <div className="border-t border-white/6 pt-5">
            <p className="flex items-center gap-1.5 text-[10px] font-semibold uppercase tracking-[0.14em] text-muted-foreground/50">
              <CalendarDaysIcon className="h-3.5 w-3.5" />
              Término
            </p>
            <p className="mt-1.5 text-base font-semibold tabular-nums">
              {formatEndDate(endAt)}
            </p>
          </div>

          <div className="border-t border-white/6 pt-5">
            {isTrial ? (
              <Button
                type="button"
                size="sm"
                onClick={() => setDialogOpen(true)}
                className="h-8 rounded-lg border border-primary/40 bg-primary/5 px-4 text-xs font-semibold tracking-[0.06em] text-primary hover:bg-primary/10"
              >
                Fazer upgrade
              </Button>
            ) : (
              <Button
                type="button"
                size="sm"
                variant="ghost"
                loading={managingSubscription}
                disabled={managingSubscription}
                onClick={onManageSubscription}
                className="h-8 rounded-lg border border-white/10 px-4 text-xs tracking-[0.06em] text-muted-foreground hover:bg-white/5 hover:text-foreground"
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
