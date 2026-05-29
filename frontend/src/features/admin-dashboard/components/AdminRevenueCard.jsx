import { ChevronRight, Loader2, TrendingUp } from "lucide-react";
import { Card, CardContent } from "@/components/ui/card";
import { cn } from "@/lib/utils";
import { CARD_BASE } from "@/shared/styles/cards";
import { Skeleton } from "@/components/ui/skeleton";
import { useExchangeRates } from "@/features/dashboard/hooks/useExchangeRates";
import { fmtCurrency } from "@/features/dashboard/utils/currencies";

export function AdminRevenueCard({ loading, stats }) {
  const basicCoaches = stats?.basicCoaches ?? 0;
  const unlimitedCoaches = stats?.unlimitedCoaches ?? 0;
  const estimatedRevenueBrl = stats?.estimatedMonthlyRevenueBrl ?? 0;
  const estimatedBasicRevenueBrl = stats?.estimatedBasicRevenueBrl ?? 0;
  const estimatedUnlimitedRevenueBrl = stats?.estimatedUnlimitedRevenueBrl ?? 0;

  const { displayCurrency, rate, symbol, flag, loading: ratesLoading, toggleCurrency } =
    useExchangeRates("BRL");

  const converting = displayCurrency !== "BRL";
  const converted = rate != null ? estimatedRevenueBrl * rate : null;
  const basicConverted = rate != null ? estimatedBasicRevenueBrl * rate : null;
  const unlimitedConverted = rate != null ? estimatedUnlimitedRevenueBrl * rate : null;

  return (
    <Card className={cn(CARD_BASE, "relative flex flex-1 flex-col overflow-hidden")}>
      <div className="pointer-events-none absolute -right-3 -top-5 opacity-[0.04]">
        <TrendingUp className="h-28 w-28" />
      </div>
      <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-primary/40 to-transparent" />

      <CardContent className="relative z-10 space-y-4 px-6 py-5">
        <div className="flex items-center justify-between">
          <p className="text-xs font-semibold uppercase tracking-[0.12em] text-muted-foreground/90">
            Receita Estimada
          </p>
          <button
            onClick={toggleCurrency}
            title="Clique para converter a moeda"
            className="flex items-center gap-1.5 rounded-full bg-white/5 px-3 py-1 font-mono text-xs tracking-[0.08em] text-muted-foreground/90 transition-colors hover:bg-white/10 hover:text-foreground"
          >
            <span>{flag}</span>
            {displayCurrency}
            <ChevronRight className="h-3 w-3 opacity-60" />
          </button>
        </div>

        {loading ? (
          <div className="space-y-2">
            <Skeleton className="h-12 w-32 bg-white/6" />
            <Skeleton className="h-3 w-16 bg-white/4" />
            <Skeleton className="mt-3 h-px w-full bg-white/4" />
            <Skeleton className="h-3 w-full bg-white/4" />
            <Skeleton className="h-3 w-full bg-white/4" />
          </div>
        ) : (
          <>
            <div className="flex items-end gap-2 font-mono">
              {ratesLoading ? (
                <Loader2 className="h-8 w-8 animate-spin text-muted-foreground/50" />
              ) : converted != null ? (
                <>
                  <span className="text-4xl font-bold leading-none tracking-tight text-foreground">
                    {symbol} {fmtCurrency(converted)}
                  </span>
                  <span className="pb-1 text-xs tracking-[0.08em] text-muted-foreground/75">
                    / mês
                  </span>
                </>
              ) : (
                <span className="text-sm text-muted-foreground/60">Cotação indisponível</span>
              )}
            </div>

            <div className="space-y-2.5 border-t border-white/8 pt-3">
              <div className="flex items-center justify-between text-xs">
                <span className="flex items-center gap-2 text-muted-foreground/75">
                  <span className="h-1.5 w-1.5 shrink-0 rounded-full bg-sky-400/70" />
                  Basic
                  <span className="font-mono text-muted-foreground/40">× {basicCoaches}</span>
                </span>
                <span className="font-mono font-semibold text-muted-foreground/65">
                  {basicConverted != null ? `${symbol} ${fmtCurrency(basicConverted)}` : "—"}
                </span>
              </div>
              <div className="flex items-center justify-between text-xs">
                <span className="flex items-center gap-2 text-muted-foreground/75">
                  <span className={cn("h-1.5 w-1.5 shrink-0 rounded-full", unlimitedCoaches > 0 ? "bg-primary/70" : "bg-white/20")} />
                  Unlimited
                  <span className="font-mono text-muted-foreground/40">× {unlimitedCoaches}</span>
                </span>
                <span className={cn("font-mono font-semibold", unlimitedCoaches > 0 ? "text-primary" : "text-muted-foreground/65")}>
                  {unlimitedConverted != null ? `${symbol} ${fmtCurrency(unlimitedConverted)}` : "—"}
                </span>
              </div>
            </div>

            {converting && rate != null && (
              <p className="font-mono text-xs tracking-[0.06em] text-muted-foreground/50">
                1 BRL = {fmtCurrency(rate)} {displayCurrency}
              </p>
            )}

            <p className="text-[10px] leading-relaxed text-muted-foreground/35">
              Estimativa · sem descontos de indicação
            </p>
          </>
        )}
      </CardContent>
    </Card>
  );
}
