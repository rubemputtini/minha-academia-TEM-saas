import { useState, useCallback } from "react";
import { createSignupCheckout } from "@/shared/services/checkoutService";

export function useCheckout() {
  const [loading, setLoading] = useState(false);

  const go = useCallback(async (planCode) => {
    try {
        setLoading(true);
        const url = await createSignupCheckout(planCode);
        window.location.replace(url);
    } catch (e) {
        console.error(e);
        setLoading(false);
    }
  }, []);

  return { loading, go };
}
