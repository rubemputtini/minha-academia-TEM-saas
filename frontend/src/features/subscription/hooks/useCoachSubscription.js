import { useCallback, useEffect, useState } from "react";
import { toast } from "sonner";

import { getMyCoach } from "@/features/account/services/accountService";
import { createBillingPortalSession } from "@/features/billing/services/billingService";

export function useCoachSubscription() {
  const [loading, setLoading] = useState(true);
  const [managingSubscription, setManagingSubscription] = useState(false);

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
  };
}
