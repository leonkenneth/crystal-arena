import { HandState } from "@/types";
import CardInHand from "./CardInHand";
import { HStack } from "@chakra-ui/react";

type Props = {
    hand: HandState;
    upsideDown?: boolean;
}

export default function Hand({ hand, upsideDown }: Props) {
    const cards = hand.cards;

    if (cards.length === 0) {
        return <span>No card in hand</span>
    }

    return (
        <HStack w="full" justifyContent="center" overflowX="scroll" flexShrink={0} flexGrow={0} gap={2} style={{ scrollSnapType: "x mandatory", transform: `rotateX(${upsideDown ? 180 : 0}deg)` }}>
            {hand.cards.map((card) => {
                return (
                    <div key={card.cardId}>
                    <CardInHand card={card} />
                </div>
            );
        })}
        </HStack>
    );
}