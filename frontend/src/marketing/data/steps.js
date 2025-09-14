import { UserPlus, ClipboardList, ClipboardCheck } from "lucide-react";

export const STEPS = [
  {
    id: 1,
    role: "ALUNO",
    Icon: UserPlus,
    title: "Entra com o código do professor",
    description: "Crie sua conta e insira o código para se conectar automaticamente ao professor.",
    hint: "Leva ~1 minuto.",
  },
  {
    id: 2,
    role: "ALUNO",
    Icon: ClipboardList,
    title: "Registra os equipamentos da academia",
    description: "Marque rapidamente o que TEM / NÃO TEM e atualize sempre que houver mudanças.",
    hint: "Fluxo simples e rápido.",
  },
  {
    id: 3,
    role: "TREINADOR",
    Icon: ClipboardCheck,
    title: "Planeja os treinos com base nos dados",
    description: "Veja as respostas do aluno e adapte conforme seu método preferido.",
    hint: "Funciona com qualquer abordagem.",
  },
];
