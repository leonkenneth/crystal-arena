"use client";

import { createContext, useContext } from "react";

// Drag state for tracking card being dragged
export type DragState<T> = {
  card: T;
  startPosition: { x: number; y: number };
  currentPosition: { x: number; y: number };
} | null;

// Context for managing card interaction state
type CardContextType<T> = {
  // Hover/long-press preview (no actions)
  previewCard: T | null;
  setPreviewCard: (card: T | null) => void;
  // Click/tap selection (with actions menu)
  // Drag state
  dragState: DragState<T>;
  setDragState: (state: DragState<T>) => void;
  // Callback for when drag ends on a zone
  onCardClick?: (card: T | null) => void;
  onDragEnd?: (card: T, zone: string | null) => void;
  // Track if a card interaction is in progress (to prevent panning)
  isCardInteracting: boolean;
  setIsCardInteracting: (value: boolean) => void;
  // Track currently hovered drop zone during drag
  hoveredDropZone: string | null;
  setHoveredDropZone: (zone: string | null) => void;
};

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const CardContext = createContext<CardContextType<any>>({
  previewCard: null,
  setPreviewCard: () => {},
  dragState: null,
  setDragState: () => {},
  onCardClick: undefined,
  onDragEnd: undefined,
  isCardInteracting: false,
  setIsCardInteracting: () => {},
  hoveredDropZone: null,
  setHoveredDropZone: () => {},
});

export default function useCardContext() {
  return useContext(CardContext);
}
