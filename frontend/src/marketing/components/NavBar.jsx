import { useState } from "react";
import { Button } from "@/components/ui/button";
import {
    Popover,
    PopoverTrigger,
    PopoverContent,
} from "@/components/ui/popover";
import Container from "@/marketing/components/Container";
import { AcademicCapIcon, Bars3Icon, LockClosedIcon } from "@heroicons/react/24/outline";
import { links } from "@/marketing/data/navLinks";
import { ROUTES } from "@/shared/routes/routes";
import { Link } from "react-router-dom";

export default function NavBar() {
    const [open, setOpen] = useState(false);

    return (
        <header className="bg-background">
            <Container className="relative flex h-16 items-center justify-between gap-4">
                <Link to={ROUTES.home} aria-label="Home" className="inline-flex">
                    <img src="/logo.png" alt="Minha Academia TEM?" className="h-7 md:h-8 w-auto" />
                </Link>

                <nav className="hidden md:flex items-center gap-2" aria-label="Navegação principal">
                    {links.map((l) => (
                        <a
                            key={l.href}
                            href={l.href}
                            className="px-3 py-2 rounded-xl text-sm text-foreground/80 hover:text-foreground hover:bg-foreground/5 transition"
                        >
                            {l.label}
                        </a>
                    ))}
                </nav>

                <div className="hidden md:flex items-center gap-2">
                    <Button
                        asChild
                        className="rounded-xl"
                    >
                        <Link to={ROUTES.userSignup} className="inline-flex items-center gap-2" aria-label="Sou aluno">
                            <AcademicCapIcon className="h-4 w-4" />
                            Aluno
                        </Link>
                    </Button>

                    <Button
                        asChild
                        variant="inverse"
                        className="rounded-xl"
                    >
                        <Link to={ROUTES.login} className="inline-flex items-center gap-2">
                            <LockClosedIcon className="h-4 w-4" />
                            Treinador
                        </Link>
                    </Button>
                </div>

                <div className="md:hidden">
                    <Popover open={open} onOpenChange={setOpen}>
                        <PopoverTrigger asChild>
                            <button
                                aria-label="Abrir menu"
                                className="inline-flex items-center justify-center rounded-xl p-2 ring-1 ring-foreground/10 hover:bg-foreground/5 transition"
                            >
                                <Bars3Icon className="h-5 w-5" />
                            </button>
                        </PopoverTrigger>

                        <PopoverContent
                            align="end"
                            side="bottom"
                            sideOffset={8}
                            aria-label="Menu de navegação"
                            className="w-[65vw] max-w-[18rem] p-0 rounded-2xl border border-foreground/10 shadow-xl bg-background"
                        >
                            <div className="flex flex-col">
                                <nav className="px-3 py-3 grid gap-2" aria-label="Navegação mobile">
                                    {links.map((l) => (
                                        <a
                                            key={l.href}
                                            href={l.href}
                                            onClick={() => setOpen(false)}
                                            className="rounded-xl px-3 py-2 text-sm bg-foreground/5 hover:bg-foreground/10 transition"
                                        >
                                            {l.label}
                                        </a>
                                    ))}
                                </nav>

                                <div className="px-3 pb-3 pt-2 border-t border-foreground/10 grid gap-2">
                                    <Button
                                        asChild
                                        className="rounded-xl"
                                    >
                                        <Link to={ROUTES.userSignup} className="inline-flex items-center justify-center gap-2" aria-label="Sou aluno">
                                            <AcademicCapIcon className="h-4 w-4" />
                                            Aluno
                                        </Link>
                                    </Button>

                                    <Button
                                        asChild
                                        variant="inverse"
                                        className="rounded-xl"
                                    >
                                        <Link to={ROUTES.login} className="inline-flex items-center justify-center gap-2">
                                            <LockClosedIcon className="h-4 w-4" />
                                            Treinador
                                        </Link>
                                    </Button>
                                </div>
                            </div>
                        </PopoverContent>
                    </Popover>
                </div>
            </Container>
        </header>
    );
}
