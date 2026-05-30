import { CheckBadgeIcon, UserGroupIcon } from "@heroicons/react/24/outline";

export default function ReferralStats({
  totalCreditsEarned,
  creditsAvailable,
  usedCredits,
}) {
  return (
    <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
      <div className="flex flex-col gap-1 rounded-xl border border-white/8 bg-white/[0.03] p-5">
        <div className="mb-2 flex items-center gap-2 text-muted-foreground/60">
          <UserGroupIcon className="h-4 w-4" />
          <span className="text-[10px] font-semibold uppercase tracking-[0.14em]">
            Indicações totais
          </span>
        </div>
        <span className="text-4xl font-bold font-mono tabular-nums leading-none">
          {totalCreditsEarned ?? "—"}
        </span>
        <p className="mt-1.5 text-xs text-muted-foreground/50">
          Treinadores que usaram seu código
        </p>
      </div>

      <div className="relative flex flex-col gap-1 overflow-hidden rounded-xl border border-emerald-500/20 bg-emerald-500/[0.06] p-5">
        <div className="pointer-events-none absolute -right-4 -top-4 h-24 w-24 rounded-full bg-emerald-500/10 blur-2xl" />
        <div className="mb-2 flex items-center gap-2 text-emerald-400">
          <CheckBadgeIcon className="h-4 w-4" />
          <span className="text-[10px] font-semibold uppercase tracking-[0.14em]">
            Meses garantidos
          </span>
        </div>
        <span className="text-4xl font-bold font-mono tabular-nums leading-none text-emerald-100">
          {creditsAvailable ?? "—"}
        </span>
        <p className="mt-1.5 text-xs text-emerald-200/50">
          {usedCredits > 0
            ? `${usedCredits} meses já utilizados`
            : "Disponíveis para uso futuro"}
        </p>
      </div>
    </div>
  );
}
