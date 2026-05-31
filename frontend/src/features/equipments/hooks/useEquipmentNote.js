import { useEffect, useState } from "react";
import { toast } from "sonner";
import { getMyEquipmentNote, upsertMyEquipmentNote, deleteMyEquipmentNote } from "../services/equipmentNotesService";

export function useEquipmentNote() {
  const [note, setNote] = useState("");
  const [savedNote, setSavedNote] = useState("");
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    async function load() {
      setLoading(true);

      try {
        const data = await getMyEquipmentNote();
        const text = data?.message ?? "";

        setNote(text);
        setSavedNote(text);
      } catch {
        // nota vazia é estado válido
      } finally {
        setLoading(false);
      }
    }

    load();
  }, []);

  const isDirty = note !== savedNote;

  async function save() {
    if (!isDirty || saving) return;

    setSaving(true);

    try {
      await upsertMyEquipmentNote(note.trim());

      setSavedNote(note.trim());
      setNote(note.trim());

      toast.success("Observação salva.");
    } catch (error) {
      toast.error(error?.message || "Erro ao salvar observação.");
    } finally {
      setSaving(false);
    }
  }

  async function deleteNote() {
    setSaving(true);

    try {
      await deleteMyEquipmentNote();

      setSavedNote("");
      setNote("");

      toast.success("Observação apagada.");
    } catch (error) {
      toast.error(error?.message || "Erro ao apagar observação.");
    } finally {
      setSaving(false);
    }
  }

  return { note, setNote, savedNote, loading, saving, isDirty, save, deleteNote };
}
