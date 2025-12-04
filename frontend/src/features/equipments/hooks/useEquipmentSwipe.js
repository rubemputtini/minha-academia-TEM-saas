import { useEffect, useState } from "react";
import { getMyUser } from "@/features/account/services/accountService";
import { getActiveByCoach } from "@/features/equipments/services/equipmentService";

export function useEquipmentSwipe() {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [loadError, setLoadError] = useState("");

  useEffect(() => {
    async function load() {
      try {
        setLoadError("");
        setLoading(true);

        const me = await getMyUser();
        const coachId = me?.coachId;

        const list = await getActiveByCoach(coachId);

        const mapped = (list ?? []).map((e) => ({
          id: e.id,
          name: e.name,
          exerciseVideoUrl: e.videoUrl || null,
          src: e.photoUrl || null,
        }));

        setItems(mapped);
      } catch (err) {
        setLoadError(err?.message || "Não foi possível carregar os equipamentos.");
      } finally {
        setLoading(false);
      }
    }

    load();
  }, []);

  return { items, loading, loadError };
}
