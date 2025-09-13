export default function SegmentedTabs({ tab, setTab }) {
    const isAfter = tab === "after";
    return (
        <div
            role="tablist"
            aria-label="Comparativo Antes e Depois"
            className="relative grid h-11 grid-cols-2 place-items-center rounded-full border border-foreground/10 bg-card/60 p-1 text-sm md:hidden"
        >
            <div
                className={[
                    "absolute inset-y-1 left-1 w-[calc(50%-0.25rem)] rounded-full",
                    "bg-foreground/10 transition-transform duration-300 ease-out",
                    isAfter ? "translate-x-[calc(100%+0.5rem)]" : "translate-x-0",
                ].join(" ")}
            />
            <button
                role="tab"
                aria-selected={!isAfter}
                onClick={() => setTab("before")}
                className={[
                    "relative z-10 w-full rounded-full px-3 py-1",
                    !isAfter ? "text-foreground" : "text-foreground/70 hover:text-foreground",
                ].join(" ")}
            >
                Antes
            </button>
            <button
                role="tab"
                aria-selected={isAfter}
                onClick={() => setTab("after")}
                className={[
                    "relative z-10 w-full rounded-full px-3 py-1",
                    isAfter ? "text-primary" : "text-foreground/70 hover:text-foreground",
                ].join(" ")}
            >
                Depois
            </button>
        </div>
    );
}
