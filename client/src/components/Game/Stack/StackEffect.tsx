import { EffectState, ObjectIdContainer } from "@/types";
import Card from "../Card";
import TargetArrow from "@/components/ui/TargetArrow";
import { useLoadedGameContext } from "@/utils/LoadedGameContext";
import { doAction } from "@/utils/useDoAction";

type StackEffectProps = {
  effect: EffectState;
  stackOid: ObjectIdContainer;
  effectIndex: number;
};

export default function StackEffect({ effect, stackOid, effectIndex }: StackEffectProps) {
  const { gameId, refresh } = useLoadedGameContext();
  const card = effect.card;
  const text = effect.text;
  const targetCardIds = effect.targets.filter((t) => t.targetType === "Card").map((c) => c.cardId);

  const handleClick = () => {
    doAction(gameId, stackOid, "SelectEffect", { index: effectIndex });
    refresh();
  };

  return (
    <>
      <Card
        card={card}
        text={text}
        size="md"
        dataCardId={`stackeffect-${card.cardId}`}
        containerProps={{ onClick: handleClick }}
      />
      {targetCardIds.map((id) => {
        return <TargetArrow fromCardId={card.cardId} toCardId={id} key={id} />;
      })}
    </>
  );
}
