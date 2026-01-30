'use client'

import { useCallback, useRef, useState } from "react"
import { ThreeEvent } from "@react-three/fiber"
import useCardContext from "./useCardContext"


// Long press duration in ms
const LONG_PRESS_DURATION = 400

// Drag activation distance in pixels
const DRAG_DISTANCE_THRESHOLD = 8

// Hook for handling hover, long-press, click, and drag
export default function useCardInteraction<T>(cardData: T | null) {
  const { previewCard, setPreviewCard, dragState, setDragState, onCardClick, onDragEnd, setIsCardInteracting } = useCardContext()
  const longPressTimer = useRef<ReturnType<typeof setTimeout> | null>(null)
  const isLongPress = useRef(false)
  const pointerDownTime = useRef(0)
  const pointerDownPosition = useRef<{ x: number; y: number } | null>(null)
  const isDragging = useRef(false)
  const [isHovered, setIsHovered] = useState(false)

  const handlePointerEnter = useCallback(() => {
    // Only show preview if no card is currently being dragged
    if (cardData && !dragState) {
      setPreviewCard(cardData)
      setIsHovered(true)
    }
  }, [cardData, setPreviewCard, dragState])

  const handlePointerLeave = useCallback(() => {
    if (longPressTimer.current) {
      clearTimeout(longPressTimer.current)
      longPressTimer.current = null
    }
    // Only clear preview if it's still showing this card and not dragging
    if (previewCard === cardData && !isDragging.current) {
      setPreviewCard(null)
    }
    setIsHovered(false)
  }, [setPreviewCard, previewCard, cardData])

  const handlePointerDown = useCallback((e: ThreeEvent<PointerEvent>) => {
    if (!cardData) return
    e.stopPropagation()

    // Mark that a card interaction is in progress (prevents panning)
    setIsCardInteracting(true)

    pointerDownTime.current = Date.now()
    pointerDownPosition.current = { x: e.nativeEvent.clientX, y: e.nativeEvent.clientY }
    isLongPress.current = false
    isDragging.current = false

    longPressTimer.current = setTimeout(() => {
      // Only trigger long press if not dragging
      if (!isDragging.current) {
        isLongPress.current = true
        // Long press shows preview (for mobile)
        setPreviewCard(cardData)
      }
    }, LONG_PRESS_DURATION)
  }, [cardData, setPreviewCard, setIsCardInteracting])

  const handlePointerMove = useCallback((e: ThreeEvent<PointerEvent>) => {
    if (!cardData || !pointerDownPosition.current) return

    const dx = e.nativeEvent.clientX - pointerDownPosition.current.x
    const dy = e.nativeEvent.clientY - pointerDownPosition.current.y
    const distance = Math.sqrt(dx * dx + dy * dy)

    // Check if we should start dragging
    if (!isDragging.current && distance >= DRAG_DISTANCE_THRESHOLD) {
      isDragging.current = true

      // Cancel long press timer when drag starts
      if (longPressTimer.current) {
        clearTimeout(longPressTimer.current)
        longPressTimer.current = null
      }

      // Clear preview when starting drag
      setPreviewCard(null)

      // Start drag
      setDragState({
        card: cardData,
        startPosition: pointerDownPosition.current,
        currentPosition: { x: e.nativeEvent.clientX, y: e.nativeEvent.clientY },
      })
    }

    // Update drag position
    if (isDragging.current && dragState) {
      setDragState({
        ...dragState,
        currentPosition: { x: e.nativeEvent.clientX, y: e.nativeEvent.clientY },
      })
    }
  }, [cardData, setPreviewCard, dragState, setDragState])

  const handlePointerUp = useCallback((e: ThreeEvent<PointerEvent>) => {
    if (!cardData) return
    e.stopPropagation()

    // Mark that card interaction is complete
    setIsCardInteracting(false)

    const pressDuration = Date.now() - pointerDownTime.current

    if (longPressTimer.current) {
      clearTimeout(longPressTimer.current)
      longPressTimer.current = null
    }

    // Handle drag end
    if (isDragging.current) {
      isDragging.current = false
      pointerDownPosition.current = null

      // Find drop zone from pointer position
      const dropZone = findDropZone(e.nativeEvent.clientX, e.nativeEvent.clientY)

      // Notify about drag end
      if (onDragEnd) {
        onDragEnd(cardData, dropZone)
      }

      setDragState(null)
      return
    }

    pointerDownPosition.current = null

    // If it was a long press, just hide preview on release
    if (isLongPress.current) {
      setPreviewCard(null)
      isLongPress.current = false
      return
    }

    // Short tap/click - select the card and show action menu
    if (pressDuration < LONG_PRESS_DURATION) {
      setPreviewCard(null)
      onCardClick?.(cardData)
    }
  }, [cardData, setPreviewCard, onCardClick, onDragEnd, setDragState, setIsCardInteracting])

  // Stop click events from propagating to the table surface
  const handleClick = useCallback((e: ThreeEvent<MouseEvent>) => {
    if (!cardData) return
    e.stopPropagation()
  }, [cardData])

  return {
    onPointerEnter: handlePointerEnter,
    onPointerLeave: handlePointerLeave,
    onPointerDown: handlePointerDown,
    onPointerMove: handlePointerMove,
    onPointerUp: handlePointerUp,
    onClick: handleClick,
    isHovered,
  }
}

// Find drop zone based on screen coordinates
function findDropZone(clientX: number, clientY: number): string | null {
  // Get element at pointer position
  const element = document.elementFromPoint(clientX, clientY)
  if (!element) return null

  // Look for drop zone data attribute
  const dropZoneElement = element.closest('[data-drop-zone]')
  if (dropZoneElement) {
    return dropZoneElement.getAttribute('data-drop-zone')
  }

  return null
}