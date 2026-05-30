import { useState } from "react";
import { Link } from "react-router-dom";
import { UserPlus, Lock, Copy, Check } from "lucide-react";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Skeleton } from "@/components/ui/skeleton";
import { ROUTES } from "@/shared/routes/routes";
import { cn } from "@/lib/utils";
import { CARD_BASE } from "@/shared/styles/cards";
import { CalendarDays } from "lucide-react";
import { TrainingRow } from "./TrainingRow";

function TrainingRowSkeleton() {
  return (
    <div className="flex items-center justify-between border-b border-white/5 px-3 py-4 sm:px-6">
      <div className="flex items-center gap-3">
        <Skeleton className="h-10 w-10 shrink-0 rounded-full bg-white/6" />
        <div className="space-y-2">
          <Skeleton className="h-3.5 w-28 bg-white/6" />
          <Skeleton className="h-3 w-20 bg-white/4" />
        </div>
      </div>
      <div className="flex items-center gap-2 sm:gap-4">
        <Skeleton className="hidden h-8 w-16 rounded-lg bg-white/4 sm:block" />
        <Skeleton className="h-8 w-8 rounded-lg bg-white/6 sm:w-24" />
      </div>
    </div>
  );
}

const GHOST_ROWS = [
  { avatar: 40, name: 72, gym: 48, badge: 56 },
  { avatar: 40, name: 56, gym: 36, badge: 40 },
];

function GhostRows() {
  return (
    <div className="relative overflow-hidden">
      <div className="pointer-events-none select-none blur-[2px]">
        {GHOST_ROWS.map((w, i) => (
          <div
            key={i}
            className="flex items-center justify-between border-b border-white/5 px-3 py-4 sm:px-6"
            style={{ opacity: 0.35 - i * 0.12 }}
          >
            <div className="flex items-center gap-3">
              <div className="h-10 w-10 shrink-0 rounded-full bg-white/10" />
              <div className="space-y-2">
                <div className="h-3.5 rounded bg-white/15" style={{ width: w.name * 1.5 }} />
                <div className="h-3 rounded bg-white/8" style={{ width: w.gym * 1.5 }} />
              </div>
            </div>
            <div className="flex items-center gap-4">
              <div className="hidden h-4 rounded bg-white/10 sm:block" style={{ width: w.badge * 1.5 }} />
              <div className="h-8 w-8 rounded-lg bg-white/8 sm:w-24" />
            </div>
          </div>
        ))}
      </div>
      <div className="absolute inset-0 bg-gradient-to-b from-transparent via-[rgba(10,12,20,0.5)] to-[rgba(10,12,20,0.95)]" />
      <div className="absolute inset-x-0 bottom-0 flex flex-col items-center gap-3 pb-5">
        <div className="flex items-center gap-2 text-muted-foreground/75">
          <Lock className="h-3.5 w-3.5" />
          <span className="text-xs tracking-[0.06em]">
            Faça upgrade para adicionar mais alunos
          </span>
        </div>
        <Link to={ROUTES.coachSubscription}>
          <Button
            size="sm"
            className="h-7 rounded-lg px-4 text-xs font-semibold tracking-[0.06em]"
          >
            Ver planos
          </Button>
        </Link>
      </div>
    </div>
  );
}

export function TrainingScheduleCard({ loading, schedule, totalActiveClients, overdueCount, upcomingCount, onEditItem, atLimit, coachCode }) {
  const [copied, setCopied] = useState(false);

  function handleCopyInviteLink() {
    if (!coachCode) return;
    
    const link = `${window.location.origin}/cadastro/aluno?coach=${coachCode}`;
    navigator.clipboard.writeText(link);
    setCopied(true);
    setTimeout(() => setCopied(false), 2000);
  }
  return (
    <Card className={cn(CARD_BASE, "relative overflow-hidden")}>
      <div className="pointer-events-none absolute -right-3 -top-5 opacity-[0.04]">
        <CalendarDays className="h-28 w-28" />
      </div>
      <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-primary/40 to-transparent" />
      <CardHeader className="relative z-10 flex flex-row items-center justify-between gap-3 border-b border-white/10 py-5">
        <div className="min-w-0 space-y-1.5">
          <CardTitle className="text-xl font-semibold tracking-tight text-foreground sm:text-2xl">
            Trocas de treino
          </CardTitle>
          {loading ? (
            <Skeleton className="hidden h-4 w-44 bg-white/4 sm:block" />
          ) : (
            <p className="hidden items-center gap-2 text-sm sm:flex">
              <span className={overdueCount > 0 ? "font-medium text-red-400" : "text-muted-foreground/60"}>
                {overdueCount} atrasada{overdueCount !== 1 ? "s" : ""}
              </span>
              <span className="text-muted-foreground/25">·</span>
              <span className={upcomingCount > 0 ? "font-medium text-amber-400" : "text-muted-foreground/60"}>
                {upcomingCount} esta semana
              </span>
            </p>
          )}
        </div>

        <Badge className="pointer-events-none shrink-0 rounded-full bg-white/5 px-4 py-1 font-mono text-xs tracking-[0.1em] text-muted-foreground/90">
          {loading ? "…" : `${totalActiveClients ?? schedule.length} ATIVO${(totalActiveClients ?? schedule.length) === 1 ? "" : "S"}`}
        </Badge>
      </CardHeader>

      <CardContent className="relative z-10 p-0">
        {loading ? (
          Array.from({ length: 3 }).map((_, i) => <TrainingRowSkeleton key={i} />)
        ) : schedule.length === 0 ? (
          <div className="flex flex-col items-center gap-4 px-6 py-12 text-center">
            <div className="flex h-12 w-12 shrink-0 items-center justify-center rounded-full border border-white/10 bg-white/[0.03] text-muted-foreground/60">
              {(totalActiveClients ?? 0) === 0 ? (
                <UserPlus className="h-5 w-5" />
              ) : (
                <CalendarDays className="h-5 w-5" />
              )}
            </div>
            <div className="space-y-1">
              {(totalActiveClients ?? 0) === 0 ? (
                <>
                  <p className="text-sm font-medium text-muted-foreground/80">
                    Nenhum aluno cadastrado
                  </p>
                  <p className="text-xs text-muted-foreground/60">
                    Compartilhe o link de convite para seus alunos se cadastrarem.
                  </p>
                </>
              ) : (
                <>
                  <p className="text-sm font-medium text-muted-foreground/80">
                    Nenhuma troca prevista nos próximos 14 dias
                  </p>
                  <p className="text-xs text-muted-foreground/60">
                    Todos os alunos estão em dia com o treino.
                  </p>
                </>
              )}
            </div>
            {(totalActiveClients ?? 0) === 0 && (
              <Button
                variant="ghost"
                size="sm"
                onClick={handleCopyInviteLink}
                disabled={!coachCode}
                className="h-8 rounded-lg border border-white/10 px-4 text-xs tracking-[0.06em] text-muted-foreground hover:bg-white/5 hover:text-foreground"
              >
                {copied ? (
                  <><Check className="mr-1.5 h-3 w-3 text-primary" />Link copiado!</>
                ) : (
                  <><Copy className="mr-1.5 h-3 w-3" />Copiar link de convite</>
                )}
              </Button>
            )}
          </div>
        ) : (
          <>
            {schedule.map((item) => (
              <TrainingRow
                key={item.userId}
                item={item}
                onEdit={() => onEditItem(item)}
              />
            ))}
            {atLimit && <GhostRows />}
          </>
        )}
        {!loading && (
          <div className="border-t border-white/5 px-6 py-3">
            <Link
              to={ROUTES.coachUsers}
              className="text-xs text-muted-foreground/50 hover:text-muted-foreground transition-colors"
            >
              Ver todos os alunos →
            </Link>
          </div>
        )}
      </CardContent>
    </Card>
  );
}
