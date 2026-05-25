import api from "@/shared/services/api";
import { handleError } from "@/shared/services/handleError";

export async function getTrainingSchedule() {
  try {
    const response = await api.get("/api/v1/coaches/me/training-schedule");
    
    return response.data;
  } catch (error) {
    handleError(error, "Não foi possível carregar o cronograma de trocas.");
  }
}

export async function updateTrainingDate(userId, nextTrainingChangeAt) {
  try {
    await api.put(`/api/v1/coaches/me/clients/${userId}/training-date`, {
      nextTrainingChangeAt,
    });
  } catch (error) {
    handleError(error, "Não foi possível atualizar a data de troca.");
  }
}
