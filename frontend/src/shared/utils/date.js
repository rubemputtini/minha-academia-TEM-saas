// Formata DateTime ISO para exibição curta (ex: "16 Jan")
export function fmtDue(isoDate) {
  if (!isoDate) return "—";

  return new Date(isoDate)
    .toLocaleDateString("pt-BR", { day: "2-digit", month: "short" })
    .replace(".", "");
}

// Converte Date local para string ISO sem deslocar timezone
export function toISODateString(date) {
  const d = new Date(date);

  return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, "0")}-${String(d.getDate()).padStart(2, "0")}`;
}

export function daysFromToday(isoDate) {
  const today = new Date();
  today.setHours(0, 0, 0, 0);

  const target = new Date(isoDate);
  target.setHours(0, 0, 0, 0);

  return Math.round((target - today) / (1000 * 60 * 60 * 24));
}
