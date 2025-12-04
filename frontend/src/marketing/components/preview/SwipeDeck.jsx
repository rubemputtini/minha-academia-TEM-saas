import { useLayoutEffect, useMemo, useRef, useState } from "react";
import SwipeCard from "./SwipeCard";
import VideoModal from "@/shared/components/VideoModal";

export default function SwipeDeck({
    items,
    brandLogoSrc,
    yesIconSrc,
    noIconSrc,
    backIconSrc,
    videoIconSrc,
    infiniteLoop = false,
    timings = { feedbackMs: 500, flyMs: 650, threshold: 110 },
    className = "",
    onChoice,
    onFinish,
}) {
    const { feedbackMs, flyMs, threshold } = timings;

    const [cards, setCards] = useState(items);
    const [decision, setDecision] = useState(null);
    const [leaving, setLeaving] = useState(null);
    const [history, setHistory] = useState([]);
    const liveRegionRef = useRef(null);

    const frameRef = useRef(null);
    const [flyOutX, setFlyOutX] = useState(520);

    const [videoOpen, setVideoOpen] = useState(false);
    const [videoUrl, setVideoUrl] = useState("");

    // reseta deck quando a lista muda
    useLayoutEffect(() => {
        setCards(items);
        setHistory([]);
        setLeaving(null);
        setDecision(null);
    }, [items]);

    // distância de voo dinâmica pra sumir fora da tela (mobile/desktop)
    useLayoutEffect(() => {
        if (!frameRef.current) return;
        const calc = () => {
            const r = frameRef.current?.getBoundingClientRect();
            const base = Math.max(window.innerWidth, r?.width || 520);
            setFlyOutX(Math.round(base * 0.9 + 120));
        };
        calc();
        window.addEventListener("resize", calc);
        return () => window.removeEventListener("resize", calc);
    }, []);

    const topCard = cards[0];
    const tail = useMemo(() => cards.slice(1, 4), [cards]);

    const decide = (kind) => {
        if (!topCard || leaving) return;

        const chosen = topCard;
        const isLastCard = !infiniteLoop && cards.length === 1;

        setDecision({ id: chosen.id, kind });

        if (liveRegionRef.current) {
            liveRegionRef.current.textContent = `${chosen.name}: ${kind === "yes" ? "TEM" : "NÃO TEM"
                }`;
        }

        // notifica quem chamou o componente
        if (typeof onChoice === "function") {
            onChoice(chosen, kind);
        }

        if (isLastCard) {
            // só mostra o feedback visual e depois dispara o onFinish
            window.setTimeout(() => {
                setDecision(null);
                if (typeof onFinish === "function") {
                    onFinish();
                }
            }, feedbackMs);

            return;
        }

        // remove do topo e anima o fantasma
        setLeaving({ item: chosen, kind });
        setCards((prev) => prev.filter((c) => c.id !== chosen.id));

        // limpa highlight depois do feedback
        window.setTimeout(() => setDecision(null), feedbackMs);
    };

    const handleGhostDone = () => {
        if (leaving) {
            setCards((prev) => {
                const next = infiniteLoop
                    ? [...prev, leaving.item] // LandingPage → sempre joga de volta
                    : prev                    // Produto → não devolve (some de vez)

                if (!infiniteLoop && prev.length === 0 && typeof onFinish === "function") {
                    // agenda para depois do repaint
                    setTimeout(() => onFinish(), 0);
                }

                return next;
            });

            setHistory((h) =>
                [{ item: leaving.item, kind: leaving.kind }, ...h].slice(0, 10)
            );
        }
        setLeaving(null);
    };

    const undo = () => {
        if (!history.length || leaving) return;

        const [last, ...rest] = history;
        setHistory(rest);

        // devolve o último card pro topo
        setCards((prev) => {
            const next = prev.filter((c) => c.id !== last.item.id);
            return [last.item, ...next];
        });

        // notifica o caller do "desfazer"
        if (typeof onChoice === "function") {
            const kind = last.kind === "yes" ? "undo-yes" : "undo-no";
            onChoice(last.item, kind);
        }
    };

    const openVideo = () => {
        if (!topCard?.exerciseVideoUrl) return;
        setVideoUrl(topCard.exerciseVideoUrl);
        setVideoOpen(true);
    };

    return (
        <div className={className}>
            <span
                ref={liveRegionRef}
                className="sr-only"
                aria-live="polite"
                aria-atomic="true"
            />

            <div className="pointer-events-none absolute inset-0 -z-10">
                <div className="mx-auto h-56 w-11/12 rounded-[2rem] bg-[radial-gradient(55%_65%_at_50%_0%,rgba(255,255,255,0.07),transparent_70%)] blur-2xl" />
                <div className="absolute inset-0 bg-[linear-gradient(rgba(255,255,255,0.02)_1px,transparent_1px),linear-gradient(90deg,rgba(255,255,255,0.02)_1px,transparent_1px)] bg-[size:18px_18px]" />
            </div>

            <div
                ref={frameRef}
                className="relative z-0 mx-auto h-[28rem] w-full max-w-[22rem] md:max-w-[26rem]"
                style={{ touchAction: leaving ? "none" : "pan-x pan-y" }}
            >
                <div className="absolute inset-0">
                    {tail.map((card, i) => (
                        <SwipeCard
                            key={`back-${card.id}`}
                            item={card}
                            order={i + 1}
                            isTop={false}
                            decision={decision?.id === card.id ? decision.kind : null}
                            onDecide={decide}
                            threshold={threshold}
                            brandLogoSrc={brandLogoSrc}
                            flyMs={flyMs}
                            flyOutX={flyOutX}
                        />
                    ))}

                    {topCard && (
                        <SwipeCard
                            key={`top-${topCard.id}`}
                            item={topCard}
                            order={0}
                            isTop
                            decision={decision?.id === topCard.id ? decision.kind : null}
                            onDecide={decide}
                            threshold={threshold}
                            brandLogoSrc={brandLogoSrc}
                            flyMs={flyMs}
                            flyOutX={flyOutX}
                        />
                    )}

                    {leaving && (
                        <SwipeCard
                            key={`ghost-${leaving.item.id}`}
                            item={leaving.item}
                            order={0}
                            isTop={false}
                            decision={leaving.kind}
                            onDecide={() => { }}
                            onFlyOutEnd={handleGhostDone}
                            threshold={threshold}
                            brandLogoSrc={brandLogoSrc}
                            flyMs={flyMs}
                            flyOutX={flyOutX}
                        />
                    )}
                </div>
            </div>

            <div className="relative z-10 mt-6 w-full">
                <div className="relative mx-auto w-fit">
                    <button
                        type="button"
                        onClick={undo}
                        aria-label="Voltar"
                        title="Voltar"
                        disabled={!history.length || !!leaving}
                        className="absolute top-1/2 -translate-y-1/2 -left-18 grid h-14 w-14 place-items-center
                        rounded-full bg-white ring-1 ring-black/10 shadow-[0_18px_50px_rgba(0,0,0,0.22),0_8px_22px_rgba(0,0,0,0.12)]
                        transition-transform hover:scale-[1.03] active:scale-95 disabled:opacity-40 disabled:grayscale"
                    >
                        <img
                            src={backIconSrc}
                            alt="Voltar"
                            className="h-8 w-8"
                            loading="lazy"
                            decoding="async"
                        />
                    </button>

                    <div className="flex items-center gap-4 rounded-full bg-white px-4 py-2.5
                    ring-1 ring-black/10 shadow-[0_18px_50px_rgba(0,0,0,0.22),0_8px_22px_rgba(0,0,0,0.12)]">
                        <button
                            type="button"
                            onClick={() => topCard && decide("no", topCard.id)}
                            aria-label="Não tem"
                            title="Não tem"
                            className="rounded-full p-1 outline-none focus-visible:ring-2 focus-visible:ring-amber-500/60 group"
                            disabled={!topCard || !!leaving}
                        >
                            <img
                                src={noIconSrc}
                                alt="Não"
                                draggable="false"
                                className="h-12 w-12 transition-transform duration-150 group-hover:scale-105 group-active:scale-95"
                                loading="lazy"
                                decoding="async"
                            />
                        </button>

                        <span className="h-6 w-px bg-neutral-200/90" aria-hidden="true" />

                        <button
                            type="button"
                            onClick={() => topCard && decide("yes", topCard.id)}
                            aria-label="Tem"
                            title="Tem"
                            className="rounded-full p-1 outline-none focus-visible:ring-2 focus-visible:ring-amber-500/60 group"
                            disabled={!topCard || !!leaving}
                        >
                            <img
                                src={yesIconSrc}
                                alt="Sim"
                                draggable="false"
                                className="h-12 w-12 transition-transform duration-150 group-hover:scale-105 group-active:scale-95"
                                loading="lazy"
                                decoding="async"
                            />
                        </button>
                    </div>

                    <button
                        type="button"
                        onClick={openVideo}
                        aria-label="Assistir vídeo"
                        title="Assistir vídeo"
                        disabled={!topCard?.exerciseVideoUrl || !!leaving}
                        className="absolute top-1/2 -translate-y-1/2 -right-18 grid h-14 w-14 place-items-center
                        rounded-full bg-white ring-1 ring-black/10 shadow-[0_18px_50px_rgba(0,0,0,0.22),0_8px_22px_rgba(0,0,0,0.12)]
                        transition-transform hover:scale-[1.03] active:scale-95 disabled:opacity-40 disabled:grayscale"
                    >
                        <img src={videoIconSrc} alt="Assistir vídeo" className="h-8 w-8" loading="lazy" decoding="async" />
                    </button>
                </div>

                <div
                    aria-hidden="true"
                    className="absolute left-1/2 -translate-x-1/2 -z-10 mt-14 h-12 w-[78%] rounded-[999px] blur-2xl"
                    style={{
                        background:
                            "radial-gradient(60% 100% at 50% 50%, rgba(255,207,64,0.24) 0%, rgba(255,207,64,0.0) 70%)",
                    }}
                />
            </div>

            <VideoModal
                open={videoOpen}
                onOpenChange={setVideoOpen}
                url={videoUrl}
                title={topCard?.name || "Vídeo"}
            />
        </div>
    );
}
