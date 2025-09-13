import Container from "@/marketing/components/Container";
import { HIGHLIGHTS } from "../data/highlights";

export default function Impact() {
    return (
        <section id="impact" className="relative py-14 overflow-hidden">
            <Container>
                <div className="flex flex-col md:flex-row md:items-stretch md:divide-x md:divide-foreground/10">
                    {HIGHLIGHTS.map(({ icon: Icon, title, desc }) => (
                        <div
                            key={title}
                            className="flex-1 px-0 md:px-8 py-10 md:py-0 first:pt-0 last:pb-0 text-center md:text-left"
                        >
                            <div className="flex justify-center md:justify-start mb-6">
                                <div className="h-16 w-16 rounded-full bg-primary/10 text-primary flex items-center justify-center">
                                    <Icon className="h-7 w-7" />
                                </div>
                            </div>
                            <h3 className="text-xl md:text-2xl font-semibold tracking-tight">{title}</h3>
                            <p className="mt-3 text-base text-foreground/70 leading-relaxed max-w-sm mx-auto md:mx-0">{desc}</p>
                        </div>
                    ))}
                </div>
            </Container>
        </section>
    );
}