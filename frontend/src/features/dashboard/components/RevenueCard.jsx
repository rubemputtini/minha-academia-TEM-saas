import { TrendingUp, ChevronRight, Pencil, Loader2 } from "lucide-react";
import { Card, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { useExchangeRates } from "../hooks/useExchangeRates";
import { fmtCurrency } from "../utils/currencies";

export function RevenueCard({ currentUsers, revenue, loading, onEdit }) {
  const baseCurrency = revenue?.currency || "BRL";
  const monthlyRate = revenue?.monthlyRate ?? null;

  const {
    displayCurrency,
    rate,
    symbol,
    flag,
    loading: ratesLoading,
    toggleCurrency,
  } = useExchangeRates(baseCurrency);

  const converting = displayCurrency !== baseCurrency;
  const ratePerUser = monthlyRate != null && rate != null ? monthlyRate * rate : null;
  const total = ratePerUser != null ? currentUsers * ratePerUser : null;

  return (
    <Card className="rounded-2xl border border-white/10 bg-[rgba(12,14,22,0.96)] shadow-[0_16px_55px_rgba(0,0,0,0.7)] backdrop-blur-2xl">
      <CardContent className="space-y-4 px-6 py-5">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-2">
            <div className="flex h-7 w-7 shrink-0 items-center justify-center rounded-full bg-primary/10 text-primary">
              <TrendingUp className="h-4 w-4" />
            </div>
            <p className="text-xs font-semibold uppercase tracking-[0.12em] text-muted-foreground/90">
              Receita estimada
            </p>
          </div>

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
            <Skeleton className="h-3 w-40 bg-white/4" />
            <Skeleton className="h-8 w-32 bg-white/6" />
            <Skeleton className="h-3 w-16 bg-white/4" />
          </div>
        ) : monthlyRate == null ? (
          <p className="text-[12px] text-muted-foreground/70">
            Configure o valor do seu plano para ver o faturamento estimado.
          </p>
        ) : (
          <div className="space-y-1.5">
            {ratePerUser != null ? (
              <p className="text-sm text-muted-foreground/80">
                {currentUsers} {currentUsers === 1 ? "aluno" : "alunos"} × {symbol} {fmtCurrency(ratePerUser)}/mês
              </p>
            ) : ratesLoading ? (
              <Loader2 className="h-3.5 w-3.5 animate-spin text-muted-foreground/50" />
            ) : (
              <p className="text-xs text-muted-foreground/50">Cotação indisponível</p>
            )}

            <div className="border-t border-white/8 pt-2">
              {total != null ? (
                <>
                  <p className="font-mono text-3xl font-bold tracking-tight text-foreground">
                    {symbol} {fmtCurrency(total)}
                  </p>
                  <p className="mt-0.5 text-xs tracking-[0.1em] text-muted-foreground/60">
                    por mês
                  </p>
                </>
              ) : (
                <p className="text-sm text-muted-foreground/60">—</p>
              )}
            </div>

            {converting && rate != null && (
              <p className="text-xs font-mono tracking-[0.06em] text-muted-foreground/50">
                1 {baseCurrency} = {fmtCurrency(rate)} {displayCurrency}
              </p>
            )}
          </div>
        )}

        <Button
          variant="ghost"
          size="sm"
          onClick={onEdit}
          className="h-7 w-full rounded-lg border border-white/10 text-xs tracking-[0.06em] text-muted-foreground hover:bg-white/5 hover:text-foreground"
        >
          <Pencil className="mr-1.5 h-3 w-3" />
          Ajustar valor por aluno
        </Button>
      </CardContent>
    </Card>
  );
}
