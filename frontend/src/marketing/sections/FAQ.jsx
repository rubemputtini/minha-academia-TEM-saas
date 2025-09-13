import Container from "@/marketing/components/Container";
import {
    Accordion,
    AccordionItem,
    AccordionTrigger,
    AccordionContent,
} from "@/components/ui/accordion";
import { FAQS } from "../data/faqs";

export default function FAQ() {
    const mid = Math.ceil(FAQS.length / 2);
    const left = FAQS.slice(0, mid);
    const right = FAQS.slice(mid);

    return (
        <section id="faq" className="pt-16 section-accent">
            <Container>
                <header className="max-w-2xl">
                    <h2 className="text-2xl md:text-3xl font-semibold tracking-tight">
                        Perguntas frequentes
                    </h2>
                    <p className="mt-2 text-foreground/70">
                        Tudo o que você precisa saber antes de começar.
                    </p>
                </header>

                <div className="mt-10 md:hidden">
                    <Accordion type="multiple">
                        {FAQS.map((item, i) => (
                            <AccordionItem key={`m-${i}`} value={`m-${i}`}>
                                <AccordionTrigger>{item.question}</AccordionTrigger>
                                <AccordionContent>{item.answer}</AccordionContent>
                            </AccordionItem>
                        ))}
                    </Accordion>
                </div>

                <div className="mt-10 hidden md:grid md:grid-cols-2 md:gap-x-14 gap-y-8">
                    <Accordion type="multiple">
                        {left.map((item, i) => (
                            <AccordionItem key={`l-${i}`} value={`l-${i}`}>
                                <AccordionTrigger>{item.question}</AccordionTrigger>
                                <AccordionContent>{item.answer}</AccordionContent>
                            </AccordionItem>
                        ))}
                    </Accordion>

                    <Accordion type="multiple">
                        {right.map((item, i) => (
                            <AccordionItem key={`r-${i}`} value={`r-${i}`}>
                                <AccordionTrigger>{item.question}</AccordionTrigger>
                                <AccordionContent>{item.answer}</AccordionContent>
                            </AccordionItem>
                        ))}
                    </Accordion>
                </div>

                <div className="mt-10 text-center text-sm text-foreground/70">
                    Ainda está com dúvidas?{" "}
                    <a
                        href="mailto:contato@minhaacademiatem.com.br"
                        className="underline underline-offset-4"
                    >
                        Fale com a gente
                    </a>.
                </div>
            </Container>
        </section>
    );
}
