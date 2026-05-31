import { ROUTES } from "@/shared/routes/routes";
import {
  WrenchScrewdriverIcon,
  CreditCardIcon,
  GiftIcon,
  UsersIcon,
  LifebuoyIcon,
  ArrowRightOnRectangleIcon,
  ClipboardDocumentListIcon,
} from "@heroicons/react/24/outline";
import { ROLES } from "@/features/auth/constants/roles";

export const baseLinks = [];

export const userAreaLinks = [
  {
    label: "Equipamentos",
    to: ROUTES.equipments,
    icon: ClipboardDocumentListIcon,
    role: ROLES.USER,
  },
];

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
  { label: "Alunos", to: ROUTES.coachUsers, icon: UsersIcon, role: ROLES.COACH },
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
    to: ROUTES.coachSupport,
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
