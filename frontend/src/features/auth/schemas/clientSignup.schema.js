import { z } from "zod";

const nonEmpty = (msg) => z.string().trim().min(1, msg);

export const clientSignupSchema = z
    .object({
        name: nonEmpty("Informe seu nome completo.").max(80, "Máx. 80 caracteres."),
        email: z.email("E-mail inválido.").max(100, "Máx. 100 caracteres."),
        
        password: z.string().min(6, "A senha deve ter ao menos 6 caracteres."),
        confirmPassword: z.string().min(1, "Confirme sua senha."),
        
        coachCode: nonEmpty("Informe o código do treinador.").max(100, "Máx. 100 caracteres."),

        gymName: nonEmpty("Informe o nome da academia.").max(100, "Máx. 100 caracteres."),
        gymCity:  nonEmpty("Informe a cidade.").max(100, "Máx. 100 caracteres."),
        gymCountry: nonEmpty("Informe o país.").max(100, "Máx. 100 caracteres."),
    })
    .refine((data) => data.password === data.confirmPassword, {
        path: ["confirmPassword"],
        message: "As senhas não coincidem.",
});
