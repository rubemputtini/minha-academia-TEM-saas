import { Link } from "react-router-dom";
import { GraduationCap, Users, Layers } from "lucide-react";
import { Card } from "@/components/ui/card";
import { ROUTES } from "@/shared/routes/routes";
import { cn } from "@/lib/utils";
import { CARD_BASE } from "@/shared/styles/cards";

const LINKS = [
  { label: "Alunos", Icon: GraduationCap, to: ROUTES.coachUsers },
  { label: "Usuários", Icon: Users, to: ROUTES.adminUsers },
  { label: "Equipamentos", Icon: Layers, to: ROUTES.adminBaseEquipments },
];

export function AdminQuickLinksCard() {
  return (
    <Card className={cn(CARD_BASE, "relative overflow-hidden p-4")}>
      <div className="pointer-events-none absolute inset-x-0 top-0 h-px bg-gradient-to-r from-transparent via-primary/40 to-transparent" />
      <p className="mb-3 text-xs font-semibold uppercase tracking-[0.12em] text-muted-foreground/90">
        Atalhos
      </p>
      <div className="grid grid-cols-3 gap-2">
        {LINKS.map(({ label, Icon, to }) => (
          <Link key={to} to={to}>
            <div className="flex flex-col items-center gap-2 rounded-xl border border-white/8 bg-white/[0.03] p-3.5 text-center transition-colors hover:border-white/15 hover:bg-white/[0.06]">
              <Icon className="h-5 w-5 text-muted-foreground/80" />
              <span className="text-xs font-semibold tracking-[0.06em] text-muted-foreground/80">
                {label}
              </span>
            </div>
          </Link>
        ))}
      </div>
    </Card>
  );
}
