'use client'

import { createContext, useContext } from "react"


// Drag state for tracking card being dragged
export type DragState<T> = {
  card: T
  startPosition: { x: number; y: number }
  currentPosition: { x: number; y: number }
} | null

// Context for managing card interaction state
type CardContextType<T> = {
  // Hover/long-press preview (no actions)
  previewCard: T | null
  setPreviewCard: (card: T | null) => void
  // Click/tap selection (with actions menu)
  selectedCard: T | null
  setSelectedCard: (card: T | null) => void
  // Drag state
  dragState: DragState<T>
  setDragState: (state: DragState<T>) => void
  // Callback for when drag ends on a zone
  onDragEnd?: (card: T, zone: string | null) => void
  // Track if a card interaction is in progress (to prevent panning)
  isCardInteracting: boolean
  setIsCardInteracting: (value: boolean) => void
}

export const CardContext = createContext<CardContextType<any>>({
  previewCard: null,
  setPreviewCard: () => {},
  selectedCard: null,
  setSelectedCard: () => {},
  dragState: null,
  setDragState: () => {},
  onDragEnd: undefined,
  isCardInteracting: false,
  setIsCardInteracting: () => {},
})

export default function useCardContext() {
  return useContext(CardContext)
}