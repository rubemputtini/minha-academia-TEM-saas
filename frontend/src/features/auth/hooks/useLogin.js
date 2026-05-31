import { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { login as loginApi } from "@/features/auth/services/authService";
import { useAuth } from "@/features/auth/hooks/useAuth";
import { getHomeForRole } from "@/shared/routes/getHomeForRole";

export function useLogin() {
  const navigate = useNavigate();
  const location = useLocation();
  const { login: applyToken } = useAuth();

  const [submitError, setSubmitError] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  async function submit({ email, password }) {
    setSubmitError("");
    setIsSubmitting(true);

    try {
      const response = await loginApi(email, password);
      await applyToken(response?.token);

      const target =
        location.state?.from ||
        response?.redirectTo ||
        getHomeForRole(response?.role);

      navigate(target, { replace: true });
    } catch (err) {
      setSubmitError(err?.message || "Não foi possível entrar agora.");
    } finally {
      setIsSubmitting(false);
    }
  }

  return { submitError, setSubmitError, isSubmitting, submit };
}
