import api from "@/shared/services/api";
import { handleError } from "@/shared/services/handleError";

export async function getMyEquipmentNote() {
  try {
    const response = await api.get("/api/v1/equipment-notes/me");

    return response.data;
  } catch (error) {
    handleError(error, "Não foi possível buscar sua observação.");
  }
}

export async function getClientEquipmentNote(userId) {
  try {
    const response = await api.get(`/api/v1/equipment-notes/user/${userId}`);

    return response.data;
  } catch (error) {
    handleError(error, "Não foi possível buscar a observação do aluno.");
  }
}

export async function upsertMyEquipmentNote(message) {
  try {
    const response = await api.put("/api/v1/equipment-notes/me", { message });
    
    return response.data;
  } catch (error) {
    handleError(error, "Não foi possível salvar a observação.");
  }
}

export async function deleteMyEquipmentNote() {
  try {
    await api.delete("/api/v1/equipment-notes/me");
  } catch (error) {
    handleError(error, "Não foi possível apagar a observação.");
  }
}
