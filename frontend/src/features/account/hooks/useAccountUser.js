import { useEffect, useState, useCallback, useMemo } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { toast } from "sonner";

import { userAccountSchema } from "../schemas/userAccountSchema";
import { getMyUser, updateMyUser } from "../services/accountService";

export function useAccountUser() {
    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [isEditing, setIsEditing] = useState(false);

    const form = useForm({
        resolver: zodResolver(userAccountSchema),
        defaultValues: {
        name: "",
        email: "",
        gymName: "",
        gymCity: "",
        gymCountry: "",
        },
        mode: "onBlur",
    });

    const { control, handleSubmit, reset, watch } = form;

    useEffect(() => {
        async function load() {
        setLoading(true);

        try {
            const response = await getMyUser();

            if (response) {
            reset({
                name: response.name,
                email: response.email,
                gymName: response.gymName,
                gymCity: response.gymCity,
                gymCountry: response.gymCountry,
            });
            }
        } catch (error) {
            toast.error(error?.message || "Erro ao carregar seus dados.");
        } finally {
            setLoading(false);
        }
        }

        load();
    }, [reset]);

    const onSubmit = useCallback(
        async (values) => {
        setSaving(true);

        try {
            const response = await updateMyUser(values);

            if (response) {
                reset(response);
            }

            setIsEditing(false);

            toast.success("Informações atualizadas", {
                description: "Seus dados foram salvos com sucesso.",
            });
        } catch (error) {
            toast.error(error?.message || "Erro ao salvar seus dados.");
        } finally {
            setSaving(false);
        }
        },
        [reset]
    );

    const handleCancelEdit = useCallback(() => {
        reset(); // volta ao último estado persistido
        setIsEditing(false);
    }, [reset]);

    const startEditing = useCallback(() => {
        setIsEditing(true);
    }, []);

    const nameValue = watch("name");
    const emailValue = watch("email");

    const fieldEditable = useMemo(
        () => isEditing && !saving,
        [isEditing, saving]
    );

    const inputClass = useCallback(
        (editable) =>
        editable
            ? "bg-background/80 border-primary/40 transition-colors focus-visible:ring-2 focus-visible:ring-primary/60"
            : "bg-background/40 border-border/60 text-muted-foreground cursor-not-allowed",
        []
    );

    const submitHandler = handleSubmit(onSubmit);

    return {
        // estado
        loading,
        saving,
        isEditing,
        fieldEditable,

        // valores derivados
        nameValue,
        emailValue,
        inputClass,

        // ações
        startEditing,
        handleCancelEdit,
        submitHandler,

        // form
        form,
        control,
    };
}
