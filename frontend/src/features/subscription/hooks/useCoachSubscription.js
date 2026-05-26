import { useCallback, useEffect, useState } from "react";
import { toast } from "sonner";

import { getMyCoach } from "@/features/account/services/accountService";
import { createBillingPortalSession } from "@/features/billing/services/billingService";
import { createUpgradeCheckout } from "@/features/billing/services/checkoutSessionsService";

export function useCoachSubscription() {
  const [loading, setLoading] = useState(true);
  const [managingSubscription, setManagingSubscription] = useState(false);
  const [upgradingPlan, setUpgradingPlan] = useState(null);

  const [subscription, setSubscription] = useState({
    status: "",
    plan: "",
    endAt: null,
  });

  const handleManageSubscription = useCallback(async () => {
    setManagingSubscription(true);

    try {
      const url = await createBillingPortalSession();
      if (url) {
        window.location.href = url;
      }
    } catch (error) {
      toast.error(
        error?.message ||
          "Não foi possível abrir a página de gerenciamento da assinatura."
      );
    } finally {
      setManagingSubscription(false);
    }
  }, []);

  const handleUpgrade = useCallback(async (plan) => {
    setUpgradingPlan(plan);

    try {
      const data = await createUpgradeCheckout(plan);
      
      if (data?.url) {
        window.location.href = data.url;
      }
    } catch (error) {
      toast.error(error?.message || "Não foi possível iniciar o checkout.");
    } finally {
      setUpgradingPlan(null);
    }
  }, []);

  useEffect(() => {
    async function load() {
      setLoading(true);

      try {
        const response = await getMyCoach();

        if (response) {
          setSubscription({
            status: response.subscriptionStatus || "",
            plan: response.subscriptionPlan || "",
            endAt: response.subscriptionEndAt || null,
          });
        }
      } catch (error) {
        toast.error(error?.message || "Erro ao carregar sua assinatura.");
      } finally {
        setLoading(false);
      }
    }

    load();
  }, []);

  return {
    loading,
    subscription,
    managingSubscription,
    handleManageSubscription,
    upgradingPlan,
    handleUpgrade,
  };
}
