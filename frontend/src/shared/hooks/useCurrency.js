import { useEffect, useState } from "react";
import api from "@/shared/services/api";

export function useCurrency(defaultCur = "EUR") {
  const [currency, setCurrency] = useState(defaultCur);

  useEffect(() => {
    const ctrl = new AbortController();

    api.get("/api/v1/geo/currency", { signal: ctrl.signal })
       .then((res) => setCurrency(res.data?.currency || defaultCur))
       .catch(() => {});

    return () => ctrl.abort();
  }, [defaultCur]);

  return currency;
}
