import { useEffect, useState, useCallback, useMemo } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { toast } from "sonner";

import { coachAccountSchema } from "../schemas/coachAccountSchema";
import { getMyCoach, updateMyCoach } from "../services/accountService";
import { createBillingPortalSession } from "@/features/billing/services/billingService";

export function useAccountCoach() {
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [managingSubscription, setManagingSubscription] = useState(false);

  const form = useForm({
    resolver: zodResolver(coachAccountSchema),
    defaultValues: {
      name: "",
      email: "",
      phoneNumber: "",
      street: "",
      number: "",
      complement: "",
      neighborhood: "",
      city: "",
      state: "",
      country: "",
      postalCode: "",
    },
    mode: "onBlur",
  });

  const { control, handleSubmit, reset, watch } = form;

  const [subscription, setSubscription] = useState({
    status: "",
    plan: "",
    endAt: null,
    summary: null,
  });

  const [coachCode, setCoachCode] = useState("");

  const handleManageSubscription = useCallback(async () => {
    setManagingSubscription(true);

    try {
      const url = await createBillingPortalSession();
      if (url) window.location.href = url;
    } catch (error) {
      toast.error(
        error?.message ||
          "Não foi possível abrir a página de gerenciamento da assinatura."
      );
    } finally {
      setManagingSubscription(false);
    }
  }, []);

  useEffect(() => {
    async function load() {
      setLoading(true);

      try {
        const response = await getMyCoach();

        if (response) {
          reset({
            name: response.name,
            email: response.email,
            phoneNumber: response.phoneNumber,
            street: response.address?.street || "",
            number: response.address?.number || "",
            complement: response.address?.complement || "",
            neighborhood: response.address?.neighborhood || "",
            city: response.address?.city || "",
            state: response.address?.state || "",
            country: response.address?.country || "",
            postalCode: response.address?.postalCode || "",
          });

          setSubscription({
            status: response.subscriptionStatus || "",
            plan: response.subscriptionPlan || "",
            endAt: response.subscriptionEndAt || null,
            summary: response.subscriptionSummary || null,
          });

          setCoachCode(response.coachCode || "");
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
        const payload = {
          name: values.name,
          email: values.email,
          phoneNumber: values.phoneNumber,
          address: {
            street: values.street,
            number: values.number,
            complement: values.complement,
            neighborhood: values.neighborhood,
            city: values.city,
            state: values.state,
            country: values.country,
            postalCode: values.postalCode,
            latitude: null,
            longitude: null,
          },
        };

        const response = await updateMyCoach(payload);

        if (response) {
          reset({
            name: response.name,
            email: response.email,
            phoneNumber: response.phoneNumber,
            street: response.address?.street || "",
            number: response.address?.number || "",
            complement: response.address?.complement || "",
            neighborhood: response.address?.neighborhood || "",
            city: response.address?.city || "",
            state: response.address?.state || "",
            country: response.address?.country || "",
            postalCode: response.address?.postalCode || "",
          });

          setSubscription({
            status: response.subscriptionStatus || "",
            plan: response.subscriptionPlan || "",
            endAt: response.subscriptionEndAt || null,
            summary: response.subscriptionSummary || null,
          });
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
    reset();
    setIsEditing(false);
  }, [reset]);

  const startEditing = useCallback(() => {
    setIsEditing(true);
  }, []);

  const nameValue = watch("name");
  const emailValue = watch("email");

  const isEditable = useMemo(() => isEditing && !saving, [isEditing, saving]);

  const inputClass = useCallback(
    (isEditable) =>
      isEditable
        ? "bg-background/80 border-primary/40 transition-colors focus-visible:ring-2 focus-visible:ring-primary/60"
        : "bg-background/40 border-border/60 text-muted-foreground cursor-not-allowed",
    []
  );

  const submitHandler = handleSubmit(onSubmit);

  return {
    loading,
    saving,
    isEditing,
    isEditable,
    managingSubscription,
    subscription,
    coachCode,
    nameValue,
    emailValue,
    inputClass,
    startEditing,
    handleCancelEdit,
    submitHandler,
    handleManageSubscription,
    form,
    control,
  };
}
