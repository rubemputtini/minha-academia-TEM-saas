import api from "@/shared/services/api";
import { handleError } from "@/shared/services/handleError";

export async function createBillingPortalSession() {
  try {
    const { data } = await api.post("/api/v1/billing-portal/sessions", null, {
      timeout: 15000,
    });

    return data?.url;
  } catch (error) {
    handleError(
      error,
      "Não foi possível abrir a página de gerenciamento da assinatura."
    );
  }
}
