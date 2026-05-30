import { Link, useLocation } from "react-router-dom";
import { useAuth } from "@/features/auth/hooks/useAuth";
import { getHomeForRole } from "../routes/getHomeForRole";

import {
  HomeIcon,
  ArrowRightOnRectangleIcon,
  UserCircleIcon,
  ChevronDownIcon,
  Cog6ToothIcon,
} from "@heroicons/react/24/outline";

import {
  DropdownMenu,
  DropdownMenuTrigger,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuLabel,
  DropdownMenuGroup,
} from "@/components/ui/dropdown-menu";

import { cn } from "@/lib/utils";
import { ROLES } from "@/features/auth/constants/roles";
import {
  adminLinks,
  baseLinks,
  coachAccountLinks,
  coachAreaLinks,
} from "@/shared/utils/appNavLinks";
import { ROUTES } from "../routes/routes";

function NavIconButton({ as = "button", to, children, className, ...props }) {
  const isLink = as === "link";
  const Comp = isLink ? Link : "button";

  return (
    <Comp
      {...(isLink ? { to } : { type: "button" })}
      className={cn(
        "inline-flex h-10 items-center justify-center gap-1.5 rounded-xl px-2",
        "transition-colors hover:bg-white/[0.06]",
        "focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-white/20",
        className
      )}
      {...props}
    >
      {children}
    </Comp>
  );
}

function getAccountLinkForRole(role) {
  if (role === ROLES.COACH || role === ROLES.ADMIN) {
    return { label: "Conta", to: ROUTES.coachAccount, icon: Cog6ToothIcon };
  }
  return { label: "Conta", to: ROUTES.account, icon: Cog6ToothIcon };
}

const AREA_LABEL_BY_ROLE = {
  [ROLES.ADMIN]: "Administração",
  [ROLES.COACH]: "Área do treinador",
};

export default function AppNav() {
  const { role, logout } = useAuth();
  const location = useLocation();

  const home = getHomeForRole(role);
  const isOnHome = location.pathname === home;

  const isCoach = role === ROLES.COACH;
  const isAdmin = role === ROLES.ADMIN;

  // Links extras que aparecem em "MINHA CONTA"
  const accountExtraLinks = [
    ...baseLinks,
    ...(isCoach || isAdmin ? coachAccountLinks : []),
  ];

    // Links da área específica (Admin / Treinador)
  const areaLinks = isAdmin ? adminLinks : isCoach ? coachAreaLinks : [];
  const hasAreaLinks = areaLinks.length > 0;
  const areaLabel = AREA_LABEL_BY_ROLE[role] ?? "";

  const accountLink = getAccountLinkForRole(role);
  const AccountIcon = accountLink.icon;

  function isActive(to) {
    return location.pathname === to || location.pathname.startsWith(to + "/");
  }

  return (
    <header className="sticky top-0 z-40 h-14 border-b border-white/8 bg-[rgba(12,14,22,0.88)] backdrop-blur-xl">
      <div className="pointer-events-none absolute inset-x-0 bottom-0 h-px bg-gradient-to-r from-transparent via-white/8 to-transparent" />

      <div className="mx-auto flex h-full max-w-6xl items-center justify-between px-3 sm:px-4">
        {/* Home */}
        <NavIconButton as="link" to={home} aria-label="Início" title="Início">
          <HomeIcon
            className={cn(
              "h-6 w-6 transition-colors",
              isOnHome ? "text-primary" : "text-foreground/60 hover:text-primary"
            )}
          />
        </NavIconButton>

        {/* Conta */}
        <DropdownMenu modal={false}>
          <DropdownMenuTrigger asChild>
            <NavIconButton aria-label="Menu da conta" title="Conta">
              <UserCircleIcon className="h-6 w-6 text-foreground/80" />
              <ChevronDownIcon className="h-4 w-4 opacity-70" />
            </NavIconButton>
          </DropdownMenuTrigger>

          <DropdownMenuContent
            align="end"
            sideOffset={10}
            className={cn(
              "relative w-64 max-h-[min(88vh,480px)] overflow-auto rounded-2xl",
              "border border-white/10 bg-[rgba(10,12,20,0.98)] backdrop-blur-xl",
              "shadow-[0_20px_60px_-12px_rgba(0,0,0,0.8)]"
            )}
          >
            <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-primary/30 to-transparent" />

            {/* MINHA CONTA */}
            <DropdownMenuLabel className="px-3 py-2.5 text-[10px] font-semibold uppercase tracking-[0.14em] text-muted-foreground/40">
              Minha conta
            </DropdownMenuLabel>

            <DropdownMenuGroup>
              <DropdownMenuItem asChild key={accountLink.to}>
                <Link
                  to={accountLink.to}
                  className={cn(
                    "cursor-pointer",
                    isActive(accountLink.to) && "text-primary focus:text-primary"
                  )}
                >
                  <AccountIcon className="mr-2 h-4 w-4" />
                  {accountLink.label}
                </Link>
              </DropdownMenuItem>

              {accountExtraLinks.map((link) => {
                const Icon = link.icon;
                return (
                  <DropdownMenuItem asChild key={link.to}>
                    <Link
                      to={link.to}
                      className={cn(
                        "cursor-pointer",
                        isActive(link.to) && "text-primary focus:text-primary"
                      )}
                    >
                      <Icon className="mr-2 h-4 w-4" />
                      {link.label}
                    </Link>
                  </DropdownMenuItem>
                );
              })}
            </DropdownMenuGroup>

            {/* ÁREA DO TREINADOR / ADMINISTRAÇÃO */}
            {hasAreaLinks && (
              <>
                <DropdownMenuSeparator className="bg-white/6" />

                <DropdownMenuLabel className="px-3 py-2.5 text-[10px] font-semibold uppercase tracking-[0.14em] text-muted-foreground/40">
                  {areaLabel}
                </DropdownMenuLabel>

                <DropdownMenuGroup>
                  {areaLinks.map((link) => {
                    const Icon = link.icon;
                    return (
                      <DropdownMenuItem asChild key={link.to}>
                        <Link
                          to={link.to}
                          className={cn(
                            "cursor-pointer",
                            isActive(link.to) && "text-primary focus:text-primary"
                          )}
                        >
                          <Icon className="mr-2 h-4 w-4" />
                          {link.label}
                        </Link>
                      </DropdownMenuItem>
                    );
                  })}
                </DropdownMenuGroup>
              </>
            )}

            <DropdownMenuSeparator className="bg-white/6" />

            <DropdownMenuItem
              onClick={logout}
              className="text-red-400/80 hover:text-red-300 focus:text-red-300"
            >
              <ArrowRightOnRectangleIcon className="mr-2 h-4 w-4" />
              Sair
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      </div>
    </header>
  );
}
