import { useState, useEffect } from "react";
import { toast } from "sonner";
import { getCoachClientDetails } from "../services/clientService";

export function useClientDetail(clientId) {
  const [loading, setLoading] = useState(true);
  const [client, setClient] = useState(null);

  useEffect(() => {
    if (!clientId) return;

    async function load() {
      setLoading(true);

      try {
        const data = await getCoachClientDetails(clientId);
        
        if (data) setClient(data);
      } catch (error) {
        toast.error(error?.message || "Erro ao carregar detalhes do aluno.");
      } finally {
        setLoading(false);
      }
    }

    load();
  }, [clientId]);

  return { loading, client };
}
