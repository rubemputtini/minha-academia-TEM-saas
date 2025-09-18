import { z } from "zod";

const nonEmpty = (msg) => z.string().trim().min(1, msg);

// trata "" como null e converte string→number quando houver valor
const numberOrNull = z.preprocess(
  (v) => (v === "" || v === undefined ? null : v),
  z.union([z.coerce.number(), z.null()])
);

export const coachSignupSchema = z
    .object({
        name: nonEmpty("Informe seu nome completo.").max(100, "Máx. 100 caracteres."),
        email: z.email("E-mail inválido.").max(100, "Máx. 100 caracteres."),
        phoneNumber: nonEmpty("Telefone obrigatório."),

        password: z.string().min(6, "A senha deve ter ao menos 6 caracteres."),
        confirmPassword: z.string().min(1, "Confirme sua senha."),

        street: nonEmpty("Rua obrigatória.").max(100, "Máx. 100 caracteres."),
        number: nonEmpty("Número obrigatório.").max(20, "Máx. 20 caracteres."),
        complement: z.string().trim().max(100, "Máx. 100 caracteres.").optional(),
        neighborhood: nonEmpty("Bairro obrigatório.").max(60, "Máx. 60 caracteres."),
        city: nonEmpty("Cidade obrigatória.").max(60, "Máx. 60 caracteres."),
        state: nonEmpty("Estado obrigatório.").max(50, "Máx. 50 caracteres."),
        country: nonEmpty("País obrigatório.").max(50, "Máx. 50 caracteres."),
        postalCode: nonEmpty("Código postal obrigatório.").max(20, "Máx. 20 caracteres."),

        latitude: numberOrNull.optional().nullable(),
        longitude: numberOrNull.optional().nullable(),
    })
    .refine((data) => data.password === data.confirmPassword, {
      path: ["confirmPassword"],
      message: "As senhas não coincidem.",
});
