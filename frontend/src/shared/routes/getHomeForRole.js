import { ROUTES } from "./routes";

export function getHomeForRole(role) {
    if (role == "Admin") return ROUTES.dashboardAdmin;
    if (role == "Coach") return ROUTES.dashboard;

    return ROUTES.app;
};