import Container from "@/marketing/components/Container";
import { STEPS } from "../data/steps";
import StepTitle from "../components/howItWorks/StepTitle";
import RoleBadge from "../components/howItWorks/RoleBadge";
import NumberBadge from "../components/howItWorks/NumberBadge";
import StepHeader from "../components/howItWorks/StepHeader";

export default function HowItWorks({ id = "como-funciona" }) {
    return (
        <section id={id} className="py-16 md:py-20">
            <Container>
                <header className="max-w-3xl">
                    <h2 className="text-2xl md:text-3xl font-semibold tracking-tight leading-tight">Como funciona</h2>
                    <p className="mt-2 text-base md:text-lg text-foreground/70 text-pretty">
                        Do cadastro ao treino, simples assim.
                    </p>
                </header>

                <div className="mt-12 hidden md:block">
                    <ol className="mx-auto grid max-w-6xl grid-cols-3 auto-rows-fr gap-10">
                        {STEPS.map((s) => (
                            <li key={s.id} className="h-full flex flex-col">
                                <StepHeader id={s.id} role={s.role} Icon={s.Icon} />

                                <div className="mt-4 flex-1">
                                    <StepTitle>{s.title}</StepTitle>
                                    <p className="mt-3 text-base text-foreground/70 text-pretty">{s.description}</p>
                                    <p className="mt-4 text-sm text-foreground/55 text-pretty">{s.hint}</p>
                                </div>

                                <div className="mt-6 h-px w-2/3 bg-amber-300/25" />
                            </li>
                        ))}
                    </ol>
                </div>

                <div className="relative mt-10 md:hidden">
                    <span aria-hidden className="absolute left-[1.25rem] top-0 bottom-0 w-px bg-amber-300/25" />
                    <ol className="relative space-y-10">
                        {STEPS.map((s) => (
                            <li key={s.id} className="relative pl-12">
                                <NumberBadge value={s.id} size="sm" className="absolute left-0 top-0" />
                                <RoleBadge role={s.role} Icon={s.Icon} compact />

                                <div className="mt-2">
                                    <StepTitle small>{s.title}</StepTitle>
                                </div>

                                <p className="mt-2 text-sm text-foreground/70 text-pretty">{s.description}</p>
                                <p className="mt-3 text-xs text-foreground/55 text-pretty">{s.hint}</p>
                            </li>
                        ))}
                    </ol>
                </div>
            </Container>
        </section>
    );
}
