import { useEffect, useState } from "react";
import { toast } from "sonner";
import { getUserView } from "@/features/equipments/services/equipmentSelectionsService";

export function useEquipmentSwipe() {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [loadError, setLoadError] = useState("");
  const [alreadyCompleted, setAlreadyCompleted] = useState(false);

  useEffect(() => {
    async function load() {
      try {
        setLoadError("");
        setLoading(true);

        const list = await getUserView();

        const mapped = (list ?? []).map((e) => ({
          id: e.equipmentId,
          name: e.name,
          exerciseVideoUrl: e.videoUrl || null,
          src: e.photoUrl || null,
        }));

        setItems(mapped);
        setAlreadyCompleted((list ?? []).some((e) => e.isAvailable));
      } catch (err) {
        const msg = err?.message || "Não foi possível carregar os equipamentos.";
        setLoadError(msg);
        toast.error(msg);
      } finally {
        setLoading(false);
      }
    }

    load();
  }, []);

  return { items, loading, loadError, alreadyCompleted };
}
