import { useState, useCallback } from "react";
import { toast } from "sonner";
import { createSignupCheckout } from "@/shared/services/checkoutService";

export function useCheckout() {
  const [loading, setLoading] = useState(false);

  const go = useCallback(async (planCode) => {
    try {
        setLoading(true);
        const url = await createSignupCheckout(planCode);
        window.location.replace(url);
    } catch (e) {
        toast.error(e?.message || "Não foi possível iniciar o checkout. Tente novamente.");
        setLoading(false);
    }
  }, []);

  return { loading, go };
}
