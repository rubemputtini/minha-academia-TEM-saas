import Container from "@/marketing/components/Container";
import SwipeDeck from "@/marketing/components/preview/SwipeDeck";
import NoImg from "@/assets/no.png";
import YesImg from "@/assets/yes.png";
import { useMemo } from "react";
import { EQUIPMENTS_MOCK } from "../data/equipmentsMock";

export default function Preview({
    id = "preview",
    items = EQUIPMENTS_MOCK,
    brandLogo = "/logo.png",
    yesIcon = YesImg,
    noIcon = NoImg,
}) {
    const deckItems = useMemo(() => items, [items]);

    return (
        <section id={id} className="py-16 md:py-20">
            <Container>
                <div className="mt-2 grid gap-10 md:mt-0 md:grid-cols-2 md:items-start">
                    <header className="max-w-3xl md:max-w-xl md:self-center">
                        <h2 className="text-2xl md:text-3xl font-semibold tracking-tight leading-tight">
                            Parece um app que você já conhece 👀
                        </h2>
                        <p className="mt-3 text-base md:text-lg text-foreground/70">
                            É simples: arraste para direita/esquerda ou use os botões.
                        </p>
                    </header>

                    <div className="relative md:self-start">
                        <SwipeDeck
                            items={deckItems}
                            brandLogoSrc={brandLogo}
                            yesIconSrc={yesIcon}
                            noIconSrc={noIcon}
                            timings={{ feedbackMs: 500, flyMs: 1000, threshold: 110 }}
                            className="relative"
                        />
                    </div>
                </div>
            </Container>
        </section>
    );
}
