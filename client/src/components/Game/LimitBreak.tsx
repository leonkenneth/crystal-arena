import { LimitBreakState } from "@/types/zones";
import { HStack } from "@chakra-ui/react";
import Card from "./Card";

type Props = {
  limitBreak: LimitBreakState;
};

export default function LimitBreak({ limitBreak }: Props) {
  const cards = limitBreak.cards;
  if (cards.length === 0) {
    return null;
  }
  return <HStack pointerEvents="auto">
    {cards.map((card) => (
      <Card key={card.cardId} card={card} containerProps={{ size: "xs" }} />
    ))}
  </HStack>
  /*
  return (
    <CollapsedZone zone={limitBreak} linkOnly name="LimitBreak">
      <Text color="fg.muted">Limit Break</Text>
    </CollapsedZone>
  );
  */
}