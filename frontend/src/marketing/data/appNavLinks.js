import { ROUTES } from "@/shared/routes/routes";
import {
  WrenchScrewdriverIcon,
  CreditCardIcon,
  GiftIcon,
  UsersIcon,
  LifebuoyIcon,
  ArrowRightOnRectangleIcon,
} from "@heroicons/react/24/outline";
import { ROLES } from "@/features/auth/constants/roles";

// Base (todos os usuários logados) – por enquanto não temos nada aqui,
// mas deixamos preparado para futuro (ex: Notificações, Preferências globais)
export const baseLinks = [];

// Links de conta específicos do coach (ficam em "MINHA CONTA")
export const coachAccountLinks = [
  {
    label: "Assinatura",
    to: ROUTES.coachSubscription,
    icon: CreditCardIcon,
    role: ROLES.COACH,
  },
];

// Links da área operacional do coach (ficam em "ÁREA DO TREINADOR")
export const coachAreaLinks = [
  { label: "Alunos", to: ROUTES.users, icon: UsersIcon, role: ROLES.COACH },
  {
    label: "Equipamentos",
    to: ROUTES.equipments,
    icon: WrenchScrewdriverIcon,
    role: ROLES.COACH,
  },
  {
    label: "Indicação",
    to: ROUTES.coachReferral,
    icon: GiftIcon,
    role: ROLES.COACH,
  },
  {
    label: "Suporte",
    to: ROUTES.support,
    icon: LifebuoyIcon,
    role: ROLES.COACH,
  },
];

// Admin-only (área de administração)
export const adminLinks = [
  {
    label: "Usuários",
    to: ROUTES.adminUsers,
    icon: UsersIcon,
    role: ROLES.ADMIN,
  },
  {
    label: "Suporte",
    to: ROUTES.adminSupport,
    icon: LifebuoyIcon,
    role: ROLES.ADMIN,
  },
];

// Footer actions
export const footerActions = [
  {
    label: "Sair",
    to: "#logout",
    icon: ArrowRightOnRectangleIcon,
    action: "logout",
  },
];
