import Container from "@/marketing/components/Container";
import { socialLinks } from "../data/socialLinks";
import InstagramIcon from "./icons/InstagramIcon";

export default function Footer() {
    const year = new Date().getFullYear();

    return (
        <footer className="mt-24">
            <div className="h-px w-full bg-gradient-to-r from-transparent via-foreground/15 to-transparent" />

            <Container className="py-8 md:py-10">
                <div className="flex flex-col items-center justify-between gap-4 md:flex-row">
                    <a href="#" className="inline-flex items-center gap-3">
                        <img src="/logo.png" alt="Minha Academia TEM?" className="h-8 md:h-9 w-auto" />
                    </a>

                    <a
                        href={socialLinks.instagram}
                        target="_blank"
                        rel="noopener noreferrer"
                        aria-label="Instagram"
                        title="@minhaacademiatem"
                        className="inline-flex size-9 items-center justify-center rounded-full ring-1 ring-foreground/20 hover:ring-foreground/35 text-foreground/75 hover:text-foreground transition"
                    >
                        <InstagramIcon className="h-5 w-5" />
                    </a>
                </div>

                <div className="mt-6 text-center md:text-left text-xs text-foreground/60">
                    © {year} Minha Academia TEM? — Todos os direitos reservados.
                </div>
            </Container>
        </footer>
    );
}
