import { useEffect, useMemo, useState } from "react";
import { toast } from "sonner";
import { getUserView, saveOwn } from "../services/equipmentSelectionsService";
import { useEquipmentFiltering } from "@/shared/hooks/useEquipmentFiltering";

export function useUserEquipments() {
  const [loading, setLoading] = useState(true);
  const [items, setItems] = useState([]);
  const [selected, setSelected] = useState(new Set());
  const [savedSelected, setSavedSelected] = useState(new Set());
  const [saving, setSaving] = useState(false);

  const { filtered, grouped, searchInput, setSearchInput, isSearching } = useEquipmentFiltering(items);

  useEffect(() => {
    async function load() {
      setLoading(true);

      try {
        const data = await getUserView();
        const list = data ?? [];

        setItems(list);
        const ids = new Set(list.filter((e) => e.isAvailable).map((e) => e.equipmentId));
        
        setSelected(ids);
        setSavedSelected(new Set(ids));
      } catch (error) {
        toast.error(error?.message || "Erro ao carregar equipamentos.");
      } finally {
        setLoading(false);
      }
    }

    load();
  }, []);

  const isDirty = useMemo(() => {
    if (selected.size !== savedSelected.size) return true;

    for (const id of selected) {
      if (!savedSelected.has(id)) return true;
    }

    return false;
  }, [selected, savedSelected]);

  const changesCount = useMemo(() => {
    let count = 0;

    for (const id of selected) if (!savedSelected.has(id)) count++;
    for (const id of savedSelected) if (!selected.has(id)) count++;
    
    return count;
  }, [selected, savedSelected]);

  function toggle(equipmentId) {
    setSelected((prev) => {
      const next = new Set(prev);

      if (next.has(equipmentId)) next.delete(equipmentId);
      else next.add(equipmentId);
      
      return next;
    });
  }

  function reset() {
    setSelected(new Set(savedSelected));
  }

  function selectAll() {
    setSelected(new Set(items.map((e) => e.equipmentId)));
  }

  function clearAll() {
    setSelected(new Set());
  }

  async function save() {
    if (!isDirty || saving) return;

    setSaving(true);
    try {
      await saveOwn({ availableEquipmentIds: Array.from(selected) });
      
      setSavedSelected(new Set(selected));
      toast.success("Seleção salva com sucesso.");
    } catch (error) {
      toast.error(error?.message || "Erro ao salvar seleção.");
    } finally {
      setSaving(false);
    }
  }

  return {
    loading,
    items: filtered,
    grouped,
    selected,
    toggle,
    reset,
    selectAll,
    clearAll,
    isDirty,
    changesCount,
    save,
    saving,
    total: items.length,
    selectedCount: selected.size,
    searchInput,
    setSearchInput,
    isSearching,
  };
}
