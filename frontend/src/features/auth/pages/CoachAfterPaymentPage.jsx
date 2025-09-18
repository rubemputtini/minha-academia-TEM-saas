import { useSearchParams } from "react-router-dom";
import CoachRegistration from "../components/signup/CoachRegistration";
import { useCoachSignup } from "../hooks/useCoachSignup";

export default function CoachAfterPaymentPage() {
    const [search] = useSearchParams();
    const sessionId = search.get("session_id");
    const w = useCoachSignup({ sessionId });

    return (
        <CoachRegistration
            {...w}
            onSubmit={w.onSubmit}
        />
    );
}
