import { daysFromToday, fmtDue } from "@/shared/utils/date";

export function getStatus(isoDate) {
  if (!isoDate) return "noDate";

  const days = daysFromToday(isoDate);

  if (days < 0) return "late";
  if (days <= 7) return "urgent";

  return "normal";
}

export function calcDaysInfo(isoDate) {
  if (!isoDate) {
    return { primary: "Sem data", subtitle: null, colorClass: "text-muted-foreground/30" };
  }

  const days = daysFromToday(isoDate);
  const dateStr = fmtDue(isoDate);

  if (days < 0) {
    const abs = Math.abs(days);
    
    return { primary: "Atrasado", subtitle: `há ${abs} dia${abs !== 1 ? "s" : ""}`, colorClass: "text-red-400" };
  }

  if (days === 0) return { primary: "Hoje", subtitle: dateStr, colorClass: "text-red-400" };
  if (days === 1) return { primary: "Amanhã", subtitle: dateStr, colorClass: "text-amber-400" };
  if (days <= 7) return { primary: `em ${days} dias`, subtitle: dateStr, colorClass: "text-amber-400" };
  if (days <= 14) return { primary: `em ${days} dias`, subtitle: dateStr, colorClass: "text-amber-400/60" };

  return { primary: `em ${days} dias`, subtitle: dateStr, colorClass: "text-muted-foreground/60" };
}
