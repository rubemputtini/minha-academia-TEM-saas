import { motion as Motion, useMotionValue, useTransform } from "framer-motion";
import { Check, X } from "lucide-react";

export default function SwipeCard({
    item,
    order,
    isTop,
    decision,
    onDecide,
    onFlyOutEnd,
    threshold = 110,
    brandLogoSrc,
    flyMs = 1000,
    flyOutX = 520,
    dragX,
    bgProgress,
}) {
    const localX = useMotionValue(0);
    const x = isTop && dragX ? dragX : localX;

    // rotation tied to drag position
    const rotate = useTransform(x, [-flyOutX * 0.55, 0, flyOutX * 0.55], [-18, 0, 18]);

    // stamps opacity
    const likeOpacity = useTransform(x, [threshold * 0.15, threshold * 0.65], [0, 1]);
    const nopeOpacity = useTransform(x, [-threshold * 0.65, -threshold * 0.15], [1, 0]);

    // live color tint as you drag (before releasing)
    const likeTint = useTransform(x, [0, threshold * 0.85], [0, 0.45]);
    const nopeTint = useTransform(x, [-threshold * 0.85, 0], [0.45, 0]);

    // background card progressive reveal driven by parent's bgProgress
    const fallbackProgress = useMotionValue(0);
    const progress = bgProgress ?? fallbackProgress;
    const bgRevealY = useTransform(progress, [0, 1], [0, -8]);
    const bgRevealScale = useTransform(progress, [0, 1], [1, 1 + 0.035]);

    const isDeciding = !!decision;
    const isDragging = isTop && !isDeciding;

    const flyTarget = isDeciding
        ? decision === "yes"
            ? { x: flyOutX, rotate: 18, opacity: 0 }
            : { x: -flyOutX, rotate: -18, opacity: 0 }
        : { x: 0, rotate: order === 0 ? 0 : order % 2 ? -1.5 : 1.5, opacity: 1 };

    const flyTransition = isDeciding
        ? { type: "tween", duration: flyMs / 1000, ease: [0.4, 0, 0.8, 0.6] }
        : { type: "spring", stiffness: 240, damping: 22 };

    return (
        <Motion.div
            className="absolute inset-0 mx-auto w-full max-w-[22rem]"
            style={{
                zIndex: 10 - order,
                // background cards subtly rise as the top card is dragged
                y: !isTop ? bgRevealY : undefined,
                scale: !isTop ? bgRevealScale : undefined,
            }}
            initial={{ y: order * 8, scale: 1 - order * 0.035 }}
            animate={{ y: order * 8, scale: 1 - order * 0.035 }}
        >
            <Motion.div
                drag={isDragging ? "x" : false}
                dragConstraints={{ left: 0, right: 0 }}
                dragElastic={0.15}
                dragMomentum={false}
                dragTransition={{ power: 0.2, timeConstant: 120 }}
                style={isDragging ? { x, rotate } : undefined}
                animate={!isDragging ? flyTarget : undefined}
                transition={!isDragging ? flyTransition : undefined}
                whileDrag={{ scale: 1.04 }}
                onDragEnd={(_, info) => {
                    const { offset, velocity } = info;
                    const fastFlick = Math.abs(velocity.x) > 500 && Math.abs(offset.x) > 40;
                    if (offset.x > threshold || (fastFlick && offset.x > 0)) onDecide("yes", item.id);
                    else if (offset.x < -threshold || (fastFlick && offset.x < 0)) onDecide("no", item.id);
                }}
                onAnimationComplete={() => {
                    if (isDeciding && (decision === "yes" || decision === "no")) {
                        onFlyOutEnd?.();
                    }
                }}
                className="relative flex h-full flex-col overflow-hidden rounded-2xl bg-black/40 backdrop-blur-sm ring-1 ring-white/10 shadow-[0_18px_60px_-20px_rgba(0,0,0,0.6)] cursor-grab active:cursor-grabbing"
            >
                {/* LIKE stamp */}
                {isDragging && (
                    <Motion.div
                        style={{ opacity: likeOpacity }}
                        className="pointer-events-none absolute left-4 top-7 z-30 -rotate-[22deg]"
                    >
                        <span className="block rounded-lg border-4 border-emerald-400 bg-emerald-500/15 px-4 py-2 text-3xl font-black uppercase tracking-[0.22em] text-emerald-300 [text-shadow:0_0_20px_rgba(52,211,153,0.7)]">
                            TEM
                        </span>
                    </Motion.div>
                )}

                {/* NOPE stamp */}
                {isDragging && (
                    <Motion.div
                        style={{ opacity: nopeOpacity }}
                        className="pointer-events-none absolute right-4 top-7 z-30 rotate-[22deg]"
                    >
                        <span className="block rounded-lg border-4 border-rose-400 bg-rose-500/15 px-4 py-2 text-3xl font-black uppercase tracking-[0.22em] text-rose-300 [text-shadow:0_0_20px_rgba(251,113,133,0.7)]">
                            NÃO
                        </span>
                    </Motion.div>
                )}

                {/* Live tint overlays while dragging */}
                {isDragging && (
                    <>
                        <Motion.div
                            style={{ opacity: likeTint }}
                            className="pointer-events-none absolute inset-0 z-10 rounded-2xl bg-emerald-500"
                        />
                        <Motion.div
                            style={{ opacity: nopeTint }}
                            className="pointer-events-none absolute inset-0 z-10 rounded-2xl bg-rose-500"
                        />
                    </>
                )}

                <div className="relative flex h-14 items-center justify-center border-b border-white/10 bg-black/55 py-6">
                    <img
                        src={brandLogoSrc}
                        alt="Minha Academia TEM? — logo"
                        className="h-10 w-auto"
                        loading="lazy"
                        decoding="async"
                        draggable="false"
                    />
                    <div aria-hidden className="pointer-events-none absolute inset-x-0 top-0 h-8 bg-[linear-gradient(to_bottom,rgba(255,255,255,0.06),transparent)]" />
                </div>

                <div className="relative flex-1">
                    <div className="relative mx-auto aspect-square w-full overflow-hidden bg-black/30 ring-1 ring-white/5">
                        <img
                            src={item.src}
                            alt={`${item.name} — preview`}
                            className="absolute inset-0 h-full w-full object-contain"
                            loading="lazy"
                            decoding="async"
                            draggable="false"
                        />
                        <div aria-hidden className="pointer-events-none absolute inset-0 bg-[radial-gradient(110%_90%_at_50%_0%,rgba(255,255,255,0.04),transparent_55%)]" />
                    </div>
                </div>

                <div className="relative border-t border-white/10 bg-black/60">
                    <p
                        className="mx-auto flex min-h-[2.6rem] items-center justify-center text-center font-semibold leading-snug text-white/95 [text-wrap:balance]"
                        title={item.name}
                    >
                        {item.name}
                    </p>
                </div>

                {/* Decision overlay (after release) */}
                <Motion.div
                    aria-hidden
                    className="pointer-events-none absolute inset-0 z-20 rounded-2xl"
                    initial={{ opacity: 0 }}
                    animate={{ opacity: isDeciding ? 1 : 0 }}
                    transition={{ duration: 0.12 }}
                >
                    {decision === "yes" && (
                        <div className="absolute inset-0 rounded-2xl bg-gradient-to-br from-emerald-500/80 via-emerald-500/60 to-emerald-600/40" />
                    )}
                    {decision === "no" && (
                        <div className="absolute inset-0 rounded-2xl bg-gradient-to-br from-rose-500/80 via-rose-500/60 to-rose-600/40" />
                    )}
                    {decision && (
                        <div className={`absolute inset-0 rounded-2xl ring-2 ring-inset ${decision === "yes" ? "ring-emerald-300/30" : "ring-rose-300/30"}`} />
                    )}

                    {isDeciding && (
                        <div className="absolute left-1/2 top-1/2 z-[60] -translate-x-1/2 -translate-y-1/2">
                            <Motion.div
                                initial={{ scale: 0.4, opacity: 0 }}
                                animate={{ scale: 1, opacity: 1 }}
                                transition={{ type: "spring", stiffness: 380, damping: 22 }}
                                className="rounded-full bg-white/20 p-5 md:p-6 backdrop-blur-sm ring-2 ring-white/40 shadow-[0_0_40px_rgba(0,0,0,0.3)]"
                            >
                                {decision === "yes" ? (
                                    <Check className="h-14 w-14 text-white drop-shadow-lg" strokeWidth={2.5} />
                                ) : (
                                    <X className="h-14 w-14 text-white drop-shadow-lg" strokeWidth={2.5} />
                                )}
                            </Motion.div>
                        </div>
                    )}
                </Motion.div>
            </Motion.div>
        </Motion.div>
    );
}
