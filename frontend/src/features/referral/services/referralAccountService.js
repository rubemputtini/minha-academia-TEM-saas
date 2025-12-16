import api from "@/shared/services/api";
import { handleError } from "@/shared/services/handleError";

export async function getMyCoachReferral() {
  try {
    const response = await api.get("/api/v1/referral-accounts/me");

    return response.data;
  } catch (error) {
    handleError(error, "Não foi possível buscar os dados de indicação.");
  }
}
