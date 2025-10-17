import { DamageZoneState } from "@/types";
import CollapsedZone from "./CollapsedZone";
import { HStack, Text } from "@chakra-ui/react";
import Card from "./Card";
import { CardState } from "@/types";

type Props = {
  damageZone: DamageZoneState;
  damageZoneName: string;
};

export default function LifeAndDamageZone({ damageZone, damageZoneName }: Props) {
  return (
    <div>
      <CollapsedZone zone={damageZone} linkOnly name={damageZoneName}>
        <Text color="fg.muted">Damage: {damageZone.cards.length}</Text>

        <HStack cursor="pointer">
          {damageZone.cards.map((card: CardState, index: number) => (
            <Card
              key={card.cardId}
              card={card}
              containerProps={{
                size: "xs",
                isInteractable: true,
                style: {
                  transform: `scale(0.7) translateX(-${index * 2}rem)`,
                },
              }}
            />
          ))}
        </HStack>
      </CollapsedZone>
    </div>
  );
}
