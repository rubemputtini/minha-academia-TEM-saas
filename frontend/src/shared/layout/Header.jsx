import { Link } from "react-router-dom";
import logo from "/logo.png";
import { ROUTES } from "../routes/routes";

export default function Header() {
    return (
        <header className="pt-6 mx-auto container flex justify-center items-center">
            <Link to={ROUTES.home} className="block">
                <img
                    src={logo}
                    alt="Minha Academia TEM? - Logo"
                    className="h-auto w-48 md:w-56 lg:w-64 cursor-pointer block"
                />
            </Link>
        </header>
    );
}