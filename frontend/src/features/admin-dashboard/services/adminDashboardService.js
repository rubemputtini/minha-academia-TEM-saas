import api from "@/shared/services/api";
import { handleError } from "@/shared/services/handleError";

export async function getAdminStats() {
  try {
    const response = await api.get("/api/v1/admin/stats");

    return response.data;
  } catch (error) {
    handleError(error, "Não foi possível carregar as estatísticas.");
  }
}

export async function getAdminCoaches(page = 1, pageSize = 10) {
  try {
    const response = await api.get("/api/v1/admin/coaches", { params: { page, pageSize } });

    return response.data;
  } catch (error) {
    handleError(error, "Não foi possível carregar os treinadores.");
  }
}
