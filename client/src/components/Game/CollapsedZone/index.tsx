import { ZoneState } from "@/types";
import StackOfCards from "./StackOfCards";

type Props = {
    zone: ZoneState;
    linkOnly?: boolean;
    children?: React.ReactNode;
}

export default function CollapsedZone({ zone, linkOnly, children }: Props) {
    if (linkOnly) {
        return null;
    }

    return (
        <div>
            <StackOfCards cards={zone.cards} />
        </div>
    );
}