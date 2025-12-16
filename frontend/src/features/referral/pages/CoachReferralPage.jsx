import AppLayout from "@/shared/layout/AppLayout";
import { Separator } from "@/components/ui/separator";
import PageHeader from "@/shared/components/PageHeader";
import CoachReferralCard from "@/features/referral/components/CoachReferralCard";
import { useCoachReferral } from "../hooks/useCoachReferral";
import LoadingCard from "@/shared/ui/LoadingCard";

export default function CoachReferralPage() {
  const { loading, referral } = useCoachReferral();

  return (
    <AppLayout>
      <div className="max-w-4xl mx-auto px-4 py-6 space-y-6">
        <PageHeader
          title="Indique e ganhe"
          subtitle="Indique treinadores e ganhe descontos na sua assinatura."
          align="left"
        />

        <Separator />

        {loading ? <LoadingCard /> : <CoachReferralCard referral={referral} />}
      </div>
    </AppLayout>
  );
}
