import NavBar from "@/marketing/components/NavBar";
import Hero from "@/marketing/sections/Hero";
import Benefits from "@/marketing/sections/Benefits";
import HowItWorks from "@/marketing/sections/HowItWorks";
import Problem from "@/marketing/sections/Problem";
import Impact from "@/marketing/sections/Impact";
import Preview from "@/marketing/sections/Preview";
import Pricing from "@/marketing/sections/Pricing";
import FAQ from "@/marketing/sections/FAQ";
import Footer from "@/marketing/components/Footer";

export default function LandingPage() {
    return (
        <>
            <NavBar />
            <main>
                <Hero />
                <Problem />
                <Benefits />
                <Impact />
                <HowItWorks />
                <Preview />
                <Pricing />
                <FAQ />
            </main>
            <Footer />
        </>
    );
}
