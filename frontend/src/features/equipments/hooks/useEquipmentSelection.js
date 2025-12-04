import { useCallback, useRef, useState } from "react";
import { saveOwn } from "@/features/equipments/services/equipmentSelectionsService";

export function useEquipmentSelection() {
    const [saving, setSaving] = useState(false);
    const [saveError, setSaveError] = useState("");
    const [confirmOpen, setConfirmOpen] = useState(false);
    const [saveSuccess, setSaveSuccess] = useState(false);

    // ids marcados como "tem"
    const yesSetRef = useRef(new Set());

    const handleChoice = useCallback((item, kind) => {
        switch (kind) {
        case "yes":
            yesSetRef.current.add(item.id);
            break;
        case "no":
        case "undo-yes":
            yesSetRef.current.delete(item.id);
            break;
        case "undo-no":
        default:
            // apenas visual
            break;
        }
    }, []);

    // chamado pelo SwipeDeck quando terminou todos os cards
    const requestSave = useCallback(() => {
        setConfirmOpen(true);
        setSaveError("");
        setSaveSuccess(false);
    }, []);

    const cancelSave = useCallback(() => {
        setConfirmOpen(false);
        setSaveError("");
    }, []);

    const confirmSave = useCallback(async () => {
    setSaving(true);
    setSaveError("");
    setSaveSuccess(false);

    const availableEquipmentIds = Array.from(yesSetRef.current);

    try {
      await saveOwn({ availableEquipmentIds });
      setSaveSuccess(true);
    } catch (err) {
      setSaveError(err?.message);
    } finally {
      setSaving(false);
      setConfirmOpen(false); 
    }
    }, []);

    const resetSuccess = useCallback(() => {
        setSaveSuccess(false);
    }, []);

    return {
        handleChoice,
        requestSave,
        confirmSave,
        cancelSave,
        resetSuccess,
        saving,
        saveError,
        confirmOpen,
        saveSuccess,
    };
}
