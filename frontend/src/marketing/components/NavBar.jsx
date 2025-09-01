import { useState } from "react";
import { Button } from "@/components/ui/button";
import {
    Popover,
    PopoverTrigger,
    PopoverContent,
} from "@/components/ui/popover";
import Container from "@/marketing/components/Container";
import { Bars3Icon, LockClosedIcon } from "@heroicons/react/24/outline";
import { links } from "@/marketing/data/nav-links";
import { ROUTES } from "@/shared/routes";

export default function NavBar() {
    const [open, setOpen] = useState(false);

    return (
        <header className="bg-background">
            <Container className="relative flex h-14 md:h-16 items-center justify-between gap-4">
                <a href={ROUTES.home} aria-label="Home" className="inline-flex">
                    <img src="/logo.png" alt="Minha Academia TEM?" className="h-7 md:h-8 w-auto" />
                </a>

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

                <div className="hidden md:flex items-center">
                    <Button
                        asChild
                        className="rounded-xl bg-white text-black hover:bg-white/90 shadow-sm ring-1 ring-black/10 hover:ring-black/20"
                    >
                        <a href={ROUTES.login} className="inline-flex items-center gap-2">
                            <LockClosedIcon className="h-4 w-4" />
                            Entrar
                        </a>
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

                                <div className="px-3 pb-3 pt-2 border-t border-foreground/10">
                                    <Button
                                        asChild
                                        className="w-full rounded-xl bg-white text-black hover:bg-white/90 shadow-sm ring-1 ring-black/10 hover:ring-black/20"
                                        onClick={() => setOpen(false)}
                                    >
                                        <a href={ROUTES.login} className="inline-flex items-center justify-center gap-2">
                                            <LockClosedIcon className="h-4 w-4" />
                                            Entrar
                                        </a>
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
