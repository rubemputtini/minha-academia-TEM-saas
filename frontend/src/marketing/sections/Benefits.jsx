import { useState } from "react";
import Container from "@/marketing/components/Container";
import { CheckCircleIcon, XMarkIcon } from "@heroicons/react/24/outline";
import { AFTER, BEFORE } from "../data/problemsList";
import SegmentedTabs from "@/marketing/components/benefits/SegmentedTabs";
import Panel from "@/marketing/components/benefits/Panel";

export default function Benefits() {
    const [tab, setTab] = useState("before");

    return (
        <section id="beneficios" className="relative overflow-hidden py-16">
            <div className="pointer-events-none absolute inset-0 -z-10">
                <div className="mx-auto mt-10 h-48 w-11/12 rounded-[3rem] bg-[radial-gradient(60%_60%_at_50%_0%,rgba(255,255,255,0.06),transparent_70%)] blur-2xl" />
            </div>

            <Container>
                <header>
                    <h2 className="text-2xl md:text-3xl font-semibold tracking-tight">
                        Mais tempo para focar no que realmente importa: <span className="text-primary">o ALUNO!</span>
                    </h2>
                    <p className="mt-2 text-foreground/70">
                        Com o Minha Academia TEM?, você troca improviso por previsibilidade — e transforma a experiência de treino.
                    </p>
                </header>

                <div className="mt-6 md:hidden">
                    <SegmentedTabs tab={tab} setTab={setTab} />
                </div>

                <div className="mt-8 grid gap-6 md:grid-cols-2">
                    <Panel
                        title="Antes"
                        tone="bad"
                        items={BEFORE}
                        iconHeader={XMarkIcon}
                        hiddenOnMobile={tab !== "before"}
                    />
                    <Panel
                        title="Depois com Minha Academia TEM?"
                        tone="good"
                        items={AFTER}
                        iconHeader={CheckCircleIcon}
                        hiddenOnMobile={tab !== "after"}
                        highlight
                    />
                </div>
            </Container>
        </section>
    );
}
