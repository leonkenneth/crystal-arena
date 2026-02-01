import { createContext, useContext, useCallback, useRef } from 'react'
import * as THREE from 'three'

type CardPositions = Map<string, THREE.Vector3>

type CardPositionsContextType = {
  registerPosition: (cardId: string, position: THREE.Vector3) => void
  unregisterPosition: (cardId: string) => void
  getPosition: (cardId: string) => THREE.Vector3 | undefined
  positions: React.MutableRefObject<CardPositions>
}

export const CardPositionsContext = createContext<CardPositionsContextType | null>(null)

export function useCardPositions() {
  const context = useContext(CardPositionsContext)
  if (!context) {
    throw new Error('useCardPositions must be used within CardPositionsProvider')
  }
  return context
}

export function useCardPositionsProvider() {
  const positions = useRef<CardPositions>(new Map())

  const registerPosition = useCallback((cardId: string, position: THREE.Vector3) => {
    positions.current.set(cardId, position.clone())
  }, [])

  const unregisterPosition = useCallback((cardId: string) => {
    positions.current.delete(cardId)
  }, [])

  const getPosition = useCallback((cardId: string) => {
    return positions.current.get(cardId)
  }, [])

  return {
    registerPosition,
    unregisterPosition,
    getPosition,
    positions,
  }
}
