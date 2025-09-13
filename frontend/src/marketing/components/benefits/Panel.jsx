import { CheckCircleIcon, XMarkIcon } from "@heroicons/react/24/outline";

export default function Panel({
    title,
    tone = "good",
    items = [],
    iconHeader: Icon,
    hiddenOnMobile,
    highlight,
}) {
    const toneClasses =
        tone === "good"
            ? {
                ring: "ring-emerald-400/25",
                bg: "from-emerald-500/6 to-transparent",
                chipBg: "bg-emerald-500/10",
                chipRing: "ring-emerald-400/25",
                chipText: "text-emerald-300",
                iconWrap: "bg-emerald-500/10 ring-emerald-400/25 text-emerald-300",
            }
            : {
                ring: "ring-rose-400/25",
                bg: "from-rose-500/6 to-transparent",
                chipBg: "bg-rose-500/10",
                chipRing: "ring-rose-400/25",
                chipText: "text-rose-300",
                iconWrap: "bg-rose-500/10 ring-rose-400/25 text-rose-300",
            };

    return (
        <div
            className={[
                "rounded-3xl p-5 md:p-6 ring-1 shadow-[inset_0_1px_0_rgba(255,255,255,0.04)]",
                `bg-gradient-to-b ${toneClasses.bg} ${toneClasses.ring}`,
                hiddenOnMobile ? "hidden md:block" : "block",
                highlight ? "md:scale-[1.01]" : "",
            ].join(" ")}
        >
            <div className="flex items-center gap-3">
                <span className={["inline-flex h-9 w-9 items-center justify-center rounded-xl ring-1", toneClasses.iconWrap].join(" ")}>
                    <Icon className="h-5 w-5" />
                </span>
                <h3 className="text-base font-medium">{title}</h3>
            </div>

            <div className="mt-4 h-1.5 w-32 overflow-hidden rounded-full bg-foreground/10">
                <div
                    className={[
                        "h-full transition-[width] duration-700",
                        tone === "good" ? "bg-gradient-to-r from-emerald-400 to-emerald-300" : "bg-gradient-to-r from-rose-400 to-rose-300",
                    ].join(" ")}
                    style={{ width: tone === "good" ? "100%" : "55%" }}
                />
            </div>

            <ul className="mt-5 divide-y divide-foreground/8 rounded-2xl border border-foreground/10 bg-card/60">
                {items.map(({ icon: RowIcon, label }, idx) => (
                    <li key={idx} className="grid grid-cols-[28px_1fr_auto] items-center gap-3 px-4 py-3">
                        <span className="flex h-7 w-7 items-center justify-center rounded-lg bg-foreground/5 ring-1 ring-foreground/10">
                            <RowIcon className="h-4 w-4 text-foreground/70" />
                        </span>
                        <span className="text-sm leading-relaxed">{label}</span>

                        {tone === "good" ? (
                            <span className={["inline-flex items-center gap-1 rounded-md px-2 py-1 text-xs", toneClasses.chipBg, toneClasses.chipText, "ring-1", toneClasses.chipRing].join(" ")}>
                                <CheckCircleIcon className="h-3.5 w-3.5" />
                                Resolvido
                            </span>
                        ) : (
                            <span className={["inline-flex items-center gap-1 rounded-md px-2 py-1 text-xs", toneClasses.chipBg, toneClasses.chipText, "ring-1", toneClasses.chipRing].join(" ")}>
                                <XMarkIcon className="h-3.5 w-3.5" />
                                Problema
                            </span>
                        )}
                    </li>
                ))}
            </ul>
        </div>
    );
}
