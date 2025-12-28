import { LimitBreakState } from "@/types/zones";
import Hand from "./Hand";

type Props = {
  limitBreak: LimitBreakState;
};

export default function LimitBreak({ limitBreak }: Props) {
  return <Hand cards={limitBreak.cards} />
}
