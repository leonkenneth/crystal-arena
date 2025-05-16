import { CardOnFieldState } from "@/types"
import Card from "../Card"

type Props = {
    card: CardOnFieldState
}

export default function CardOnField({ card } : Props) {
    return <Card card={card} />
}