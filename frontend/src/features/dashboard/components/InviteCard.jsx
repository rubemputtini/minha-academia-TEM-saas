import { useState } from "react";
import { UserPlus, Copy, Check, Share2 } from "lucide-react";
import { Card, CardContent } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { cn } from "@/lib/utils";
import { CARD_BASE } from "@/shared/styles/cards";

export function InviteCard({ coachCode, loading }) {
  const [copied, setCopied] = useState(false);

  function handleCopy() {
    if (!coachCode) return;

    navigator.clipboard.writeText(coachCode);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  }

  const canShare = typeof navigator !== "undefined" && !!navigator.share;

  function handleShare() {
    if (!coachCode) return;

    const url = `${window.location.origin}/cadastro/aluno`;
    navigator.share({
      text: `Use meu código para se cadastrar no Minha Academia TEM. Cadastre os equipamentos da sua academia e seu treino será montado pra sua realidade.\n\nCódigo: ${coachCode}\n${url}`,
    });
  }

  return (
    <Card className={cn(CARD_BASE, "relative overflow-hidden")}>
      <div className="pointer-events-none absolute -right-3 -top-5 opacity-[0.035]">
        <UserPlus className="h-28 w-28" />
      </div>
      <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-primary/40 to-transparent" />

      <CardContent className="relative z-10 space-y-3 px-6 py-5">
        <p className="text-xs font-semibold uppercase tracking-[0.12em] text-muted-foreground/90">
          Convide um aluno
        </p>

        <div className="flex items-center justify-between rounded-xl border border-primary/15 bg-primary/5 px-4 py-2.5">
          {loading || !coachCode ? (
            <Skeleton className="h-5 w-24 bg-white/6" />
          ) : (
            <span className="font-mono text-sm font-semibold tracking-[0.14em] text-primary">
              {coachCode}
            </span>
          )}

          <div className="flex items-center gap-1.5">
            {canShare && (
              <button
                onClick={handleShare}
                disabled={!coachCode || loading}
                title="Compartilhar"
                className="flex h-7 w-7 items-center justify-center rounded-lg border border-white/10 text-muted-foreground transition-colors hover:bg-white/8 hover:text-foreground disabled:opacity-40"
              >
                <Share2 className="h-3.5 w-3.5" />
              </button>
            )}
            <button
              onClick={handleCopy}
              disabled={!coachCode || loading}
              title={copied ? "Copiado!" : "Copiar código"}
              className="flex h-7 w-7 items-center justify-center rounded-lg border border-white/10 text-muted-foreground transition-colors hover:bg-white/8 hover:text-foreground disabled:opacity-40"
            >
              {copied ? (
                <Check className="h-3.5 w-3.5 text-primary" />
              ) : (
                <Copy className="h-3.5 w-3.5" />
              )}
            </button>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
