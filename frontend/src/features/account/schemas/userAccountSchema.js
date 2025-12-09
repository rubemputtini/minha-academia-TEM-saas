import { z } from "zod";

const nonEmpty = (msg) => z.string().trim().min(1, msg);

export const userAccountSchema = z.object({
    name: nonEmpty("Informe seu nome completo.").max(80, "Máx. 80 caracteres."),
    email: z.email({ message: "E-mail inválido." }).max(100, "Máx. 100 caracteres."),
    gymName: nonEmpty("Informe o nome da academia.").max(100, "Máx. 100 caracteres."),
    gymCity: nonEmpty("Informe a cidade.").max(100, "Máx. 100 caracteres."),
    gymCountry: nonEmpty("Informe o país.").max(100, "Máx. 100 caracteres."),
});
