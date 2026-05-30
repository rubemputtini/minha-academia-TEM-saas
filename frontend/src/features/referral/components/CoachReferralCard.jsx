import { Badge } from "@/components/ui/badge";
import { Card, CardContent, CardFooter } from "@/components/ui/card";
import { GiftIcon } from "@heroicons/react/24/outline";
import { cn } from "@/lib/utils";
import { CARD_BASE } from "@/shared/styles/cards";
import ReferralStats from "./ReferralStats";
import ReferralCodeSection from "./ReferralCodeSection";
import ReferralSteps from "./ReferralSteps";

export default function CoachReferralCard({ referral }) {
  const { referralCode, totalCreditsEarned, creditsAvailable, usedCredits } =
    referral || {};

  return (
    <Card className={cn(CARD_BASE, "relative overflow-hidden")}>
      <div className="pointer-events-none absolute -right-5 -top-6 opacity-[0.035]">
        <GiftIcon className="h-36 w-36" />
      </div>
      <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-primary/40 to-transparent" />

      <CardContent className="relative z-10 space-y-8 px-6 pt-5 pb-2">
        <div className="flex flex-col items-start justify-between gap-4 sm:flex-row sm:items-center">
          <div>
            <p className="text-[10px] font-semibold uppercase tracking-[0.14em] text-muted-foreground/50">
              Programa de indicação
            </p>
            <h2 className="mt-1.5 text-xl font-bold tracking-tight">
              Painel de recompensas
            </h2>
            <p className="mt-0.5 text-sm text-muted-foreground/70">
              Acompanhe suas indicações e seus descontos acumulados.
            </p>
          </div>

          <Badge
            variant="outline"
            className="shrink-0 inline-flex items-center gap-1.5 rounded-full border-emerald-500/40 bg-emerald-500/8 px-3.5 py-1 text-xs font-semibold tracking-wide text-emerald-400 uppercase"
          >
            <GiftIcon className="h-3.5 w-3.5" />
            50% desconto
          </Badge>
        </div>

        <ReferralStats
          totalCreditsEarned={totalCreditsEarned}
          creditsAvailable={creditsAvailable}
          usedCredits={usedCredits}
        />

        <ReferralCodeSection referralCode={referralCode} />

        <ReferralSteps />
      </CardContent>

      <CardFooter className="relative z-10 justify-center pb-4 pt-2">
        <p className="text-[10px] text-muted-foreground/40">
          *Desconto não cumulativo dentro do mesmo mês.
        </p>
      </CardFooter>
    </Card>
  );
}
