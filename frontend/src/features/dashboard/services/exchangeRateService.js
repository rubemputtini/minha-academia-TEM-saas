import api from "@/shared/services/api";
import { handleError } from "@/shared/services/handleError";

export async function getExchangeRates(from, to) {
  try {
    const response = await api.get(`/api/v1/exchange-rates?from=${from}&to=${to}`);
    
    return response.data.rates;
  } catch (error) {
    handleError(error, "Não foi possível buscar a cotação de câmbio.");
  }
}
