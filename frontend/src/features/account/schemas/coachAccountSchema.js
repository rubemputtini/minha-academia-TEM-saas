import { z } from "zod";

export const coachAccountSchema = z.object({
  name: z
    .string()
    .min(1, "O nome é obrigatório.")
    .max(80, "O nome deve ter no máximo 80 caracteres."),
  email: z
    .email({ message: "E-mail inválido." })
    .max(100, "Máx. 100 caracteres."),

  phoneNumber: z.string().min(1, "O número de telefone é obrigatório."),
  street: z.string().min(1, "A rua é obrigatória."),
  number: z.string().optional().nullable(),
  complement: z.string().optional().nullable(),
  neighborhood: z.string().min(1, "O bairro é obrigatório."),
  city: z.string().min(1, "A cidade é obrigatória."),
  state: z.string().min(1, "O estado é obrigatório."),
  country: z.string().min(1, "O país é obrigatório."),
  postalCode: z.string().min(1, "O código postal é obrigatório."),
});
