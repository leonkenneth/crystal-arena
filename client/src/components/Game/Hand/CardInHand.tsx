import { CardOutsideFieldState } from "@/types";
import Card from "../Card";

type Props = {
  card: CardOutsideFieldState;
};

export default function CardInHand({ card }: Props) {
  return <Card card={card} />;
}
