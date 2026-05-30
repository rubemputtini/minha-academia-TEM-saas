import { useState, useEffect } from "react";
import { toast } from "sonner";
import { getTrainingSchedule, getTotalClients } from "../services/trainingService";
import { updateTrainingDate } from "@/features/clients/services/clientService";

export function useTrainingSchedule() {
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [schedule, setSchedule] = useState([]);
  const [totalClients, setTotalClients] = useState(0);

  useEffect(() => {
    async function load() {
      try {
        const [data, total] = await Promise.all([
          getTrainingSchedule(),
          getTotalClients(),
        ]);

        if (data) setSchedule(data.slice().sort(sortByDate));
        if (total != null) setTotalClients(total);
      } catch (error) {
        toast.error(error?.message || "Erro ao carregar cronograma.");
      } finally {
        setLoading(false);
      }
    }

    load();
  }, []);

  async function saveDate(userId, date) {
    setSaving(true);
    try {
      await updateTrainingDate(userId, date);

      setSchedule((prev) =>
        prev
          .map((item) =>
            item.userId === userId
              ? { ...item, nextTrainingChangeAt: date }
              : item
          )
          .sort(sortByDate)
      );

      toast.success("Data de troca atualizada.");
    } catch (error) {
      toast.error(error?.message || "Erro ao salvar data.");
      throw error;
    } finally {
      setSaving(false);
    }
  }

  return { loading, saving, schedule, totalClients, saveDate };
}

function sortByDate(a, b) {
  if (!a.nextTrainingChangeAt && !b.nextTrainingChangeAt) return 0;
  if (!a.nextTrainingChangeAt) return 1;
  if (!b.nextTrainingChangeAt) return -1;

  return new Date(a.nextTrainingChangeAt) - new Date(b.nextTrainingChangeAt);
}
