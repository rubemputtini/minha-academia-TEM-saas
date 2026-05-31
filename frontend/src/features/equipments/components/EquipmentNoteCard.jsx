import { useState } from "react";
import { MessageSquare, Pencil, Plus, Trash2, X } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { CARD_BASE } from "@/shared/styles/cards";
import { useEquipmentNote } from "../hooks/useEquipmentNote";
import { cn } from "@/lib/utils";

const MAX_CHARS = 300;

export default function EquipmentNoteCard() {
  const { note, setNote, savedNote, loading, saving, save, deleteNote } = useEquipmentNote();
  const [isEditing, setIsEditing] = useState(false);

  const hasSavedNote = !!savedNote;

  function startEditing() {
    setNote(savedNote);
    setIsEditing(true);
  }

  function cancelEditing() {
    setNote(savedNote);
    setIsEditing(false);
  }

  async function handleSave() {
    await save();
    setIsEditing(false);
  }

  async function handleDelete() {
    await deleteNote();
    setIsEditing(false);
  }

  const isDirty = note !== savedNote;

  return (
    <div className={`${CARD_BASE} relative overflow-hidden`}>
      <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-primary/40 to-transparent" />
      <MessageSquare className="pointer-events-none absolute right-5 top-5 h-20 w-20 -rotate-6 text-white opacity-[0.035]" />

      <div className="px-6 pb-6 pt-5">
        {/* Header */}
        <div className="flex items-start justify-between gap-3 mb-4">
          <div className="space-y-0.5">
            <p className="text-[10px] font-semibold uppercase tracking-[0.14em] text-muted-foreground/50">
              Observação
            </p>
            <h2 className="text-base font-semibold">Recado para o treinador</h2>
          </div>

          {/* Action button — only in view mode */}
          {!loading && !isEditing && (
            <button
              type="button"
              onClick={startEditing}
              className={cn(
                "flex items-center gap-1.5 rounded-xl border px-3 py-1.5 text-xs transition-colors shrink-0",
                hasSavedNote
                  ? "border-white/10 text-muted-foreground/60 hover:border-white/20 hover:text-foreground/80"
                  : "border-primary/30 text-primary/70 hover:border-primary/50 hover:text-primary"
              )}
            >
              {hasSavedNote
                ? <><Pencil className="h-3 w-3" />Editar</>
                : <><Plus className="h-3 w-3" />Adicionar</>
              }
            </button>
          )}
        </div>

        {/* Content */}
        {loading ? (
          <div className="space-y-2">
            <Skeleton className="h-4 w-3/4 bg-white/4" />
            <Skeleton className="h-4 w-1/2 bg-white/4" />
            <Skeleton className="h-4 w-2/3 bg-white/[0.03]" />
          </div>
        ) : isEditing ? (
          /* Edit mode */
          <div className="space-y-3">
            <p className="text-xs text-muted-foreground/60">
              Informe equipamentos extras, restrições ou qualquer observação para os seus treinos.
            </p>
            <div className="relative">
              <textarea
                value={note}
                onChange={(e) => setNote(e.target.value.slice(0, MAX_CHARS))}
                placeholder="Ex: Tenho halter de 5kg e 10kg em casa. Não consigo usar o leg press por conta do joelho..."
                rows={4}
                autoFocus
                className="w-full resize-none rounded-xl border border-white/15 bg-white/[0.03] px-4 py-3 text-sm text-foreground placeholder:text-muted-foreground/35 focus:border-primary/40 focus:outline-none focus:ring-0 transition-colors"
              />
              <span className="absolute bottom-2.5 right-3 text-[10px] text-muted-foreground/30 select-none">
                {note.length}/{MAX_CHARS}
              </span>
            </div>
            <div className="flex items-center justify-between gap-2">
              {/* Delete — only if a saved note exists */}
              {hasSavedNote ? (
                <button
                  type="button"
                  onClick={handleDelete}
                  disabled={saving}
                  className="flex items-center gap-1.5 text-xs text-red-400/50 hover:text-red-400/80 transition-colors px-2 py-1 disabled:pointer-events-none"
                >
                  <Trash2 className="h-3.5 w-3.5" />
                  Apagar nota
                </button>
              ) : <span />}

              <div className="flex items-center gap-2">
                <button
                  type="button"
                  onClick={cancelEditing}
                  disabled={saving}
                  className="flex items-center gap-1 text-xs text-muted-foreground/50 hover:text-muted-foreground/80 transition-colors px-2 py-1 disabled:pointer-events-none"
                >
                  <X className="h-3 w-3" />
                  Cancelar
                </button>
                <Button
                  size="sm"
                  disabled={!isDirty || saving}
                  loading={saving}
                  onClick={handleSave}
                  className="rounded-xl px-5 text-xs font-medium"
                >
                  Salvar
                </Button>
              </div>
            </div>
          </div>
        ) : hasSavedNote ? (
          /* View mode — note exists */
          <p className="text-sm text-foreground/75 leading-relaxed whitespace-pre-wrap">
            {savedNote}
          </p>
        ) : (
          /* View mode — no note */
          <p className="text-sm text-muted-foreground/35 italic">
            Nenhuma observação registrada. Use o botão acima para adicionar um recado ao seu treinador.
          </p>
        )}
      </div>
    </div>
  );
}
