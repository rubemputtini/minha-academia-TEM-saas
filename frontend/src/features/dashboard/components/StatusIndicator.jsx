import { getStatus } from "../utils/trainingSchedule.utils";

function getInitials(name) {
  if (!name) return "?";

  const parts = name.trim().split(/\s+/);

  if (parts.length === 1) return parts[0].slice(0, 2).toUpperCase();
  
  return (parts[0][0] + parts[parts.length - 1][0]).toUpperCase();
}

export function StatusIndicator({ nextTrainingChangeAt, name }) {
  const status = getStatus(nextTrainingChangeAt);
  const initials = getInitials(name);

  if (status === "late") {
    return (
      <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full border border-red-400/40 bg-red-500/10 shadow-[0_0_28px_rgba(239,68,68,0.45)]">
        <span className="text-xs font-bold text-red-400">{initials}</span>
      </div>
    );
  }

  if (status === "urgent") {
    return (
      <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full border border-amber-300/40 bg-amber-500/10 shadow-[0_0_28px_rgba(245,197,94,0.45)]">
        <span className="text-xs font-bold text-amber-300">{initials}</span>
      </div>
    );
  }

  if (status === "noDate") {
    return (
      <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full border border-white/6 bg-white/[0.02] opacity-40">
        <span className="text-xs font-bold text-zinc-400">{initials}</span>
      </div>
    );
  }

  return (
    <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full border border-white/8 bg-white/5">
      <span className="text-xs font-bold text-zinc-400/70">{initials}</span>
    </div>
  );
}
