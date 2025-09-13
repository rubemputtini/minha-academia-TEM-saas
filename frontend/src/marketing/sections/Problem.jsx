import { useRef, useEffect } from "react";
import { motion as Motion, useInView, useAnimationControls, useReducedMotion } from "framer-motion";

import Container from "@/marketing/components/Container";
import WhatsAppMessage from "@/marketing/components/WhatsAppMessage";
import { clientMessages } from "@/marketing/data/clientMessages";

const container = {
    hidden: {},
    show: { transition: { staggerChildren: 1 } },
};

const item = (i) => ({
    hidden: { opacity: 0, y: 18, rotate: (i % 3) - 1 },
    show: {
        opacity: 1, y: 0, rotate: (i % 3) - 1,
        transition: { type: "spring", stiffness: 220, damping: 24 },
    },
});

const colOffset = (i) => {
    const col = i % 4;
    return col === 1 ? "md:translate-y-4 lg:translate-y-6"
        : col === 2 ? "md:-translate-y-2 lg:-translate-y-3"
            : col === 3 ? "md:translate-y-3 lg:translate-y-4"
                : "";
};

export default function Problem() {
    const secRef = useRef(null);

    const inView = useInView(secRef, { once: false, amount: 0.3, margin: "0px 0px -20% 0px" });
    const controls = useAnimationControls();
    const prefersReduced = useReducedMotion();

    useEffect(() => {
        if (prefersReduced) return;
        if (inView) controls.start("show");
        else controls.start("hidden");
    }, [inView, controls, prefersReduced]);

    return (
        <section id="problema" ref={secRef} className="py-10">
            <Container>
                <header className="max-w-2xl mx-auto md:mx-0">
                    <h2 className="text-2xl md:text-3xl font-semibold tracking-tight">
                        Quantas vezes você já ouviu isso dos alunos?
                    </h2>
                    <p className="mt-2 text-foreground/70">
                        Sem informações sobre a estrutura da academia, você acaba refazendo treinos à toa.
                    </p>
                </header>

                <div className="relative mt-12">
                    <div className="pointer-events-none absolute inset-x-0 -top-8 -z-10 flex justify-center">
                        <div className="h-36 w-[92%] max-w-[1100px] rounded-[2rem] bg-[radial-gradient(55%_65%_at_50%_0%,rgba(255,255,255,0.07),transparent_70%)] blur-2xl" />
                    </div>

                    <Motion.div
                        variants={container}
                        initial="hidden"
                        animate={controls}
                        className="mx-auto max-w-[1200px] grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-x-6 gap-y-7 justify-items-center md:justify-items-stretch"
                    >
                        {clientMessages.map((msg, i) => (
                            <Motion.div key={i} variants={item(i)} className={["will-change-transform", colOffset(i)].join(" ")}>
                                <div className="transition-transform duration-300 ease-out hover:-translate-y-0.5 hover:drop-shadow-[0_6px_22px_rgba(0,0,0,0.18)] flex justify-center">
                                    <WhatsAppMessage text={msg.text} time={msg.time} align="center" className="md:!self-auto" />
                                </div>
                            </Motion.div>
                        ))}
                    </Motion.div>
                </div>
            </Container>
        </section>
    );
}
