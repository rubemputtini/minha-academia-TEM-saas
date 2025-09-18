import api from "@/shared/services/api";
import { handleError } from "@/shared/services/handleError";
import { clearToken } from "../auth";

export const registerCoach = async (payload) => {
    try {
        const body = {
            name: payload.name,
            email: payload.email,
            phoneNumber: payload.phoneNumber,
            password: payload.password,
            address: {
                street: payload.street,
                number: payload.number,
                complement: payload.complement,
                neighborhood: payload.neighborhood,
                city: payload.city,
                state: payload.state,
                country: payload.country,
                postalCode: payload.postalCode,
                latitude: payload.latitude,
                longitude: payload.longitude,
            }
        };

        const { data } = await api.post("/api/v1/auth/register/coach", body, { timeout: 15000 });

        return data;
    } catch (error) {
        throw handleError(error, "Erro ao registrar treinador.");
    }
};

export const registerCoachAfterPayment = async ({ sessionId, password }) => {
    try {
        const { data } = await api.post(
            "/api/v1/auth/register/coach/after-payment",
            { sessionId, password },
            { timeout: 15000 }
        );

        return data;
    } catch (error) {
        throw handleError(error, "Erro ao finalizar cadastro após pagamento.");
    }
};

export const registerUser = async (payload) => {
    try {
        const body = {
            name: payload.name,
            email: payload.email,
            password: payload.password,
            coachCode: payload.coachCode,
            gymName: payload.gymName,
            gymCity: payload.gymCity,
            gymCountry: payload.gymCountry
        };

        const { data } = await api.post("/api/v1/auth/register/user", body, { timeout: 15000 });

        return data;
    } catch (error) {
        throw handleError(error, "Erro ao registrar aluno.");
    }
};

export const login = async (email, password) => {
    try {
        const data = await api.post("/api/v1/auth/login", { email, password }, { timeout: 15000 });

        return data;
    } catch (error) {
        throw handleError(error, "Não foi possível entrar. Verifique suas credenciais.");
    }
};

export const logout = () => {
    clearToken();
    delete api.defaults.headers.common.Authorization;
};

export const forgotPassword = async (email) => {
    try {
        const data = await api.post("/api/v1/auth/forgot-password", { email });

        return data.message;
    } catch (error) {
        throw handleError(error, "Erro ao solicitar redefinição de senha.");
    }
};

export const resetPassword = async ({ email, token, newPassword }) => {
    try {
        const data = await api.post("/api/v1/auth/reset-password", { email, token, newPassword });

        return data.message;
    } catch (error) {
        throw handleError(error, "Erro ao redefinir nova senha.");
    }
};