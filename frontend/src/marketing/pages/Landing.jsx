import NavBar from "@/marketing/components/NavBar";
import Hero from "@/marketing/sections/Hero";
// import Benefits from "@/marketing/sections/Benefits";
// import HowItWorks from "@/marketing/sections/HowItWorks";
// import Problem from "@/marketing/sections/Problem";
// import BenefitsList from "@/marketing/sections/BenefitsList";
// import Preview from "@/marketing/sections/Preview";
// import Pricing from "@/marketing/sections/Pricing";
// import FAQ from "@/marketing/sections/FAQ";
import Footer from "@/marketing/components/Footer";

export default function Landing() {
    return (
        <>
            <NavBar />
            <main>
                <Hero />
                {/* <Problem mode="text" />
                <Benefits />
                <BenefitsList />
                <HowItWorks />
                <Preview />
                <Pricing />
                <FAQ /> */}
            </main>
            <Footer />
        </>
    );
}
