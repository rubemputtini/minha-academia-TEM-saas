import { Link } from "react-router-dom";
import { ChevronRight, Gift, UserCheck, ArrowUp, ArrowDown } from "lucide-react";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { cn } from "@/lib/utils";
import { CARD_BASE } from "@/shared/styles/cards";
import { Badge } from "@/components/ui/badge";
import { Skeleton } from "@/components/ui/skeleton";
import { ROUTES } from "@/shared/routes/routes";
import { getInitials } from "@/shared/utils/getInitials";

const PLAN_LABEL = {
  Unlimited: { label: "Unlimited", className: "text-amber-400" },
  Basic: { label: "Basic", className: "text-sky-400" },
};

const STATUS_LABEL = {
  Trial: { label: "Trial", className: "text-muted-foreground/50" },
  PastDue: { label: "Em atraso", className: "text-red-400" },
  Canceled: { label: "Cancelado", className: "text-muted-foreground/40" },
};

function getPlanLabel(coach) {
  if (coach.subscriptionStatus === "Active")
    return PLAN_LABEL[coach.subscriptionPlan] ?? { label: coach.subscriptionPlan, className: "text-amber-400" };
  
  return STATUS_LABEL[coach.subscriptionStatus] ?? { label: coach.subscriptionStatus, className: "text-muted-foreground/50" };
}


function CoachRowSkeleton() {
  return (
    <div className="flex items-center justify-between border-b border-white/5 px-3 py-4 sm:px-6">
      <div className="flex items-center gap-3">
        <Skeleton className="h-9 w-9 shrink-0 rounded-full bg-white/6" />
        <div className="space-y-2">
          <Skeleton className="h-3.5 w-28 bg-white/6" />
          <Skeleton className="h-3 w-40 bg-white/4" />
        </div>
      </div>
      <div className="flex items-center gap-2 sm:gap-3">
        <Skeleton className="hidden h-3 w-16 bg-white/4 sm:block" />
        <Skeleton className="h-5 w-20 rounded-full bg-white/6" />
      </div>
    </div>
  );
}

function CoachRow({ coach }) {
  const initials = getInitials(coach.name);
  const label = getPlanLabel(coach);

  return (
    <div className="flex items-center justify-between border-b border-white/5 px-3 py-3.5 sm:px-6">
      <div className="flex min-w-0 items-center gap-3">
        <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-full bg-white/8 text-xs font-semibold tracking-wide text-muted-foreground/80">
          {initials}
        </div>
        <div className="min-w-0">
          <div className="flex min-w-0 items-center gap-1.5">
            <p className="truncate text-sm font-medium text-foreground">{coach.name}</p>
            <span className={`shrink-0 text-xs font-medium ${label.className}`}>
              · {label.label}
            </span>
          </div>
          <p className="truncate text-xs text-muted-foreground/70">{coach.email}</p>
        </div>
      </div>

      <div className="flex shrink-0 items-center gap-3">
        {coach.referralsCount > 0 && (
          <span className="hidden items-center gap-1 text-xs text-muted-foreground/60 sm:flex">
            <Gift className="h-3 w-3" />
            {coach.referralsCount}
          </span>
        )}
        <span className="hidden text-xs text-muted-foreground/60 sm:block">
          {coach.clientsCount} {coach.clientsCount === 1 ? "aluno" : "alunos"}
        </span>
      </div>
    </div>
  );
}

export function AdminCoachListCard({ loading, coaches, totalCoaches, stats }) {

  return (
    <Card className={cn(CARD_BASE, "relative flex h-full flex-col overflow-hidden")}>
      <div className="pointer-events-none absolute -right-3 -top-5 opacity-[0.04]">
        <UserCheck className="h-28 w-28" />
      </div>
      <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-primary/40 to-transparent" />
      <CardHeader className="relative z-10 flex flex-row items-center justify-between gap-3 border-b border-white/10 py-5">
        <div className="min-w-0 space-y-1.5">
          <CardTitle className="text-xl font-semibold tracking-tight text-foreground sm:text-2xl">
            Treinadores
          </CardTitle>

          {loading ? (
            <Skeleton className="hidden h-4 w-44 bg-white/4 sm:block" />
          ) : (
            <p className="hidden items-center gap-2 text-sm sm:flex">
              <span
                className={
                  stats?.activeCoaches > 0
                    ? "font-medium text-primary"
                    : "text-muted-foreground/60"
                }
              >
                {stats?.activeCoaches ?? 0} assinante{stats?.activeCoaches !== 1 ? "s" : ""}
              </span>
              
              {(() => {
                const delta = (stats?.newActiveSubscriptionsThisMonth ?? 0) - (stats?.canceledSubscriptionsThisMonth ?? 0);
                if (delta === 0) return null;
                return delta > 0 ? (
                  <span className="flex items-center gap-0.5 font-mono text-xs font-semibold text-emerald-400">
                    <ArrowUp className="h-3 w-3" />{delta}
                  </span>
                ) : (
                  <span className="flex items-center gap-0.5 font-mono text-xs font-semibold text-red-400">
                    <ArrowDown className="h-3 w-3" />{Math.abs(delta)}
                  </span>
                );
              })()}
              
              <span className="text-muted-foreground/25">·</span>
              <span className="text-muted-foreground/60">
                {stats?.trialCoaches ?? 0} grátis
              </span>
              {stats?.pastDueCoaches > 0 && (
                <>
                  <span className="text-muted-foreground/25">·</span>
                  <span className="font-medium text-red-400">
                    {stats.pastDueCoaches} em atraso
                  </span>
                </>
              )}
            </p>
          )}
        </div>

        <Badge className="pointer-events-none shrink-0 rounded-full bg-white/5 px-4 py-1 font-mono text-xs tracking-[0.1em] text-muted-foreground/90">
          {loading ? "…" : `${totalCoaches} TOTAL`}
        </Badge>
      </CardHeader>

      <CardContent className="relative z-10 p-0">
        {loading ? (
          Array.from({ length: 3 }).map((_, i) => <CoachRowSkeleton key={i} />)
        ) : coaches.length === 0 ? (
          <div className="flex flex-col items-center gap-4 px-6 py-12 text-center">
            <div className="flex h-12 w-12 shrink-0 items-center justify-center rounded-full border border-white/10 bg-white/[0.03] text-muted-foreground/40">
              <UserCheck className="h-5 w-5" />
            </div>
            <p className="text-sm font-medium text-muted-foreground/70">
              Nenhum treinador cadastrado
            </p>
          </div>
        ) : (
          <>
            {coaches.map((coach) => (
              <CoachRow key={coach.id} coach={coach} />
            ))}
            {totalCoaches > coaches.length && (
              <Link to={ROUTES.adminCoaches} className="group block border-t border-white/5">
                <div className="relative flex items-center justify-between px-6 py-4 transition-colors hover:bg-white/[0.03]">
                  <div className="pointer-events-none absolute inset-0 bg-gradient-to-r from-primary/0 via-primary/[0.05] to-primary/0 opacity-0 transition-opacity group-hover:opacity-100" />
                  <span className="relative text-xs tracking-[0.06em] text-muted-foreground/55 transition-colors group-hover:text-muted-foreground/90">
                    Ver todos os treinadores
                  </span>
                  <ChevronRight className="relative h-3.5 w-3.5 text-muted-foreground/35 transition-all group-hover:translate-x-0.5 group-hover:text-muted-foreground/70" />
                </div>
              </Link>
            )}
          </>
        )}
      </CardContent>
    </Card>
  );
}
