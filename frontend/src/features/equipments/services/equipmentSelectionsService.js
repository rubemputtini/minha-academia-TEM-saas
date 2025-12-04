import api from "@/shared/services/api";
import { handleError } from "@/shared/services/handleError";

export async function getUserView() {
    try {
        const response = await api.get(
            `/api/v1/equipment-selections/me`
        );

        return response.data;
    } catch (error) {
        handleError(error, "Não foi possível buscar a sua seleção de equipamentos.");
    }
}

export async function getCoachView(userId) {
    try {
        const response = await api.get(
            `/api/v1/equipment-selections/users/${userId}`
        );

        return response.data;
    } catch (error) {
        handleError(error, "Não foi possível buscar a seleção de equipamentos do aluno.");
    }
}

export async function saveOwn({ availableEquipmentIds }) {
    try {
        const response = await api.put(
            `/api/v1/equipment-selections/me`, { availableEquipmentIds }
        );

        return response.data;
    } catch (error) {
        handleError(error, "Não foi possível salvar a sua seleção de equipamentos.");
    }
}

export async function saveClient(userId, request) {
    try {
        const response = await api.put(
            `/api/v1/equipment-selections/users/${userId}`, request
        );

        return response.data;
    } catch (error) {
        handleError(error, "Não foi possível salvar a seleção de equipamentos do aluno.");
    }
}