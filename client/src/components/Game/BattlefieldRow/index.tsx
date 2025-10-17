import { BattlefieldRowState, SlotState } from "@/types";
import CardOnField from "./CardOnField";
import { HStack } from "@chakra-ui/react";
import { getCardSize } from "../Card/size";

function BattlefieldSlot({ slot }: { slot: SlotState }) {
  const cards = slot.permanents;

  if (cards.length === 0) return null;
  if (cards.length > 1) throw new Error("Unsupported multiple cards per slot");

  return <CardOnField card={cards[0]} />;
}

type Props = {
  row: BattlefieldRowState;
};

export default function BattlefieldRow({ row }: Props) {
  const { height } = getCardSize("md");
  return (
    <HStack w="full" overflowX="scroll" justifyContent="center" minH={height}>
      {row.slots.map((slot: SlotState, i: number) => (
        <BattlefieldSlot slot={slot} key={i} />
      ))}
    </HStack>
  );
}
