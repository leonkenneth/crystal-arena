import { useCallback, useRef } from "react"
import { ThreeEvent } from "@react-three/fiber"
import useCardContext from "./useCardContext"


// Long press duration in ms
const LONG_PRESS_DURATION = 400

// Hook for handling hover, long-press, and click
export default function useCardInteraction<T>(cardData: T | null) {
  const { setPreviewCard, setSelectedCard, selectedCard } = useCardContext()
  const longPressTimer = useRef<ReturnType<typeof setTimeout> | null>(null)
  const isLongPress = useRef(false)
  const pointerDownTime = useRef(0)

  const handlePointerEnter = useCallback(() => {
    // Only show preview if no card is currently selected
    if (cardData && !selectedCard) {
      setPreviewCard(cardData)
    }
  }, [cardData, setPreviewCard, selectedCard])

  const handlePointerLeave = useCallback(() => {
    if (longPressTimer.current) {
      clearTimeout(longPressTimer.current)
      longPressTimer.current = null
    }
    setPreviewCard(null)
  }, [setPreviewCard])

  const handlePointerDown = useCallback((e: ThreeEvent<PointerEvent>) => {
    if (!cardData) return
    e.stopPropagation()

    pointerDownTime.current = Date.now()
    isLongPress.current = false

    longPressTimer.current = setTimeout(() => {
      isLongPress.current = true
      // Long press shows preview (for mobile)
      setPreviewCard(cardData)
    }, LONG_PRESS_DURATION)
  }, [cardData, setPreviewCard])

  const handlePointerUp = useCallback((e: ThreeEvent<PointerEvent>) => {
    if (!cardData) return
    e.stopPropagation()

    const pressDuration = Date.now() - pointerDownTime.current

    if (longPressTimer.current) {
      clearTimeout(longPressTimer.current)
      longPressTimer.current = null
    }

    // If it was a long press, just hide preview on release
    if (isLongPress.current) {
      setPreviewCard(null)
      isLongPress.current = false
      return
    }

    // Short tap/click - select the card and show action menu
    if (pressDuration < LONG_PRESS_DURATION) {
      setPreviewCard(null)
      setSelectedCard(cardData)
    }
  }, [cardData, setPreviewCard, setSelectedCard])

  // Stop click events from propagating to the table surface
  const handleClick = useCallback((e: ThreeEvent<MouseEvent>) => {
    if (!cardData) return
    e.stopPropagation()
  }, [cardData])

  return {
    onPointerEnter: handlePointerEnter,
    onPointerLeave: handlePointerLeave,
    onPointerDown: handlePointerDown,
    onPointerUp: handlePointerUp,
    onClick: handleClick,
  }
}