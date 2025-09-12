import { Card, CardHeader, CardContent } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import FeatureRow from "./FeatureRow";
import TopBadge from "./TopBadge";
import { FEATURES, PLANS } from "../../data/plansDescription";
import { cn } from "@/lib/utils";
import ClientsChip from "./ClientsChip";
import { useCheckout } from "@/shared/hooks/useCheckout";

export default function FeaturedPlanCard({ currency = "EUR" }) {
    const { loading, go } = useCheckout();

    const plan = PLANS.find((p) => p.featured);
    if (!plan) return null;

    const price = (plan.prices && plan.prices[currency]);

    return (
        <div className="relative rounded-[1.1rem] p-[1.5px] bg-gradient-to-r from-amber-400/80 via-amber-300/60 to-amber-500/80 shadow-[0_0_0_1px_rgba(251,191,36,0.3),0_14px_40px_rgba(251,191,36,0.10)]">
            <Card
                className={cn(
                    "relative overflow-visible rounded-2xl",
                    "bg-[linear-gradient(180deg,rgba(10,12,16,0.9),rgba(10,12,16,0.8)_30%,rgba(10,12,16,0.68)_100%)]",
                    "ring-1 ring-white/10 shadow-[0_1px_0_rgba(255,255,255,0.04),0_22px_50px_rgba(0,0,0,0.55)]"
                )}
            >
                <div className="pointer-events-none absolute inset-0 rounded-2xl bg-amber-300/[0.06]" aria-hidden />

                <TopBadge>Melhor escolha</TopBadge>

                <CardHeader className="p-6 pb-4">
                    <div className="flex items-center justify-between gap-3">
                        <h3 className="text-lg font-semibold tracking-tight">{plan.name}</h3>
                        <ClientsChip text={plan.clients} muted />
                    </div>
                    <div className="mt-3 flex items-end gap-2">
                        <div className="text-4xl font-bold leading-none">{price}</div>
                        <div className="pb-1 text-sm text-white/70">{plan.cadence}</div>
                    </div>
                </CardHeader>

                <CardContent className="p-6 pt-0">
                    <Button
                        size="lg"
                        onClick={() => go(plan.code)}
                        loading={loading}
                        className={cn(
                            "w-full rounded-xl font-semibold transition-colors cursor-pointer",
                            "bg-amber-400 text-black hover:bg-amber-500 active:bg-amber-600",
                            "shadow-sm hover:shadow-md",
                            "focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-amber-300/70"
                        )}
                    >
                        {plan.cta}
                    </Button>

                    <div className="my-6 h-px w-full bg-white/10" />

                    <ul className="space-y-3">
                        {FEATURES.map((f) => (
                            <FeatureRow key={f.key} label={f.label} available={f.plans.includes(plan.id)} />
                        ))}
                    </ul>
                </CardContent>
            </Card>
        </div>
    );
}
