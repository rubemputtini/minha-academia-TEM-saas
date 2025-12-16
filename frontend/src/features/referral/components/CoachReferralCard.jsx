import { Badge } from "@/components/ui/badge";
import {
  Card,
  CardHeader,
  CardTitle,
  CardDescription,
  CardContent,
  CardFooter,
} from "@/components/ui/card";
import { GiftIcon } from "@heroicons/react/24/outline";
import ReferralStats from "./ReferralStats";
import ReferralCodeSection from "./ReferralCodeSection";
import ReferralSteps from "./ReferralSteps";

export default function CoachReferralCard({ referral }) {
  const { referralCode, totalCreditsEarned, creditsAvailable, usedCredits } =
    referral || {};

  return (
    <Card className="overflow-hidden bg-card border-border/40 shadow-sm">
      <CardHeader className="pb-2 text-center sm:text-left">
        <div className="flex flex-col items-center justify-between gap-4 sm:flex-row">
          <div>
            <CardTitle className="text-xl font-bold tracking-tight">
              Painel de recompensas
            </CardTitle>
            <CardDescription className="mt-1 text-base">
              Acompanhe suas indicações e seus descontos acumulados.
            </CardDescription>
          </div>

          <Badge
            variant="outline"
            className="hidden items-center gap-1 rounded-full border-emerald-500/40 bg-emerald-500/5 text-emerald-400 uppercase tracking-wider sm:inline-flex"
          >
            <GiftIcon className="h-3.5 w-3.5" />
            <span>50% desconto</span>
          </Badge>
        </div>
      </CardHeader>

      <CardContent className="space-y-8 pt-6">
        <ReferralStats
          totalCreditsEarned={totalCreditsEarned}
          creditsAvailable={creditsAvailable}
          usedCredits={usedCredits}
        />

        <ReferralCodeSection referralCode={referralCode} />

        <ReferralSteps />
      </CardContent>

      <CardFooter className="justify-center pb-4 pt-0">
        <p className="text-[10px] text-muted-foreground/50">
          *Desconto não cumulativo dentro do mesmo mês.
        </p>
      </CardFooter>
    </Card>
  );
}
