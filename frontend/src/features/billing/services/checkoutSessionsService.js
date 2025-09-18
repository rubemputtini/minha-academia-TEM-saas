import api from "@/shared/services/api";
import { handleError } from "@/shared/services/handleError";

export async function createSignupCheckout(subscriptionPlan) {
    try {
        const { data } = await api.post(
            "/api/v1/checkout-sessions/signup",
            { subscriptionPlan },
            { timeout: 15000 }
        );
        return data; // { url }
    } catch (error) {
        throw handleError(error, "Não foi possível iniciar o checkout.");
    }
}

export async function createUpgradeCheckout(subscriptionPlan) {
    try {
        const { data } = await api.post(
            "/api/v1/checkout-sessions/upgrade",
            { subscriptionPlan },
            { timeout: 15000 }
        );
        return data; // { url }
    } catch (error) {
        throw handleError(error, "Não foi possível iniciar o upgrade.");
    }
}

export async function getSignupPrefill(sessionId) {
    try {
        const { data } = await api.get(
            `/api/v1/checkout-sessions/${encodeURIComponent(sessionId)}/prefill`,
            { timeout: 15000 }
        );
        return data;
    } catch (error) {
        throw handleError(error, "Não foi possível carregar seus dados do checkout.");
    }
}
