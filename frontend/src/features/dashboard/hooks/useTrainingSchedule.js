import { useState, useEffect } from "react";
import { toast } from "sonner";
import { getTrainingSchedule, updateTrainingDate } from "../services/trainingService";

export function useTrainingSchedule() {
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [schedule, setSchedule] = useState([]);

  useEffect(() => {
    async function load() {
      try {
        const data = await getTrainingSchedule();

        if (data) setSchedule(data.slice().sort(sortByDate));
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
    } finally {
      setSaving(false);
    }
  }

  return { loading, saving, schedule, saveDate };
}

function sortByDate(a, b) {
  if (!a.nextTrainingChangeAt && !b.nextTrainingChangeAt) return 0;
  if (!a.nextTrainingChangeAt) return 1;
  if (!b.nextTrainingChangeAt) return -1;

  return new Date(a.nextTrainingChangeAt) - new Date(b.nextTrainingChangeAt);
}
