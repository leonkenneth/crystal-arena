import { CardState } from "@/types";
import Card from "../Card"
import EmptyCardSlot from "../Card/EmptyCardSlot";

function NoCards() {
    return <EmptyCardSlot />
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