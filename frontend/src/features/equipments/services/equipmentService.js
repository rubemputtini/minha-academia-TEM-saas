import api from "@/shared/services/api";
import { handleError } from "@/shared/services/handleError";

export async function getByCoach(coachId) {
    try {
        const response = await api.get(
            `/api/v1/equipments/coaches/${coachId}`
        );

        return response.data;
    } catch (error) {
        handleError(error, "Não foi possível buscar os equipamentos do treinador.");
    }
}

export async function getActiveByCoach(coachId) {
    try {
        const response = await api.get(
            `/api/v1/equipments/coaches/${coachId}/active`
        );

        return response.data;
    } catch (error) {
        handleError(error, "Não foi possível buscar os equipamentos ativos do treinador.");
    }
}

export async function getAll() {
    try {
        const response = await api.get(
            `/api/v1/equipments/me`
        );
        
        return response.data;
    } catch (error) {
        handleError(error, "Não foi possível buscar todos os seus equipamentos.");
    }
}

export async function getById(id) {
    try {
        const response = await api.get(
            `/api/v1/equipments/${id}`,
        );

        return response.data;
    } catch (error) {
        handleError(error, "Não foi possível buscar o equipamento.");
    }
}

export async function create(request) {
    try {
        const response = await api.post(
            `/api/v1/equipments`, request
        );

        return response.data;
    } catch (error) {
        handleError(error, "Não foi possível criar o equipamento.");
    }
}

export async function update(id, request) {
    try {
        const response = await api.put(
            `/api/v1/equipments/${id}`, request
        );

        return response.data;
    } catch (error) {
        handleError(error, "Não foi possível atualizar o equipamento.");
    }
}

export async function remove(id) {
    try {
        await api.delete(
            `/api/v1/equipments/${id}`,
        );

    } catch (error) {
        handleError(error, "Não foi possível excluir o equipamento.");
    }
}

export async function setStatus(id, request) {
    try {
        const response = await api.patch(
            `/api/v1/equipments/${id}/active`, request
        );

        return response.data;
    } catch (error) {
        handleError(error, "Não foi possível alterar o status do equipamento.");
    }
}