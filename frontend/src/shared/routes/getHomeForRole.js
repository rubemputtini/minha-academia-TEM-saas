import { ROLES } from "@/features/auth/constants/roles";
import { ROUTES } from "./routes";

export function getHomeForRole(role) {
    if (role == ROLES.ADMIN) return ROUTES.dashboardAdmin;
    if (role == ROLES.COACH) return ROUTES.dashboard;

    return ROUTES.app;
};