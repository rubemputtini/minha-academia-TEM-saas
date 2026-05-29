import { Users } from "lucide-react";
import { Card, CardContent } from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import { cn } from "@/lib/utils";
import { CARD_BASE } from "@/shared/styles/cards";

export function AdminStatsOverviewCard({ loading, stats }) {
  return (
    <Card className={cn(CARD_BASE, "relative overflow-hidden")}>
      <div className="pointer-events-none absolute -right-3 -top-5 opacity-[0.04]">
        <Users className="h-28 w-28" />
      </div>
      <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-primary/40 to-transparent" />

      <CardContent className="relative z-10 space-y-4 px-6 py-5">
        <p className="text-xs font-semibold uppercase tracking-[0.12em] text-muted-foreground/90">
          Plataforma
        </p>

        {loading ? (
          <div className="space-y-2">
            <Skeleton className="h-12 w-16 bg-white/6" />
            <Skeleton className="h-3 w-24 bg-white/4" />
            <Skeleton className="mt-3 h-px w-full bg-white/4" />
            <Skeleton className="h-3 w-full bg-white/4" />
            <Skeleton className="h-3 w-full bg-white/4" />
            <Skeleton className="h-3 w-full bg-white/4" />
            <Skeleton className="h-3 w-full bg-white/4" />
            <Skeleton className="h-3 w-full bg-white/4" />
          </div>
        ) : (
          <>
            <div className="flex items-end gap-2 font-mono">
              <span className="text-5xl font-bold leading-none text-foreground">
                {stats?.totalUsers ?? "—"}
              </span>
              <span className="pb-1 text-xs tracking-[0.08em] text-muted-foreground/75">
                {stats?.totalUsers === 1 ? "aluno" : "alunos"}
              </span>
            </div>

            <div className="border-t border-white/8 pt-3 space-y-2">
              <div className="flex items-center justify-between pb-0.5">
                <p className="text-xs font-semibold uppercase tracking-[0.1em] text-muted-foreground/70">
                  Treinadores
                </p>
                {stats?.newCoachesThisMonth > 0 && (
                  <span className="font-mono text-xs text-primary/80">
                    +{stats.newCoachesThisMonth} este mês
                  </span>
                )}
              </div>
              <div className="flex items-center justify-between text-xs">
                <span className="flex items-center gap-2 text-muted-foreground/75">
                  <span className="h-1.5 w-1.5 shrink-0 rounded-full bg-primary/70" />
                  Assinantes
                </span>
                <span className={cn("font-mono font-semibold", stats?.activeCoaches > 0 ? "text-primary" : "text-muted-foreground/65")}>
                  {stats?.activeCoaches ?? "—"}
                </span>
              </div>
              <div className="flex items-center justify-between text-xs">
                <span className="flex items-center gap-2 text-muted-foreground/75">
                  <span className="h-1.5 w-1.5 shrink-0 rounded-full bg-white/25" />
                  Grátis
                </span>
                <span className="font-mono font-semibold text-muted-foreground/65">
                  {stats?.trialCoaches ?? "—"}
                </span>
              </div>
              <div className="flex items-center justify-between text-xs">
                <span className="flex items-center gap-2 text-muted-foreground/75">
                  <span className="h-1.5 w-1.5 shrink-0 rounded-full bg-red-400/70" />
                  Em atraso
                </span>
                <span className={cn("font-mono font-semibold", stats?.pastDueCoaches > 0 ? "text-red-400" : "text-muted-foreground/65")}>
                  {stats?.pastDueCoaches ?? "—"}
                </span>
              </div>
              <div className="flex items-center justify-between text-xs">
                <span className="flex items-center gap-2 text-muted-foreground/75">
                  <span className="h-1.5 w-1.5 shrink-0 rounded-full bg-white/15" />
                  Cancelados
                </span>
                <span className="font-mono font-semibold text-muted-foreground/65">
                  {stats?.canceledCoaches ?? "—"}
                </span>
              </div>
              <div className="flex items-center justify-between text-xs">
                <span className="text-muted-foreground/75">Total</span>
                <span className="font-mono font-semibold text-foreground">
                  {stats?.totalCoaches ?? "—"}
                </span>
              </div>
              {stats?.coachesWithoutClients > 0 && (
                <div className="flex items-center justify-between text-xs pt-1 border-t border-white/8 mt-1">
                  <span className="text-amber-400/80">Sem alunos &gt;30d</span>
                  <span className="font-mono font-semibold text-amber-400">
                    {stats.coachesWithoutClients}
                  </span>
                </div>
              )}
            </div>
          </>
        )}
      </CardContent>
    </Card>
  );
}
