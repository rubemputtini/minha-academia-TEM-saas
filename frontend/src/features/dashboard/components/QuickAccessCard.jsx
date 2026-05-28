import { Link } from "react-router-dom";
import { Users, Settings2 } from "lucide-react";
import { Card } from "@/components/ui/card";
import { ROUTES } from "@/shared/routes/routes";
import { cn } from "@/lib/utils";
import { CARD_BASE } from "@/shared/styles/cards";

const LINKS = [
  { label: "Alunos", Icon: Users, to: ROUTES.coachUsers },
  { label: "Equipamentos", Icon: Settings2, to: ROUTES.coachEquipments },
];

export function QuickAccessCard() {
  return (
    <Card className={cn(CARD_BASE, "p-5")}>
      <p className="mb-3 text-xs font-semibold uppercase tracking-[0.12em] text-muted-foreground/90">
        Atalhos
      </p>
      <div className="grid grid-cols-2 gap-3">
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
