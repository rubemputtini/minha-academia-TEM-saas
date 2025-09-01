import NoImg from "@/assets/no.png";
import YesImg from "@/assets/yes.png";

export default function SwipeMockPreview({
    equipmentName = "SUPINO MÁQUINA",
    imageSrc = "/minha-academia-tem-logo.png",
    yesImage = YesImg,
    noImage = NoImg,
    onYes = () => { },
    onNo = () => { },
    className = "",
}) {

    return (
        <div
            className={`relative z-10 w-full max-w-[22rem] ${className}`}
            aria-label="Prévia de marcação de equipamento"
        >
            <div
                aria-hidden="true"
                className="absolute -bottom-16 inset-x-6 h-40 rounded-[999px] blur-[42px] z-0 pointer-events-none"
                style={{
                    background:
                        "radial-gradient(75% 100% at 50% 100%, rgba(255,207,64,0.34) 0%, rgba(255,207,64,0.12) 35%, rgba(0,0,0,0) 70%)",
                }}
            />

            <div className="relative z-10 rounded-[1.3rem] p-[1px] bg-[linear-gradient(180deg,rgba(255,255,255,0.22),rgba(255,255,255,0.06))]">
                <div className="rounded-[1.2rem] overflow-hidden border border-white/10 bg-black/30 backdrop-blur-sm shadow-[0_18px_60px_-20px_rgba(0,0,0,0.65)]">
                    {imageSrc ? (
                        <div className="relative">
                            <div
                                aria-hidden="true"
                                className="pointer-events-none absolute inset-0"
                                style={{
                                    background:
                                        "linear-gradient(to bottom, rgba(255,255,255,0.05), rgba(255,255,255,0) 30%)",
                                }}
                            />
                            <img
                                src={imageSrc}
                                alt={`${equipmentName} — preview`}
                                className="aspect-square w-full object-cover"
                                loading="lazy"
                                decoding="async"
                            />
                        </div>
                    ) : (
                        <div aria-hidden className="aspect-square w-full bg-white/5" />
                    )}

                    <div className="relative z-10 px-4 py-3.5 border-t border-white/10 bg-black/60 backdrop-blur">
                        <h3
                            className="text-center font-semibold tracking-wide text-[14px] leading-none text-white line-clamp-1"
                            title={equipmentName}
                        >
                            {equipmentName}
                        </h3>
                    </div>

                    <div
                        aria-hidden="true"
                        className="pointer-events-none absolute inset-0"
                        style={{
                            background:
                                "radial-gradient(120% 140% at 50% 10%, rgba(0,0,0,0) 0%, rgba(0,0,0,0.05) 55%, rgba(0,0,0,0.16) 100%)",
                        }}
                    />
                </div>
            </div>

            <div className="relative flex justify-center">
                <div className="absolute -bottom-24">
                    <div
                        className="flex items-center gap-4 rounded-full bg-white px-4 py-2.5 ring-1 ring-black/10 shadow-[0_18px_50px_rgba(0,0,0,0.22),0_8px_22px_rgba(0,0,0,0.12)]"
                        aria-label="Ações"
                        role="group"
                    >
                        <button
                            type="button"
                            onClick={onNo}
                            aria-label="Não tem"
                            title="Não tem"
                            className="rounded-full p-1 outline-none focus-visible:ring-2 focus-visible:ring-amber-500/60 group"
                        >
                            <img
                                src={noImage}
                                alt=""
                                draggable="false"
                                className="h-12 w-12 transition-transform duration-150 group-hover:scale-105 group-active:scale-95"
                                loading="lazy"
                                decoding="async"
                            />
                        </button>

                        <span className="h-6 w-px bg-neutral-200/90" aria-hidden="true" />

                        <button
                            type="button"
                            onClick={onYes}
                            aria-label="Tem"
                            title="Tem"
                            className="rounded-full p-1 outline-none focus-visible:ring-2 focus-visible:ring-amber-500/60 group"
                        >
                            <img
                                src={yesImage}
                                alt=""
                                draggable="false"
                                className="h-12 w-12 transition-transform duration-150 group-hover:scale-105 group-active:scale-95"
                                loading="lazy"
                                decoding="async"
                            />
                        </button>
                    </div>

                    <div
                        aria-hidden="true"
                        className="absolute left-1/2 -translate-x-1/2 -z-10 mt-2 h-12 w-[78%] rounded-[999px] blur-2xl"
                        style={{
                            background:
                                "radial-gradient(60% 100% at 50% 50%, rgba(255,207,64,0.24) 0%, rgba(255,207,64,0.0) 70%)",
                        }}
                    />
                </div>
            </div>

            <div className="h-20" aria-hidden="true" />
        </div>
    );
}
