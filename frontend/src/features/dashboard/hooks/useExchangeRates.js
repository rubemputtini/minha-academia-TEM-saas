import { useState, useEffect } from "react";
import { toast } from "sonner";
import { getExchangeRates } from "../services/exchangeRateService";
import { CURRENCIES, CURRENCY_SYMBOLS, CURRENCY_FLAGS } from "../utils/currencies";

export function useExchangeRates(baseCurrency) {
  const [displayCurrency, setDisplayCurrency] = useState(baseCurrency);
  const [rates, setRates] = useState({});
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setDisplayCurrency(baseCurrency);
  }, [baseCurrency]);

  useEffect(() => {
    if (displayCurrency === baseCurrency) return;
    if (rates[displayCurrency]) return;

    let cancelled = false;
    setLoading(true);

    const targets = CURRENCIES.filter((c) => c !== baseCurrency).join(",");
    getExchangeRates(baseCurrency, targets)
      .then((data) => {
        if (!cancelled && data) setRates({ [baseCurrency]: 1, ...data });
      })
      .catch(() => {
        if (!cancelled) toast.error("Não foi possível buscar a cotação de câmbio.");
      })
      .finally(() => {
        if (!cancelled) setLoading(false);
      });

    return () => { cancelled = true; };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [displayCurrency, baseCurrency]);

  function toggleCurrency() {
    const idx = CURRENCIES.indexOf(displayCurrency);
    setDisplayCurrency(CURRENCIES[(idx + 1) % CURRENCIES.length]);
  }

  const rate = displayCurrency === baseCurrency ? 1 : (rates[displayCurrency] ?? null);

  return {
    displayCurrency,
    rate,
    symbol: CURRENCY_SYMBOLS[displayCurrency] ?? displayCurrency,
    flag: CURRENCY_FLAGS[displayCurrency],
    loading,
    toggleCurrency,
  };
}
