import { ROUTES } from "@/shared/routes/routes";

export const PLANS = [
    {
        id: "free",
        code: 0,
        name: "Free",
        prices: { 
            BRL: "R$ 0", 
            USD: "$ 0", 
            EUR: "€ 0" 
        },
        cadence: "para sempre",
        clients: "1 aluno",
        cta: "Começar grátis",
        route: ROUTES.coachSignup,
        featured: false,
    },
    {
        id: "basic",
        code: 1,
        name: "Basic",
        prices: {
            BRL: "R$ 24,90",
            USD: "€ 7.90",
            EUR: "€ 7,90"
        },
        cadence: "/ mês",
        clients: "Até 5 alunos",
        cta: "Quero o Basic",
        featured: false,
    },
    {
        id: "unlimited",
        code: 2,
        name: "Unlimited",
        prices: {
            BRL: "R$ 39,90",
            USD: "€ 11.90",
            EUR: "€ 11,90"
        },
        cadence: "/ mês",
        clients: "Alunos ilimitados",
        cta: "Quero o Unlimited",
        featured: true,
    },
];

export const FEATURES = [
    { key: "chooseEquip", label: "Escolha quais equipamentos mostrar", plans: ["basic", "unlimited"] },
    { key: "addEquip", label: "Cadastre novos equipamentos", plans: ["unlimited"] },
    { key: "changeChoice", label: "Altere as escolhas dos alunos", plans: ["unlimited"] },
    { key: "mediaEquip", label: "Altere fotos e vídeos dos equipamentos", plans: ["unlimited"] },
];