import Container from "@/marketing/components/Container";
import { PLANS } from "../data/plansDescription";
import FeaturedPlanCard from "../components/pricing/FeaturedPlanCard";
import PlanCard from "../components/pricing/PlanCard";
import { useCurrency } from "@/shared/hooks/useCurrency";

export default function Pricing({ id = "precos" }) {
    const currency = useCurrency("EUR");

    return (
        <section id={id} className="py-16 md:py-20">
            <Container>
                <div className="mx-auto max-w-2xl text-center">
                    <h2 className="text-3xl font-bold tracking-tight">Escolha o plano ideal para o seu negócio</h2>
                    <p className="mt-3 text-sm text-white/70">Sem fidelidade. Troque ou cancele quando quiser.</p>
                </div>

                <div className="relative mt-12 px-4 sm:px-6 lg:px-0">
                    <div className="pointer-events-none absolute inset-0 -z-10">
                        <div className="mx-auto h-40 w-11/12 rounded-[2rem] bg-[radial-gradient(55%_65%_at_50%_0%,rgba(255,255,255,0.07),transparent_70%)] blur-2xl" />
                    </div>

                    <div className="grid grid-cols-1 gap-7 sm:grid-cols-2 lg:grid-cols-3">
                        {PLANS.filter((p) => !p.featured).map((plan) => (
                            <PlanCard key={plan.id} plan={plan} currency={currency} />
                        ))}

                        <FeaturedPlanCard currency={currency} />
                    </div>
                </div>
            </Container>
        </section>
    );
}
