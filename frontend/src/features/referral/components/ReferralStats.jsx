import { CheckBadgeIcon, UserGroupIcon } from "@heroicons/react/24/outline";

export default function ReferralStats({
  totalCreditsEarned,
  creditsAvailable,
  usedCredits,
}) {
  return (
    <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
      {/* Indicações totais */}
      <div className="flex flex-col gap-1 rounded-xl border border-border/50 bg-muted/30 p-4">
        <div className="mb-1 flex items-center gap-2 text-muted-foreground">
          <UserGroupIcon className="h-4 w-4" />
          <span className="text-xs font-semibold uppercase tracking-wider">
            Indicações totais
          </span>
        </div>
        <span className="text-3xl font-bold text-foreground">
          {totalCreditsEarned}
        </span>
        <p className="text-xs text-muted-foreground/70">
          Treinadores que usaram seu código
        </p>
      </div>

      {/* Meses garantidos */}
      <div className="relative flex flex-col gap-1 overflow-hidden rounded-xl border border-emerald-500/20 bg-emerald-500/5 p-4">
        <div className="pointer-events-none absolute -right-4 -top-4 h-20 w-20 rounded-full bg-emerald-500/10 blur-2xl" />
        <div className="mb-1 flex items-center gap-2 text-emerald-400">
          <CheckBadgeIcon className="h-4 w-4" />
          <span className="text-xs font-semibold uppercase tracking-wider">
            Meses garantidos
          </span>
        </div>
        <span className="text-3xl font-bold text-emerald-100">
          {creditsAvailable}
        </span>
        <p className="text-xs text-emerald-200/60">
          {usedCredits > 0
            ? `${usedCredits} meses já utilizados`
            : "Disponíveis para uso futuro"}
        </p>
      </div>
    </div>
  );
}
