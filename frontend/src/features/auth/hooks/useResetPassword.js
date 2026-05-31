import { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { resetPassword, login } from "@/features/auth/services/authService";
import { useAuth } from "@/features/auth/hooks/useAuth";
import { getHomeForRole } from "@/shared/routes/getHomeForRole";

export function useResetPassword() {
  const navigate = useNavigate();
  const location = useLocation();
  const { login: applyToken } = useAuth();

  const [submitError, setSubmitError] = useState("");
  const [submitSuccess, setSubmitSuccess] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  async function submit(values) {
    setSubmitError("");
    setSubmitSuccess("");
    setIsSubmitting(true);

    let resetMsg = "";

    try {
      const { message } = await resetPassword({
        email: values.email,
        token: values.token,
        newPassword: values.newPassword,
      });

      resetMsg = message;
    } catch (err) {
      setSubmitError(err?.message || "Não foi possível redefinir agora.");
      setIsSubmitting(false);
      
      return;
    }

    try {
      const response = await login(values.email, values.newPassword);
      await applyToken(response?.token);

      const target =
        location.state?.from ||
        response?.redirectTo ||
        getHomeForRole(response?.role);

      navigate(target, { replace: true });
    } catch (err) {
      setSubmitSuccess(resetMsg);
      setSubmitError(
        err?.message || "Senha redefinida, mas não foi possível entrar automaticamente. Faça login."
      );
    } finally {
      setIsSubmitting(false);
    }
  }

  return { submitError, setSubmitError, submitSuccess, isSubmitting, submit };
}
