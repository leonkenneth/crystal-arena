import { CardOutsideFieldState } from "@/types";
import Card from "../Card";
import { useDraggable } from "@dnd-kit/core";
import { CSS } from "@dnd-kit/utilities";

type Props = {
  card: CardOutsideFieldState;
  isZoomed: boolean;
};

export default function CardInHand({ card, isZoomed }: Props) {
  const { attributes, listeners, setNodeRef, transform } = useDraggable({
    id: card.cardId,
    data: card,
  });
  const style = {
    // Outputs `translate3d(x, y, 0)`
    transform: CSS.Translate.toString(transform),
    zIndex: 1000,
    touchAction: "none",
    pointerEvents: "auto" as React.CSSProperties["pointerEvents"],
  };

  let divProps = {};

  if (isZoomed) {
    divProps = {
      ref: setNodeRef,
      style,
      ...listeners,
      ...attributes,
    }
  }

  return (
    <div {...divProps}>
      <Card card={card} size={isZoomed ? "sm" : "xs"} isInteractable={isZoomed} />
    </div>
  );
}
