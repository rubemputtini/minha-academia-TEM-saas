import { CheckCircle2, Circle, ShieldCheck } from "lucide-react";
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover";

const rules = [
    { key: "len", label: "Pelo menos 6 caracteres", test: (s) => (s?.length || 0) >= 6 },
    { key: "up", label: "Uma letra maiúscula", test: (s) => /[A-Z]/.test(s || "") },
    { key: "low", label: "Uma letra minúscula", test: (s) => /[a-z]/.test(s || "") },
    { key: "num", label: "Um número", test: (s) => /\d/.test(s || "") },
    { key: "spc", label: "Um caractere especial", test: (s) => /[^A-Za-z0-9]/.test(s || "") },
];

function strengthMeta(passed, total) {
    const pct = Math.round((passed / total) * 100);

    if (passed <= 2) return { label: "Fraca", pct, bar: "bg-red-500" };
    if (passed <= 4) return { label: "Média", pct, bar: "bg-amber-400" };

    return { label: "Forte", pct, bar: "bg-emerald-500" };
}

export default function PasswordHintPopover({ isOpen, password, children }) {
    const val = password || "";
    const unmet = rules.filter(r => !r.test(val));
    const metCount = rules.length - unmet.length;
    const s = strengthMeta(metCount, rules.length);

    return (
        <Popover open={isOpen}>
            <PopoverTrigger asChild>{children}</PopoverTrigger>

            <PopoverContent
                side="bottom"
                align="start"
                sideOffset={8}
                onOpenAutoFocus={(e) => e.preventDefault()}
                onCloseAutoFocus={(e) => e.preventDefault()}
                className="
          w-72 rounded-xl border border-white/12 bg-popover/95 text-popover-foreground
          shadow-[0_16px_48px_-20px_rgba(0,0,0,0.55)] backdrop-blur-sm
          p-3.5 space-y-3"
            >
                <div className="flex items-center gap-2">
                    <ShieldCheck className="h-4 w-4 text-foreground/75" aria-hidden />
                    <p className="text-sm font-medium">Requisitos da senha</p>
                    <span className="ml-auto text-xs text-foreground/60">{metCount}/{rules.length}</span>
                </div>

                <div className="flex items-center gap-2">
                    <div className="h-1.5 w-full rounded-full bg-white/10 overflow-hidden">
                        <div
                            className={`h-full rounded-full transition-[width] duration-300 ${s.bar}`}
                            style={{ width: `${s.pct}%` }}
                            aria-hidden
                        />
                    </div>
                    <span className="text-[11px] text-foreground/60 min-w-[42px] text-right">{s.label}</span>
                </div>

                {unmet.length === 0 ? (
                    <div className="flex items-center gap-2 text-emerald-300">
                        <CheckCircle2 className="h-4 w-4" aria-hidden />
                        <span className="text-sm">Tudo certo com esta senha.</span>
                    </div>
                ) : (
                    <ul className="space-y-1">
                        {unmet.slice(0, 3).map((r) => (
                            <li key={r.key} className="flex items-center gap-2 text-sm text-foreground/80">
                                <Circle className="h-3.5 w-3.5 text-foreground/50" aria-hidden />
                                {r.label}
                            </li>
                        ))}
                        {unmet.length > 3 && (
                            <li className="pl-5 text-[12px] text-foreground/50">
                                …e mais {unmet.length - 3} requisito(s).
                            </li>
                        )}
                    </ul>
                )}
            </PopoverContent>
        </Popover>
    );
}
