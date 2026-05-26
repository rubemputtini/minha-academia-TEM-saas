import { useState } from "react";
import { Button } from "@/components/ui/button";
import { toISODateString } from "@/shared/utils/date";

export function TrainingDateModal({ item, saving, onSave, onClose }) {
  const initial = item.nextTrainingChangeAt
    ? toISODateString(item.nextTrainingChangeAt)
    : "";
  const [date, setDate] = useState(initial);

  async function handleSubmit(e) {
    e.preventDefault();

    if (!date) return;
    
    await onSave(date ? new Date(date + "T12:00:00").toISOString() : null);
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
            Próxima troca de treino
          </h2>
          <p className="mt-0.5 text-[11px] text-muted-foreground/70">{item.name}</p>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="space-y-1.5">
            <label className="text-[10px] font-semibold uppercase tracking-[0.22em] text-muted-foreground">
              Data prevista
            </label>
            <input
              type="date"
              value={date}
              onChange={(e) => setDate(e.target.value)}
              className="w-full rounded-xl border border-white/12 bg-white/[0.03] px-4 py-3 font-mono text-sm text-foreground focus:border-primary/40 focus:outline-none [color-scheme:dark]"
            />
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
              disabled={!date}
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
