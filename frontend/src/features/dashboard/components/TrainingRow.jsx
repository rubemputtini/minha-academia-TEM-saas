import { Building2, CalendarDays } from "lucide-react";
import { Button } from "@/components/ui/button";
import { cn } from "@/lib/utils";
import { getStatus, calcDaysInfo } from "@/shared/utils/trainingSchedule.utils";
import { StatusIndicator } from "./StatusIndicator";

const ACTION_CLASSES = {
  late: "border border-red-400/20 bg-red-500/10 text-red-400 hover:bg-red-500/20",
  urgent: "border border-amber-400/20 bg-amber-500/10 text-amber-400 hover:bg-amber-500/20",
  normal: "border border-white/10 text-muted-foreground hover:bg-white/5 hover:text-foreground",
  noDate: "border border-white/10 text-muted-foreground/50 hover:bg-white/5 hover:text-foreground",
};

export function TrainingRow({ item, onEdit }) {
  const status = getStatus(item.nextTrainingChangeAt);
  const info = calcDaysInfo(item.nextTrainingChangeAt);

  return (
    <div className="flex items-center justify-between border-b border-white/5 px-3 py-4 sm:px-6 transition-colors hover:bg-white/[0.03]">
      <div className="flex min-w-0 items-center gap-3">
        <StatusIndicator nextTrainingChangeAt={item.nextTrainingChangeAt} name={item.name} />
        <div className="min-w-0">
          <p className="truncate text-sm font-semibold tracking-tight text-foreground">
            {item.name}
          </p>
          <p className="mt-0.5 flex items-center gap-1.5 text-xs tracking-[0.04em] text-muted-foreground/80">
            <Building2 className="h-3 w-3 shrink-0 opacity-50" />
            <span className="truncate">{item.gymName || "—"}</span>
          </p>
        </div>
      </div>

      <div className="ml-2 flex shrink-0 items-center gap-2 sm:gap-4">
        <div className="hidden text-right sm:block">
          <span className={cn("text-sm font-bold leading-none", info.colorClass)}>
            {info.primary}
          </span>
          {info.subtitle && (
            <p className="mt-1 text-xs text-muted-foreground/60">
              {info.subtitle}
            </p>
          )}
        </div>

        <Button
          size="sm"
          variant="ghost"
          onClick={onEdit}
          className={cn(
            "h-8 rounded-lg px-2 sm:px-3 text-xs font-semibold tracking-[0.06em]",
            ACTION_CLASSES[status]
          )}
        >
          <CalendarDays className={cn("h-3 w-3", !item.nextTrainingChangeAt && "sm:mr-1.5")} />
          {!item.nextTrainingChangeAt && <span className="hidden sm:inline">Definir data</span>}
        </Button>
      </div>
    </div>
  );
}
