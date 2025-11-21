import {
  DndContext,
  DragEndEvent,
  DragOverlay,
  DragStartEvent,
  PointerSensor,
  useSensor,
  useSensors,
} from "@dnd-kit/core";
import Card from "../Card";
import { doAction } from "@/utils/useDoAction";
import { CardState } from "@/types";
import { useLoadedGameContext } from "@/utils/LoadedGameContext";
import { useState } from "react";

type Props = {
  children: React.ReactNode;
  onDraggedCardEnd: (card: CardState) => void;
  onDraggedCardStart: (card: CardState) => void;
};

export default function DragAndDropHandler({
  children,
  onDraggedCardEnd,
  onDraggedCardStart,
}: Props) {
  const { gameState } = useLoadedGameContext();
  const [draggedCard, setDraggedCard] = useState<CardState | null>(null);
  const sensor = useSensor(PointerSensor, {
    // Press delay of 150ms, with tolerance of 5px of movement
    activationConstraint: {
      delay: 150,
      tolerance: 5,
    },
  });
  const sensors = useSensors(sensor);
  const handleDragEnd = (event: DragEndEvent) => {
    const card = event.active.data.current as CardState;
    setDraggedCard(null);
    onDraggedCardEnd(card);
    if (event.over) {
      const zoneId = event.over.id as string;
      const abilityId = card.playableActivations?.find((a) => a.playZone === zoneId)?.abilityId;
      if (abilityId) {
        doAction(gameState.id, card.oid, "ActivateAbilityFromAbilityId", { abilityId });
      }
    }
  };

  const handleDragStart = (event: DragStartEvent) => {
    setDraggedCard(event.active.data.current as CardState);
    onDraggedCardStart(event.active.data.current as CardState);
  };
  return (
    <DndContext onDragEnd={handleDragEnd} onDragStart={handleDragStart} sensors={sensors}>
      {children}
      <DragOverlay style={{ zIndex: 100000000 }}>
        {draggedCard && <Card card={draggedCard} />}
      </DragOverlay>
    </DndContext>
  );
}
