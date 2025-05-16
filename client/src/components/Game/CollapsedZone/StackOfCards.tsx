import { CardState } from "@/types";
import Card from "../Card"
import { Card as ChakraCard, Text } from "@chakra-ui/react";
import { CardContainer } from "../Card";

function NoCards() {
    return <CardContainer isPlayable={false} border="dashed">
        <ChakraCard.Body>
            
        </ChakraCard.Body>
    </CardContainer>
}

export default function StackOfCards({ cards }: { cards: CardState[] }) {
    if (cards.length === 0) {
        return <NoCards />
    }
    
    const visibleCard = cards[cards.length - 1];
    return (
        <Card card={visibleCard} />
    );
}