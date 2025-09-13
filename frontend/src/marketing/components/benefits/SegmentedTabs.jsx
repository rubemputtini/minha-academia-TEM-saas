import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs";

export default function SegmentedTabs({ tab, setTab }) {
    return (
        <Tabs
            value={tab}
            onValueChange={setTab}
            aria-label="Comparativo Antes e Depois"
            className="md:hidden"
        >
            <TabsList
                className={[
                    "relative grid h-11 grid-cols-2 place-items-center",
                    "rounded-full border border-foreground/10 bg-card/60 p-1 text-sm",
                    "text-foreground"
                ].join(" ")}
            >
                <div
                    className={[
                        "pointer-events-none absolute inset-y-1 left-1 w-[calc(50%-0.25rem)] rounded-full",
                        "bg-foreground/10 transition-transform duration-300 ease-out",
                        tab === "after" ? "translate-x-[100%]" : "translate-x-0",
                    ].join(" ")}
                />

                <TabsTrigger
                    value="before"
                    className={[
                        "relative z-10 w-full rounded-full px-3 py-1",
                        "bg-transparent border-0 shadow-none hover:bg-transparent",
                        "focus-visible:outline-none focus-visible:ring-0 ring-0",
                        "font-normal text-[0.875rem] leading-none",
                        "data-[state=active]:bg-transparent data-[state=inactive]:bg-transparent",
                        "data-[state=active]:shadow-none",
                        "data-[state=active]:text-foreground",
                        "data-[state=inactive]:text-foreground/70 hover:text-foreground",
                    ].join(" ")}
                >
                    Antes
                </TabsTrigger>

                <TabsTrigger
                    value="after"
                    className={[
                        "relative z-10 w-full rounded-full px-3 py-1",
                        "bg-transparent border-0 shadow-none hover:bg-transparent",
                        "focus-visible:outline-none focus-visible:ring-0 ring-0",
                        "font-normal text-[0.875rem] leading-none",
                        "data-[state=active]:bg-transparent data-[state=inactive]:bg-transparent",
                        "data-[state=active]:shadow-none",
                        "data-[state=active]:text-primary",
                        "data-[state=inactive]:text-foreground/70 hover:text-foreground",
                    ].join(" ")}
                >
                    Depois
                </TabsTrigger>
            </TabsList>
        </Tabs>
    );
}
