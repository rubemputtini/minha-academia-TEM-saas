import { useEffect, useState } from "react";
import { toast } from "sonner";
import { getMyCoachReferral } from "../services/referralAccountService";

export function useCoachReferral() {
  const [loading, setLoading] = useState(true);
  const [referral, setReferral] = useState(null);

  useEffect(() => {
    async function load() {
      setLoading(true);
      try {
        const response = await getMyCoachReferral();

        if (response) {
          setReferral(response);
        }
      } catch (error) {
        toast.error(
          error?.message || "Erro ao carregar seus dados de indicação."
        );
      } finally {
        setLoading(false);
      }
    }

    load();
  }, []);

  return { loading, referral };
}
