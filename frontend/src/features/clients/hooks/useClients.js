import { useCallback, useEffect, useRef, useState } from "react";
import { toast } from "sonner";
import {
  deleteCoachClient,
  getCoachClients,
  setClientActive,
  updateTrainingDate,
} from "../services/clientService";

const PAGE_SIZE = 10;
const SEARCH_DEBOUNCE_MS = 400;

export function useClients() {
  const [loading, setLoading] = useState(true);
  const [isFetching, setIsFetching] = useState(false);
  const [clients, setClients] = useState([]);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [searchInput, setSearchInput] = useState("");
  const [debouncedSearch, setDebouncedSearch] = useState("");
  const [actionLoadingId, setActionLoadingId] = useState(null);
  const hasLoadedOnce = useRef(false);

  const totalPages = Math.ceil(total / PAGE_SIZE);

  // Debounce: só dispara a API após o usuário parar de digitar
  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedSearch(searchInput);
      setPage(1);
    }, SEARCH_DEBOUNCE_MS);

    return () => clearTimeout(timer);
  }, [searchInput]);

  const load = useCallback(async () => {
    if (!hasLoadedOnce.current) {
      setLoading(true);
    } else {
      setIsFetching(true);
    }
    try {
      const data = await getCoachClients(page, PAGE_SIZE, debouncedSearch);

      if (data) {
        setClients(data.clients);
        setTotal(data.totalClients);
        hasLoadedOnce.current = true;
      }
    } catch (error) {
      toast.error(error?.message || "Erro ao carregar alunos.");
    } finally {
      setLoading(false);
      setIsFetching(false);
    }
  }, [page, debouncedSearch]);

  useEffect(() => {
    load();
  }, [load]);

  async function removeClient(userId) {
    setActionLoadingId(userId);

    try {
      await deleteCoachClient(userId);
      toast.success("Aluno removido com sucesso.");
      await load();
    } catch (error) {
      toast.error(error?.message || "Erro ao remover aluno.");
    } finally {
      setActionLoadingId(null);
    }
  }

  async function toggleActive(userId, currentIsActive) {
    setActionLoadingId(userId);

    try {
      await setClientActive(userId, !currentIsActive);
      setClients((prev) =>
        prev.map((c) =>
          c.id === userId ? { ...c, isActive: !currentIsActive } : c
        )
      );
      toast.success(
        !currentIsActive ? "Aluno ativado com sucesso." : "Aluno desativado com sucesso."
      );
    } catch (error) {
      toast.error(error?.message || "Erro ao alterar status do aluno.");
    } finally {
      setActionLoadingId(null);
    }
  }

  async function saveTrainingDate(userId, date) {
    setActionLoadingId(userId);
    
    try {
      await updateTrainingDate(userId, date || null);
      setClients((prev) =>
        prev.map((c) =>
          c.id === userId ? { ...c, nextTrainingChangeAt: date || null } : c
        )
      );
      toast.success("Data de treino atualizada.");
    } catch (error) {
      toast.error(error?.message || "Erro ao atualizar data de treino.");
      throw error;
    } finally {
      setActionLoadingId(null);
    }
  }

  return {
    loading,
    isFetching,
    clients,
    total,
    page,
    totalPages,
    searchInput,
    debouncedSearch,
    actionLoadingId,
    setPage,
    setSearchInput,
    removeClient,
    toggleActive,
    saveTrainingDate,
  };
}
