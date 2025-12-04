import api from "@/shared/services/api";
import { handleError } from "@/shared/services/handleError";

export async function getMyUser() {
    try {
        const response = await api.get(
            `/api/v1/account/users/me`
        );

        return response.data;
    } catch (error) {
        handleError(error, "Não foi possível buscar os detalhes da sua conta aluno.");
    }
}

export async function updateMyUser(request) {
    try {
        const response = await api.put(
            `/api/v1/account/users/me`, request
        );

        return response.data;
    } catch (error) {
        handleError(error, "Não foi possível atualizar os detalhes da sua conta aluno.");
    }
}

export async function getMyCoach() {
    try {
        const response = await api.get(
            `/api/v1/account/coaches/me`
        );

        return response.data;
    } catch (error) {
        handleError(error, "Não foi possível buscar os detalhes da sua conta treinador.");
    }
}

export async function updateMyCoach(request) {
    try {
        const response = await api.put(
            `/api/v1/account/coaches/me`, request
        );

        return response.data;
    } catch (error) {
        handleError(error, "Não foi possível atualizar os detalhes da sua conta treinador.");
    }
}