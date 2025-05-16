import { DamageZoneState } from "@/types";
import CollapsedZone from "./CollapsedZone";
import { Text } from "@chakra-ui/react";
type Props = {
    damageZone: DamageZoneState;
}

export default function LifeAndDamageZone({ damageZone }: Props) {
    return (
        <div>
            <Text color="fg.muted">Damage: {damageZone.cards.length}</Text>
            <CollapsedZone
                zone={damageZone}
                linkOnly
            >
                See DamageZone
            </CollapsedZone>
        </div>
    );
}