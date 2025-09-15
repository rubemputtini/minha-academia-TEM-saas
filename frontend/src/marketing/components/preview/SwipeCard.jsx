import { motion as Motion } from "framer-motion";
import { Check, X } from "lucide-react";

export default function SwipeCard({
    item,
    order,
    isTop,
    decision,
    onDecide,
    threshold = 110,
    brandLogoSrc,
    flyMs = 1000,
}) {
    const isDeciding = !!decision;

    const flyTarget =
        decision === "yes"
            ? { x: 520, rotate: 10, opacity: 0 }
            : decision === "no"
                ? { x: -520, rotate: -10, opacity: 0 }
                : { x: 0, rotate: order === 0 ? 0 : order % 2 ? -1.5 : 1.5, opacity: 1 };

    const flyTransition = isDeciding
        ? { type: "tween", duration: flyMs / 1000, ease: [0.16, 1, 0.3, 1] }
        : { type: "spring", stiffness: 240, damping: 22 };

    return (
        <Motion.div
            className="absolute inset-0 mx-auto w-full max-w-[22rem]"
            style={{ zIndex: 10 - order }}
            initial={{ y: order * 8, scale: 1 - order * 0.035 }}
            animate={{ y: order * 8, scale: 1 - order * 0.035 }}
        >
            <Motion.div
                drag={isTop && !isDeciding ? "x" : false}
                dragConstraints={{ left: 0, right: 0 }}
                dragElastic={0.18}
                whileTap={{ scale: 0.995 }}
                onDragEnd={(e, info) => {
                    if (info.offset.x > threshold) onDecide("yes", item.id);
                    else if (info.offset.x < -threshold) onDecide("no", item.id);
                }}
                animate={flyTarget}
                transition={flyTransition}
                className="relative flex h-full flex-col overflow-hidden rounded-2xl bg-black/40 backdrop-blur-sm ring-1 ring-white/10 shadow-[0_18px_60px_-20px_rgba(0,0,0,0.6)]"
            >
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

                <Motion.div
                    aria-hidden
                    className="pointer-events-none absolute inset-0 z-20 rounded-2xl"
                    initial={{ opacity: 0 }}
                    animate={{ opacity: isDeciding ? 1 : 0 }}
                    transition={{ duration: 0.18 }}
                >
                    {/* tinta */}
                    {decision === "yes" && (
                        <div className="absolute inset-0 rounded-2xl bg-emerald-500" />
                    )}
                    {decision === "no" && (
                        <div className="absolute inset-0 rounded-2xl bg-rose-500" />
                    )}

                    {/* halo */}
                    {decision && (
                        <div
                            className={`absolute inset-0 rounded-2xl ring-2 ${decision === "yes" ? "ring-emerald-400/60" : "ring-rose-400/60"
                                } shadow-[0_0_0_10px_rgba(0,0,0,0.0)]`}
                        />
                    )}

                    {/* ícone central preso ao card */}
                    {isDeciding && (
                        <div className="absolute left-1/2 top-1/2 z-[60] -translate-x-1/2 -translate-y-1/2 rounded-full bg-black/10 p-5 md:p-6 backdrop-blur">
                            {decision === "yes" ? (
                                <Check className="h-12 w-12 text-emerald-300" />
                            ) : (
                                <X className="h-12 w-12 text-rose-300" />
                            )}
                        </div>
                    )}
                </Motion.div>
            </Motion.div>
        </Motion.div>
    );
}
