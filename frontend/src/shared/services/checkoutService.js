import api from "./api";
import { handleError } from "./handleError";

export const createSignupCheckout = async (planCode) => {
  try {
    const { data } = await api.post("/api/v1/checkout-sessions/signup", {
      subscriptionPlan: planCode,
    });
    return data.url;

  } catch (error) {
    throw handleError(error, "Erro ao criar sessão de checkout.");
  }
};

export const createUpgradeCheckout = async (planCode) => {
  try {
    const { data } = await api.post("/api/v1/checkout-sessions/upgrade", {
      subscriptionPlan: planCode,
    });
    return data.url;

  } catch (error) {
    throw handleError(error, "Erro ao criar sessão de upgrade.");
  }
};
