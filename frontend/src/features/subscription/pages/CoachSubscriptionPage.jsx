import AppLayout from "@/shared/layout/AppLayout";
import { Separator } from "@/components/ui/separator";
import PageHeader from "@/shared/components/PageHeader";
import { useCoachSubscription } from "../hooks/useCoachSubscription";
import CoachSubscriptionCard from "@/features/account/components/CoachSubscriptionCard";
import LoadingCard from "@/shared/ui/LoadingCard";

export default function CoachSubscriptionPage() {
  const {
    loading,
    subscription,
    managingSubscription,
    handleManageSubscription,
  } = useCoachSubscription();

  return (
    <AppLayout>
      <div className="max-w-4xl mx-auto px-4 py-6 space-y-6">
        <PageHeader
          title="Minha assinatura"
          subtitle="Gerencie seu plano e suas informações de cobrança."
          align="left"
        />

        <Separator />

        {loading ? (
          <LoadingCard />
        ) : (
          <CoachSubscriptionCard
            subscription={subscription}
            managingSubscription={managingSubscription}
            onManageSubscription={handleManageSubscription}
          />
        )}
      </div>
    </AppLayout>
  );
}
