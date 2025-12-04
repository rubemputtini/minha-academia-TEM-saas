import Footer from "@/shared/layout/Footer";
import AppNav from "@/shared/layout/AppNav";

export default function AppLayout({ children }) {
    return (
        <div className="flex min-h-screen flex-col bg-transparent text-foreground">
            <AppNav />

            <main className="flex-1 w-full max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-6 sm:py-8">
                {children}
            </main>

            <Footer />
        </div>
    );
}
