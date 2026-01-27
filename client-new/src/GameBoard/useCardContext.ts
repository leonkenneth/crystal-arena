import { createContext, useContext } from "react"
import { ExpandedPile } from "./index"


// Context for managing card interaction state
type CardContextType<T> = {
  // Hover/long-press preview (no actions)
  previewCard: T | null
  setPreviewCard: (card: T | null) => void
  // Click/tap selection (with actions menu)
  selectedCard: T | null
  setSelectedCard: (card: T | null) => void
  // Expanded pile view (deck or graveyard)
  expandedPile: ExpandedPile<T> | null
  setExpandedPile: (pile: ExpandedPile<T> | null) => void
}

export const CardContext = createContext<CardContextType<any>>({
  previewCard: null,
  setPreviewCard: () => {},
  selectedCard: null,
  setSelectedCard: () => {},
  expandedPile: null,
  setExpandedPile: () => {},
})

export default function useCardContext() {
  return useContext(CardContext)
}