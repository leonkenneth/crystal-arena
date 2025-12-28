import Hand from "./HandAndLBDeck/Hand";
import { HandState } from "@/types";

type Props = {
  hand: HandState;
};

export default function OpponentsHand({ hand }: Props) {
  return (
    <Hand cards={hand.cards} showToggleText={false} zoomable={false} />
  );
}