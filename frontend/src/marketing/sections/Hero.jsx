import { Button } from "@/components/ui/button";
import Container from "@/marketing/components/Container";
import SwipeMockPreview from "@/marketing/components/SwipeMockPreview";

export default function Hero() {
    return (
        <section className="py-16">
            <Container className="grid items-center gap-12 lg:gap-16 md:grid-cols-2">
                <div className="max-w-xl">
                    <h1 className="text-3xl md:text-5xl font-extrabold leading-tight tracking-tight">
                        Pare de adivinhar os <span className="text-primary">equipamentos</span> da academia do seu aluno.
                    </h1>

                    <p className="mt-4 text-lg text-foreground/80">
                        Poupe tempo no planejamento e elabore treinos personalizados de forma rápida, prática e alinhada com a realidade do aluno.
                    </p>

                    <div className="mt-8">
                        <Button size="lg" asChild className="shadow">
                            <a href="#precos">COMEÇAR AGORA</a>
                        </Button>
                    </div>

                    <p className="mt-4 text-xs text-muted-foreground italic">
                        Sem fidelidade. Cancele quando quiser.
                    </p>
                </div>

                <div className="flex md:justify-end">
                    <SwipeMockPreview
                        equipmentName="SUPINO MÁQUINA"
                        imageSrc="/minha-academia-tem-logo.png"
                    />
                </div>
            </Container>
        </section>
    );
}
