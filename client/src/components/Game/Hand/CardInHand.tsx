import { CardOutsideFieldState } from "@/types";
import Card from "../Card";
import { useDraggable } from "@dnd-kit/core";
import { CSS } from "@dnd-kit/utilities";

type Props = {
  card: CardOutsideFieldState;
};

export default function CardInHand({ card }: Props) {
  const { attributes, listeners, setNodeRef, transform } = useDraggable({
    id: card.cardId,
    data: card,
  });
  const style = {
    // Outputs `translate3d(x, y, 0)`
    transform: CSS.Translate.toString(transform),
    zIndex: 1000,
    touchAction: "none",
    pointerEvents: ("auto" as React.CSSProperties["pointerEvents"])
  };

  return (
    <div ref={setNodeRef} style={style} {...listeners} {...attributes}>
      <Card card={card} />
    </div>
  );
}
