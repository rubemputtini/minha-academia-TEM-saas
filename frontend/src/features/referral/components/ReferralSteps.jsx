import {
  GiftTopIcon,
  ShareIcon,
  UserPlusIcon,
} from "@heroicons/react/24/outline";

const STEPS = [
  {
    icon: ShareIcon,
    title: "1. Compartilhe",
    description: "Envie seu código único para outros treinadores.",
    accent: false,
  },
  {
    icon: UserPlusIcon,
    title: "2. Eles usam",
    description: (
      <>
        O treinador indicado ganha{" "}
        <span className="font-medium text-emerald-400">50% OFF</span> no 1º mês.
      </>
    ),
    accent: false,
  },
  {
    icon: GiftTopIcon,
    title: "3. Você ganha",
    description: (
      <>
        Sua próxima fatura vem com{" "}
        <span className="font-medium text-emerald-400">50% de desconto</span>.
      </>
    ),
    accent: true,
  },
];

export default function ReferralSteps() {
  return (
    <div className="rounded-xl border border-white/6 bg-white/[0.02] p-5">
      <div className="grid gap-6 sm:grid-cols-3">
        {STEPS.map((step) => (
          <div
            key={step.title}
            className="flex flex-col items-center gap-3 text-center"
          >
            <div
              className={
                step.accent
                  ? "flex h-10 w-10 shrink-0 items-center justify-center rounded-full border border-emerald-500/40 bg-emerald-500/10 shadow-[0_0_20px_rgba(34,197,94,0.2)]"
                  : "flex h-10 w-10 shrink-0 items-center justify-center rounded-full border border-white/10 bg-white/[0.04]"
              }
            >
              <step.icon
                className={`h-5 w-5 ${step.accent ? "text-emerald-400" : "text-muted-foreground/60"}`}
              />
            </div>

            <div className="space-y-1">
              <p className="text-xs font-semibold text-foreground">
                {step.title}
              </p>
              <p className="text-[11px] leading-relaxed text-muted-foreground/70">
                {step.description}
              </p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
