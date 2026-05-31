import { MUSCLE_BY_VALUE } from "@/shared/constants/muscleGroups";

export default function EquipmentSectionHeader({ muscleValue, count, selectedCount }) {
  const muscle = MUSCLE_BY_VALUE[muscleValue];
  if (!muscle) return null;

  const hasSelection = selectedCount !== undefined;

  return (
    <div className="flex items-center gap-3 mb-4">
      <span className={`h-6 w-1.5 shrink-0 rounded-full ${muscle.dot}`} />
      <span className="text-base font-bold text-foreground/85">{muscle.label}</span>
      <span className="text-sm text-muted-foreground/40 tabular-nums">
        {hasSelection
          ? `${selectedCount}/${count}`
          : `${count} ${count === 1 ? "equipamento" : "equipamentos"}`}
      </span>
      <div className="flex-1 h-px bg-white/6" />
    </div>
  );
}
