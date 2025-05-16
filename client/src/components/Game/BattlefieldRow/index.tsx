import { BattlefieldRowState, SlotState } from "@/types";
import CardOnField from "./CardOnField";
import { Card as ChakraCard, HStack } from "@chakra-ui/react";
import { CardContainer } from "../Card";

function EmptySlot() {
    return <CardContainer isPlayable={false} border="dashed">
        <ChakraCard.Body>
            
        </ChakraCard.Body>
    </CardContainer>
}

function BattlefieldSlot({ slot } : { slot: SlotState }) {
    const cards = slot.permanents;

    if (cards.length === 0) return <EmptySlot />;
    if (cards.length > 1) throw new Error("Unsupported multiple cards per slot");

    return <CardOnField card={cards[0]} />
}

type Props = {
    row: BattlefieldRowState;
}

export default function BattlefieldRow({ row }: Props) {
    return (
        <HStack w="full" overflowX="scroll" justifyContent="center">
            {row.slots.map((slot, i) => <BattlefieldSlot slot={slot} key={i} />)}
        </HStack>
    );
}