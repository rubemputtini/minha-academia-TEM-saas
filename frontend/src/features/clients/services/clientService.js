import api from "@/shared/services/api";
import { handleError } from "@/shared/services/handleError";

export async function getCoachClients(page = 1, pageSize = 10, searchTerm = "") {
  try {
    const response = await api.get("/api/v1/coaches/me/clients", {
      params: { page, pageSize, searchTerm: searchTerm || undefined },
    });

    return response.data;
  } catch (error) {
    handleError(error, "Não foi possível carregar os alunos.");
  }
}

export async function deleteCoachClient(userId) {
  try {
    await api.delete(`/api/v1/coaches/me/clients/${userId}`);
  } catch (error) {
    handleError(error, "Não foi possível remover o aluno.");
  }
}

export async function setClientActive(userId, isActive) {
  try {
    await api.patch(`/api/v1/coaches/me/clients/${userId}/active`, { isActive });
  } catch (error) {
    handleError(error, "Não foi possível alterar o status do aluno.");
  }
}

export async function updateTrainingDate(userId, nextTrainingChangeAt) {
  try {
    await api.put(`/api/v1/coaches/me/clients/${userId}/training-date`, {
      nextTrainingChangeAt,
    });
  } catch (error) {
    handleError(error, "Não foi possível atualizar a data de treino.");
  }
}
