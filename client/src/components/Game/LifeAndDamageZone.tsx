import { DamageZoneState } from "@/types";
import CollapsedZone from "./CollapsedZone";
import { Container, Text } from "@chakra-ui/react";
import Card from "./Card";
import { CardState } from "@/types";

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
                <Container cursor="pointer">
                {damageZone.cards.map((card : CardState) => (
                    <Card key={card.cardId} card={card} containerProps={{
                        size: "xs",
                        isInteractable: true,
                        isTapped: true,
                    }} />
                ))}
                </Container>
            </CollapsedZone>
        </div>
    );
}