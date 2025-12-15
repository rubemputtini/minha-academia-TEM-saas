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

// Base (todos os usuários logados)
export const baseLinks = [
  { label: "Equipamentos", to: ROUTES.equipments, icon: WrenchScrewdriverIcon },
];

// Admin-only
export const adminLinks = [
  { label: "Usuários", to: ROUTES.adminUsers, icon: UsersIcon, role: ROLES.ADMIN },
  { label: "Suporte", to: ROUTES.adminSupport, icon: LifebuoyIcon, role: ROLES.ADMIN },
];

// Coach-only
export const coachLinks = [
  { label: "Alunos", to: ROUTES.users, icon: UsersIcon, role: ROLES.COACH },
  { label: "Assinatura", to: ROUTES.coachSubscription, icon: CreditCardIcon , role: ROLES.COACH },
  { label: "Indicação", to: ROUTES.coachReferral, icon: GiftIcon, role: ROLES.COACH },
  { label: "Suporte", to: ROUTES.support, icon: LifebuoyIcon, role: ROLES.COACH },
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
