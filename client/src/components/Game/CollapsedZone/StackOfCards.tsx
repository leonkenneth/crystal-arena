import { CardState } from "@/types";
import Card from "../Card";
import EmptyCardSlot from "../Card/EmptyCardSlot";
import { useState } from "react";
import ExpandedZoneDialog from "./ExpandedZoneDialog";

function NoCards() {
  return <EmptyCardSlot />;
}

type Props = {
  cards: CardState[];
  onClick?: () => void;
};

export default function StackOfCards({ cards, onClick }: Props) {
  if (cards.length === 0) {
    return <NoCards />;
  }

  const visibleCard = cards[cards.length - 1];
  return (
    <Card
      card={visibleCard}
      containerProps={{
        isTapped: false,
        isInteractable: true,
        isPlayable: false,
        isSelected: false,
        onClick,
      }}
    />
  );
}
