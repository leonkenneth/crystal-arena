import { EffectState } from "@/types";
import Card from "../Card";
import TargetArrow, { ArrowPortal } from "@/components/ui/TargetArrow";

type StackEffectProps = {
  effect: EffectState;
};

export default function StackEffect({ effect }: StackEffectProps) {
  const card = effect.card;
  const text = effect.text;
  const targetCardIds = effect.targets.filter((t) => t.targetType === "Card").map((c) => c.cardId);

  return (
    <>
      <Card card={card} text={text} size="md" dataCardId={`stackeffect-${card.cardId}`} />
      {targetCardIds.map((id) => {
        return <TargetArrow fromCardId={card.cardId} toCardId={id} key={id} />;
      })}
      {/*<ArrowPortal x1={100} y1={220} x2={130} y2={240} color="red" width={2} curveHeight={0} />*/}
    </>
  );
}
