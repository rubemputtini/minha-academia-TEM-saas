import { z } from "zod";

export const resetPasswordSchema = z
    .object({
        email: z.email({ message: "E-mail inválido." }).max(100, "Máx. 100 caracteres."),
        token: z.string().min(1, "Link inválido ou expirado."),
        newPassword: z
            .string()
            .min(6, "Pelo menos 6 caracteres")
            .regex(/[A-Z]/, "Uma letra maiúscula")
            .regex(/[a-z]/, "Uma letra minúscula")
            .regex(/\d/, "Um número")
            .regex(/[^A-Za-z0-9]/, "Um caractere especial"),
        confirmPassword: z.string(),
    }).refine((data) => data.newPassword === data.confirmPassword, {
        path: ["confirmPassword"],
        message: "As senhas não coincidem.",
});