export const CURRENCIES = ["BRL", "EUR", "USD"];
export const CURRENCY_SYMBOLS = { BRL: "R$", USD: "$", EUR: "€" };
export const CURRENCY_FLAGS = { BRL: "🇧🇷", EUR: "🇪🇺", USD: "🇺🇸" };

export function fmtCurrency(value) {
  return new Intl.NumberFormat("pt-BR", {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(value);
}
