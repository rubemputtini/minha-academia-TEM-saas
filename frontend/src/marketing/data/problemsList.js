import {
    ClockIcon,
    EyeSlashIcon,
    ArrowsRightLeftIcon,
    ArrowPathIcon,
    FaceFrownIcon,
    BoltIcon,
    SparklesIcon,
    CheckBadgeIcon,
    ClipboardDocumentCheckIcon,
    FaceSmileIcon,
} from "@heroicons/react/24/outline";

export const BEFORE = [
    { icon: ClockIcon,                  label: "Perde tempo perguntando o que TEM/NÃO TEM" },
    { icon: EyeSlashIcon,               label: "Ignora equipamentos disponíveis na academia do aluno" },
    { icon: ArrowsRightLeftIcon,        label: "Precisa trocar exercícios toda hora" },
    { icon: ArrowPathIcon,              label: "Repete sempre os mesmos exercícios" },
    { icon: FaceFrownIcon,              label: "Aluno sente improviso e insegurança" },
];

export const AFTER = [
    { icon: BoltIcon,                   label: "Confere equipamentos em segundos" },
    { icon: SparklesIcon,               label: "Aproveita o máximo da academia do aluno" },
    { icon: CheckBadgeIcon,             label: "Prescreve só o que realmente é possível" },
    { icon: ClipboardDocumentCheckIcon, label: "Entrega consultoria personalizada e realista" },
    { icon: FaceSmileIcon,              label: "Aluno percebe previsibilidade e confiança" },
];
