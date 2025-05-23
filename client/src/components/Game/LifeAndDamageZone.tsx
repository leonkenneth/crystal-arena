import { DamageZoneState } from "@/types";
import CollapsedZone from "./CollapsedZone";
import { Text } from "@chakra-ui/react";
type Props = {
    damageZone: DamageZoneState;
    damageZoneName: string;
}

export default function LifeAndDamageZone({ damageZone, damageZoneName }: Props) {
    return (
        <div>
            <Text color="fg.muted">Damage: {damageZone.cards.length}</Text>
            <CollapsedZone
                zone={damageZone}
                linkOnly
                name={damageZoneName}
            >
                See DamageZone
            </CollapsedZone>
        </div>
    );
}