import { ROUTES } from "@/shared/routes/routes";
import {
  Cog6ToothIcon,
  WrenchScrewdriverIcon,
  UsersIcon,
  LifebuoyIcon,
  ArrowRightOnRectangleIcon,
} from "@heroicons/react/24/outline";
import { ROLES } from "@/features/auth/constants/roles";

// Base (todos os usuários logados)
export const baseLinks = [
  { label: "Conta", to: ROUTES.account, icon: Cog6ToothIcon },
  { label: "Equipamentos", to: ROUTES.equipments, icon: WrenchScrewdriverIcon },
];

// Admin-only
export const adminLinks = [
  { label: "Usuários", to: ROUTES.usersAdmin, icon: UsersIcon, role: ROLES.ADMIN },
  { label: "Suporte", to: ROUTES.supportAdmin, icon: LifebuoyIcon, role: ROLES.ADMIN },
];

// Coach-only
export const coachLinks = [
  { label: "Alunos", to: ROUTES.users, icon: UsersIcon, role: ROLES.COACH },
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
