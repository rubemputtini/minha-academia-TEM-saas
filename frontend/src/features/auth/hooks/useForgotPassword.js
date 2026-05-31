import { useState } from "react";
import { forgotPassword } from "@/features/auth/services/authService";

export function useForgotPassword() {
  const [submitError, setSubmitError] = useState("");
  const [submitSuccess, setSubmitSuccess] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  async function submit({ email }) {
    setSubmitError("");
    setSubmitSuccess("");
    setIsSubmitting(true);

    try {
      const { message } = await forgotPassword(email);
      setSubmitSuccess(message || "Se o e-mail for válido, enviaremos um link para redefinir a senha.");
    } catch (err) {
      setSubmitError(err?.message || "Não foi possível enviar agora.");
    } finally {
      setIsSubmitting(false);
    }
  }

  return { submitError, setSubmitError, submitSuccess, isSubmitting, submit };
}
