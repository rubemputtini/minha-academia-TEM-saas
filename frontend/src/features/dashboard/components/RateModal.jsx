import { useState } from "react";
import { Button } from "@/components/ui/button";
import { cn } from "@/lib/utils";
import { CURRENCIES, CURRENCY_SYMBOLS, CURRENCY_FLAGS } from "../utils/currencies";

export function RateModal({ revenue, saving, onSave, onClose }) {
  const [rate, setRate] = useState(revenue?.monthlyRate ?? "");
  const [currency, setCurrency] = useState(revenue?.currency ?? "BRL");

  async function handleSubmit(e) {
    e.preventDefault();
    const parsed = parseFloat(rate);

    if (isNaN(parsed) || parsed <= 0) return;

    await onSave(parsed, currency);
    onClose();
  }

  return (
    <div
      className="fixed inset-0 z-50 flex items-center justify-center bg-black/70 backdrop-blur-sm"
      onClick={(e) => e.target === e.currentTarget && onClose()}
    >
      <div className="w-full max-w-sm rounded-2xl border border-white/12 bg-[rgba(16,18,28,0.98)] p-6 shadow-[0_24px_80px_rgba(0,0,0,0.9)] backdrop-blur-2xl">
        <div className="mb-5">
          <h2 className="text-base font-semibold tracking-tight text-foreground">
            Valor cobrado por aluno
          </h2>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="space-y-1.5">
            <label className="text-[10px] font-semibold uppercase tracking-[0.22em] text-muted-foreground">
              Moeda
            </label>
            <div className="grid grid-cols-3 gap-2">
              {CURRENCIES.map((c) => (
                <button
                  key={c}
                  type="button"
                  onClick={() => setCurrency(c)}
                  className={cn(
                    "flex flex-col items-center gap-1 rounded-xl border py-2.5 transition-all",
                    currency === c
                      ? "border-primary/60 bg-primary/10 text-primary"
                      : "border-white/10 bg-white/[0.03] text-muted-foreground hover:border-white/20 hover:bg-white/[0.06]"
                  )}
                >
                  <span className="text-lg leading-none">{CURRENCY_FLAGS[c]}</span>
                  <span className="font-mono text-[10px] font-semibold tracking-[0.18em]">{c}</span>
                </button>
              ))}
            </div>
          </div>

          <div className="space-y-1.5">
            <label className="text-[10px] font-semibold uppercase tracking-[0.22em] text-muted-foreground">
              Valor mensal por aluno
            </label>
            <div className="flex items-center gap-2 rounded-xl border border-white/12 bg-white/[0.03] px-4 py-3 focus-within:border-primary/40">
              <span className="font-mono text-sm text-muted-foreground">
                {CURRENCY_SYMBOLS[currency]}
              </span>
              <input
                type="number"
                min="0.01"
                step="0.01"
                value={rate}
                onChange={(e) => setRate(e.target.value)}
                placeholder="0.00"
                className="flex-1 bg-transparent font-mono text-sm text-foreground placeholder:text-muted-foreground/40 focus:outline-none [appearance:textfield] [&::-webkit-outer-spin-button]:appearance-none [&::-webkit-inner-spin-button]:appearance-none"
              />
            </div>
          </div>

          <div className="flex gap-3 pt-1">
            <Button
              type="button"
              variant="ghost"
              onClick={onClose}
              className="flex-1 rounded-xl border border-white/10 text-[11px] uppercase tracking-[0.18em] text-muted-foreground hover:bg-white/5"
            >
              Cancelar
            </Button>
            <Button
              type="submit"
              loading={saving}
              className="flex-1 rounded-xl text-[11px] uppercase tracking-[0.18em]"
            >
              Salvar
            </Button>
          </div>
        </form>
      </div>
    </div>
  );
}
