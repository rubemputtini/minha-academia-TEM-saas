export const MUSCLE_GROUPS = [
  { value: 0, label: "Peito",   dot: "bg-blue-400",   badge: "text-blue-300 bg-blue-500/10 border border-blue-400/20",     shimmer: "via-blue-400/25" },
  { value: 1, label: "Ombro",   dot: "bg-violet-400", badge: "text-violet-300 bg-violet-500/10 border border-violet-400/20", shimmer: "via-violet-400/25" },
  { value: 2, label: "Costas",  dot: "bg-emerald-400",badge: "text-emerald-300 bg-emerald-500/10 border border-emerald-400/20", shimmer: "via-emerald-400/25" },
  { value: 3, label: "Pernas",  dot: "bg-amber-400",  badge: "text-amber-300 bg-amber-500/10 border border-amber-400/20",   shimmer: "via-amber-400/25" },
  { value: 4, label: "Bíceps",  dot: "bg-red-400",    badge: "text-red-300 bg-red-500/10 border border-red-400/20",         shimmer: "via-red-400/25" },
  { value: 5, label: "Tríceps", dot: "bg-orange-400", badge: "text-orange-300 bg-orange-500/10 border border-orange-400/20", shimmer: "via-orange-400/25" },
  { value: 6, label: "Abdômen", dot: "bg-cyan-400",   badge: "text-cyan-300 bg-cyan-500/10 border border-cyan-400/20",      shimmer: "via-cyan-400/25" },
];

export const MUSCLE_BY_VALUE = Object.fromEntries(MUSCLE_GROUPS.map((g) => [g.value, g]));
