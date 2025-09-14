import NumberBadge from "./NumberBadge";
import RoleBadge from "./RoleBadge";

export default function StepHeader({ id, role, Icon }) {
    return (
        <div className="flex items-center gap-3">
            <NumberBadge value={id} />
            <RoleBadge role={role} Icon={Icon} />
        </div>
    );
}