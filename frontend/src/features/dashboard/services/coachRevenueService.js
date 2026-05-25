import api from "@/shared/services/api";
import { handleError } from "@/shared/services/handleError";

export async function getCoachRevenue() {
  try {
    const response = await api.get("/api/v1/account/coaches/me");
    const { monthlyRate, currency, subscriptionPlan, usersLimit } = response.data;

    return { monthlyRate, currency, subscriptionPlan, usersLimit };
  } catch (error) {
    handleError(error, "Não foi possível carregar as configurações de faturamento.");
  }
}

export async function updateCoachRevenue(monthlyRate, currency) {
  try {
    const response = await api.put("/api/v1/account/coaches/me/rate", {
      monthlyRate,
      currency,
    });
    const { monthlyRate: rate, currency: updatedCurrency } = response.data;

    return { monthlyRate: rate, currency: updatedCurrency };
  } catch (error) {
    handleError(error, "Não foi possível salvar as configurações de faturamento.");
  }
}
