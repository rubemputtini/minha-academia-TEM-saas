import { useState, useEffect } from "react";
import { toast } from "sonner";
import { getCoachRevenue, updateCoachRevenue } from "../services/coachRevenueService";

export function useCoachRevenue() {
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [revenue, setRevenue] = useState({ monthlyRate: null, currency: "BRL", subscriptionPlan: null, usersLimit: null, coachCode: null });

  useEffect(() => {
    async function load() {
      try {
        const data = await getCoachRevenue();
        if (data) setRevenue(data);
      } catch (error) {
        toast.error(error?.message || "Erro ao carregar faturamento.");
      } finally {
        setLoading(false);
      }
    }
    load();
  }, []);

  async function saveRevenue(monthlyRate, currency) {
    setSaving(true);
    try {
      const data = await updateCoachRevenue(monthlyRate, currency);
      if (data) {
        setRevenue((prev) => ({ ...prev, ...data }));
        toast.success("Configurações de faturamento salvas.");
      }
    } catch (error) {
      toast.error(error?.message || "Erro ao salvar faturamento.");
    } finally {
      setSaving(false);
    }
  }

  return { loading, saving, revenue, saveRevenue };
}
