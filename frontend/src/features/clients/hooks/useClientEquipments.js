import { useEffect, useState } from "react";
import { toast } from "sonner";
import { getCoachClientDetails } from "../services/clientService";
import { getCoachView } from "@/features/equipments/services/equipmentSelectionsService";
import { getClientEquipmentNote } from "@/features/equipments/services/equipmentNotesService";
import { MUSCLE_GROUPS, MUSCLE_BY_VALUE } from "@/shared/constants/muscleGroups";
import { useEquipmentFiltering } from "@/shared/hooks/useEquipmentFiltering";

export { MUSCLE_GROUPS, MUSCLE_BY_VALUE };

export function useClientEquipments(clientId) {
  const [loading, setLoading] = useState(true);
  const [clientName, setClientName] = useState("");
  const [gymName, setGymName] = useState("");
  const [clientNote, setClientNote] = useState("");
  const [equipments, setEquipments] = useState([]);

  const { filtered, grouped, searchInput, setSearchInput, isSearching } = useEquipmentFiltering(equipments);

  useEffect(() => {
    if (!clientId) return;

    async function load() {
      setLoading(true);

      try {
        const [clientData, equipmentData, noteData] = await Promise.all([
          getCoachClientDetails(clientId),
          getCoachView(clientId),
          getClientEquipmentNote(clientId),
        ]);

        if (clientData) {
          setClientName(clientData.name);
          setGymName(clientData.gymName || "");
        }

        setEquipments(equipmentData ?? []);
        setClientNote(noteData?.message ?? "");
      } catch (error) {
        toast.error(error?.message || "Erro ao carregar equipamentos.");
      } finally {
        setLoading(false);
      }
    }

    load();
  }, [clientId]);

  return {
    loading,
    clientName,
    gymName,
    clientNote,
    total: equipments.length,
    equipments: filtered,
    grouped,
    isSearching,
    searchInput,
    setSearchInput,
  };
}
