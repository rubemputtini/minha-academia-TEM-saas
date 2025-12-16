import api from "@/shared/services/api";
import { handleError } from "@/shared/services/handleError";

export async function getMySubscription() {
  try {
    const response = await api.get("/api/v1/subscriptions/me");

    return response.data;
  } catch (error) {
    handleError(error, "Não foi possível buscar os dados de assinatura.");
  }
}

export async function cancelAtPeriodEnd() {
  try {
    const response = await api.post(
      "/api/v1/subscriptions/cancel-at-period-end"
    );

    return response.data;
  } catch (error) {
    handleError(error, "Não foi possível cancelar a assinatura.");
  }
}

export async function undoCancel() {
  try {
    const response = await api.post(
      "/api/v1/subscriptions/undo-scheduled-cancel"
    );

    return response.data;
  } catch (error) {
    handleError(error, "Não foi possível desfazer o cancelamento.");
  }
}
