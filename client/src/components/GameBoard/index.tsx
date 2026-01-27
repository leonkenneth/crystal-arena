'use client'

import React, { useState, useCallback, useMemo } from 'react'
import { Canvas, useThree, ThreeEvent } from '@react-three/fiber'
import { RoundedBox, Text, Html, Environment } from '@react-three/drei'
import * as THREE from 'three'
import useCardContext, { CardContext } from './useCardContext'
import useCardInteraction from './useCardInteraction'

// Create a procedural felt texture for the game board
function useBoardTextures() {
  return useMemo(() => {
    const size = 1024
    const canvas = document.createElement('canvas')
    canvas.width = size
    canvas.height = size
    const ctx = canvas.getContext('2d')!

    // Base color - rich dark blue-green felt
    const gradient = ctx.createRadialGradient(size / 2, size / 2, 0, size / 2, size / 2, size * 0.7)
    gradient.addColorStop(0, '#1e2a3a')
    gradient.addColorStop(0.5, '#1a1a2f')
    gradient.addColorStop(1, '#151525')
    ctx.fillStyle = gradient
    ctx.fillRect(0, 0, size, size)

    // Add fine noise/grain for felt fiber texture
    for (let i = 0; i < 40000; i++) {
      const x = Math.random() * size
      const y = Math.random() * size
      const brightness = Math.random() * 0.2
      const hue = 200 + Math.random() * 40 // Blue-ish hue variation
      const sat = 30 + Math.random() * 20
      const light = 15 + brightness * 25
      ctx.fillStyle = `hsla(${hue}, ${sat}%, ${light}%, ${0.2 + Math.random() * 0.4})`
      ctx.fillRect(x, y, 1 + Math.random() * 1.5, 1 + Math.random() * 1.5)
    }

    // Add diagonal weave pattern for felt texture
    ctx.strokeStyle = 'rgba(25, 35, 55, 0.2)'
    ctx.lineWidth = 1
    for (let i = -size; i < size * 2; i += 6) {
      ctx.beginPath()
      ctx.moveTo(i, 0)
      ctx.lineTo(i + size, size)
      ctx.stroke()
    }
    ctx.strokeStyle = 'rgba(35, 25, 45, 0.15)'
    for (let i = -size; i < size * 2; i += 6) {
      ctx.beginPath()
      ctx.moveTo(i + size, 0)
      ctx.lineTo(i, size)
      ctx.stroke()
    }

    // Add subtle wear patterns / lighter patches
    for (let i = 0; i < 30; i++) {
      const x = Math.random() * size
      const y = Math.random() * size
      const radius = 40 + Math.random() * 80
      const patchGradient = ctx.createRadialGradient(x, y, 0, x, y, radius)
      patchGradient.addColorStop(0, 'rgba(50, 55, 80, 0.12)')
      patchGradient.addColorStop(0.5, 'rgba(45, 50, 75, 0.06)')
      patchGradient.addColorStop(1, 'rgba(40, 45, 70, 0)')
      ctx.fillStyle = patchGradient
      ctx.fillRect(x - radius, y - radius, radius * 2, radius * 2)
    }

    // Add darker worn spots
    for (let i = 0; i < 15; i++) {
      const x = Math.random() * size
      const y = Math.random() * size
      const radius = 20 + Math.random() * 40
      const darkGradient = ctx.createRadialGradient(x, y, 0, x, y, radius)
      darkGradient.addColorStop(0, 'rgba(10, 10, 20, 0.15)')
      darkGradient.addColorStop(1, 'rgba(10, 10, 20, 0)')
      ctx.fillStyle = darkGradient
      ctx.fillRect(x - radius, y - radius, radius * 2, radius * 2)
    }

    // Add subtle wood grain border effect at edges
    const edgeGradient = ctx.createLinearGradient(0, 0, size, 0)
    edgeGradient.addColorStop(0, 'rgba(60, 45, 30, 0.1)')
    edgeGradient.addColorStop(0.05, 'rgba(60, 45, 30, 0)')
    edgeGradient.addColorStop(0.95, 'rgba(60, 45, 30, 0)')
    edgeGradient.addColorStop(1, 'rgba(60, 45, 30, 0.1)')
    ctx.fillStyle = edgeGradient
    ctx.fillRect(0, 0, size, size)

    const colorTexture = new THREE.CanvasTexture(canvas)
    colorTexture.wrapS = THREE.RepeatWrapping
    colorTexture.wrapT = THREE.RepeatWrapping
    colorTexture.repeat.set(3, 3)
    colorTexture.needsUpdate = true

    // Create a roughness map for specular variation
    const roughnessCanvas = document.createElement('canvas')
    roughnessCanvas.width = 512
    roughnessCanvas.height = 512
    const roughnessCtx = roughnessCanvas.getContext('2d')!

    // Base roughness (felt is fairly rough)
    roughnessCtx.fillStyle = '#b0b0b0' // ~0.7 roughness
    roughnessCtx.fillRect(0, 0, 512, 512)

    // Add variation for worn/polished spots (lower roughness = more specular)
    for (let i = 0; i < 50; i++) {
      const x = Math.random() * 512
      const y = Math.random() * 512
      const radius = 20 + Math.random() * 60
      const roughnessGradient = roughnessCtx.createRadialGradient(x, y, 0, x, y, radius)
      const variation = Math.random() > 0.5 ? 0.85 : 0.65 // Vary between rougher and smoother
      const colorVal = Math.floor(variation * 255)
      roughnessGradient.addColorStop(0, `rgb(${colorVal}, ${colorVal}, ${colorVal})`)
      roughnessGradient.addColorStop(1, '#b0b0b0')
      roughnessCtx.fillStyle = roughnessGradient
      roughnessCtx.fillRect(x - radius, y - radius, radius * 2, radius * 2)
    }

    // Add fine noise to roughness
    for (let i = 0; i < 8000; i++) {
      const x = Math.random() * 512
      const y = Math.random() * 512
      const val = 140 + Math.random() * 80
      roughnessCtx.fillStyle = `rgb(${val}, ${val}, ${val})`
      roughnessCtx.fillRect(x, y, 2, 2)
    }

    const roughnessTexture = new THREE.CanvasTexture(roughnessCanvas)
    roughnessTexture.wrapS = THREE.RepeatWrapping
    roughnessTexture.wrapT = THREE.RepeatWrapping
    roughnessTexture.repeat.set(3, 3)
    roughnessTexture.needsUpdate = true

    // Create a normal map for surface detail
    const normalCanvas = document.createElement('canvas')
    normalCanvas.width = 256
    normalCanvas.height = 256
    const normalCtx = normalCanvas.getContext('2d')!

    // Neutral normal (pointing up)
    normalCtx.fillStyle = '#8080ff'
    normalCtx.fillRect(0, 0, 256, 256)

    // Add subtle bumps for felt texture
    for (let i = 0; i < 3000; i++) {
      const x = Math.random() * 256
      const y = Math.random() * 256
      const nx = 128 + (Math.random() - 0.5) * 30
      const ny = 128 + (Math.random() - 0.5) * 30
      normalCtx.fillStyle = `rgb(${nx}, ${ny}, 255)`
      normalCtx.fillRect(x, y, 1 + Math.random(), 1 + Math.random())
    }

    const normalTexture = new THREE.CanvasTexture(normalCanvas)
    normalTexture.wrapS = THREE.RepeatWrapping
    normalTexture.wrapT = THREE.RepeatWrapping
    normalTexture.repeat.set(6, 6)
    normalTexture.needsUpdate = true

    return { colorTexture, roughnessTexture, normalTexture }
  }, [])
}

// Table surface with divider
function TableSurface({ onClick }: { onClick: () => void }) {
  const { colorTexture, roughnessTexture, normalTexture } = useBoardTextures()

  return (
    <group>
      {/* Table surface with enhanced materials */}
      <mesh position={[0, 0, -0.2]} onClick={onClick} receiveShadow>
        <boxGeometry args={[20, 15, 0.1]} />
        <meshStandardMaterial
          map={colorTexture}
          roughnessMap={roughnessTexture}
          normalMap={normalTexture}
          normalScale={new THREE.Vector2(0.3, 0.3)}
          roughness={0.7}
          metalness={0.05}
          envMapIntensity={0.8}
        />
      </mesh>

      {/* Raised table edge/border - wood trim */}
      <mesh position={[0, 7.55, -0.15]}>
        <boxGeometry args={[20.2, 0.15, 0.2]} />
        <meshStandardMaterial color="#3d2a1a" roughness={0.4} metalness={0.1} />
      </mesh>
      <mesh position={[0, -7.55, -0.15]}>
        <boxGeometry args={[20.2, 0.15, 0.2]} />
        <meshStandardMaterial color="#3d2a1a" roughness={0.4} metalness={0.1} />
      </mesh>
      <mesh position={[10.05, 0, -0.15]}>
        <boxGeometry args={[0.15, 15.1, 0.2]} />
        <meshStandardMaterial color="#3d2a1a" roughness={0.4} metalness={0.1} />
      </mesh>
      <mesh position={[-10.05, 0, -0.15]}>
        <boxGeometry args={[0.15, 15.1, 0.2]} />
        <meshStandardMaterial color="#3d2a1a" roughness={0.4} metalness={0.1} />
      </mesh>

      {/* Center divider line with metallic trim */}
      <mesh position={[0, 0, 0]}>
        <boxGeometry args={[20, 0.08, 0.04]} />
        <meshStandardMaterial
          color="#c9a86c"
          roughness={0.25}
          metalness={0.7}
          envMapIntensity={1.2}
        />
      </mesh>
    </group>
  )
}

// Card dimensions (roughly standard card proportions)
const CARD_WIDTH = 0.7
const CARD_HEIGHT = 1
const CARD_DEPTH = 0.02

const COLORS = {
  cardFront: '#e8e0d5',
  cardBack: '#2a4858',
  cardBorder: '#1a1a1a',
  deck: '#1e3a4c',
  graveyard: '#3d2c3d',
  battlefield: '#2d5a3d',
  hand: '#4a3728',
}

// Zone type for game areas
type Zone = 'hand' | 'battlefield' | 'deck' | 'graveyard'

// Expanded pile data
export type ExpandedPile<T> = {
  type: 'deck' | 'graveyard'
  cards: T[]
  title: string
}

// Stack effect type
export type StackEffect<T> = {
  card: T
  targetCardIds: string[]
}

// Hook to get layout info based on viewport
function useLayout() {
  const { viewport } = useThree()
  const isPortrait = viewport.height > viewport.width
  const aspect = viewport.width / viewport.height

  // Scale factor based on viewport size
  const baseScale = isPortrait
    ? Math.min(1, viewport.width / 5)
    : Math.min(1, viewport.width / 12)

  return {
    isPortrait,
    aspect,
    scale: baseScale,
    viewport,
    // Reduce battlefield slots on portrait
    maxBattlefieldSlots: isPortrait ? 5 : 7,
    // Hand spread settings - wider for better readability
    handSpreadAngle: 12,
    handSpreadRadius: 3,
  }
}

// Wrapper component that adds interaction to rendered cards
type InteractiveCardProps<T> = {
  card: T
  faceDown: boolean
  interactive: boolean
  renderCardMesh: (card: T, faceDown: boolean) => React.ReactNode
}

function InteractiveCard<T>({
  card,
  faceDown,
  interactive,
  renderCardMesh,
}: InteractiveCardProps<T>) {
  const interactions = useCardInteraction<T>(interactive && !faceDown ? card : null)

  return (
    <group {...(interactive && !faceDown ? interactions : {})}>
      {renderCardMesh(card, faceDown)}
    </group>
  )
}

type DeckProps<T> = {
  cardCount?: number
  position?: [number, number, number]
  scale?: number
  cards?: T[]
  label?: string
  renderCardMesh: (card: T, faceDown: boolean) => React.ReactNode
}

function Deck<T>({ cardCount = 30, position = [0, 0, 0], scale = 1, cards = [], label = 'Deck', renderCardMesh }: DeckProps<T>) {
  const { setExpandedPile } = useCardContext()
  const stackHeight = Math.min(cardCount, 30) * 0.01

  const handleClick = useCallback((e: ThreeEvent<MouseEvent>) => {
    e.stopPropagation()
    if (cards.length > 0) {
      setExpandedPile({ type: 'deck', cards, title: label })
    }
  }, [cards, label, setExpandedPile])

  return (
    <group position={position} scale={scale} onClick={handleClick}>
      {/* Deck base */}
      <RoundedBox
        args={[CARD_WIDTH + 0.05, CARD_HEIGHT + 0.05, stackHeight]}
        radius={0.03}
        smoothness={4}
        position={[0, 0, stackHeight / 2]}
      >
        <meshStandardMaterial color={COLORS.deck} />
      </RoundedBox>
      {/* Top card - face down */}
      <group position={[0, 0, stackHeight + CARD_DEPTH / 2]}>
        {cards.length > 0 && renderCardMesh(cards[0]!, true)}
      </group>
      {/* Card count */}
      <Text
        position={[0, -CARD_HEIGHT / 2 - 0.15, 0.1]}
        fontSize={0.15}
        color="#aaa"
        anchorX="center"
        anchorY="middle"
      >
        {cardCount}
      </Text>
    </group>
  )
}

type GraveyardProps<T> = {
  cardCount?: number
  position?: [number, number, number]
  scale?: number
  cards?: T[]
  label?: string
  renderCardMesh: (card: T, faceDown: boolean) => React.ReactNode,
  renderEmptySlot: () => React.ReactNode,
}

function Graveyard<T>({ cardCount = 5, position = [0, 0, 0], scale = 1, cards = [], label = 'Graveyard', renderCardMesh, renderEmptySlot }: GraveyardProps<T>) {
  const { setExpandedPile } = useCardContext()
  const stackHeight = Math.min(cardCount, 20) * 0.008
  const topCard = cards.length > 0 ? cards[cards.length - 1] : null

  const handleClick = useCallback((e: ThreeEvent<MouseEvent>) => {
    e.stopPropagation()
    if (cards.length > 0) {
      setExpandedPile({ type: 'graveyard', cards, title: label })
    }
  }, [cards, label, setExpandedPile])

  return (
    <group position={position} scale={scale} onClick={handleClick}>
      {/* Graveyard zone marker */}
      <mesh position={[0, 0, -0.01]} rotation={[-Math.PI / 2, 0, 0]}>
        <planeGeometry args={[CARD_WIDTH + 0.1, CARD_HEIGHT + 0.1]} />
        <meshStandardMaterial color={COLORS.graveyard} transparent opacity={0.5} />
      </mesh>
      {cardCount > 0 && (
        <>
          {/* Stack of cards */}
          <RoundedBox
            args={[CARD_WIDTH, CARD_HEIGHT, stackHeight]}
            radius={0.03}
            smoothness={4}
            position={[0, 0, stackHeight / 2]}
          >
            <meshStandardMaterial color={COLORS.graveyard} />
          </RoundedBox>
          {/* Top card (face up in graveyard) */}
          <group position={[0, 0, stackHeight + CARD_DEPTH / 2]}>
            {topCard && renderCardMesh(topCard, false)}
            {!topCard && renderEmptySlot()}
          </group>
        </>
      )}
      {label && cardCount > 0 && (
        <Text
          position={[0, -CARD_HEIGHT / 2 - 0.15, 0.1]}
          fontSize={0.1}
          color="#888"
          anchorX="center"
          anchorY="middle"
        >
          {label} ({cardCount})
        </Text>
      )}
    </group>
  )
}

type HandProps<T> = {
  cards?: T[]
  isOpponent?: boolean
  position?: [number, number, number]
  spreadAngle?: number
  spreadRadius?: number
  renderCardMesh: (card: T, faceDown: boolean) => React.ReactNode
  getCardId: (card: T) => string | number
}

function Hand<T>({
  cards = [],
  isOpponent = false,
  position = [0, 0, 0],
  spreadAngle = 12,
  spreadRadius = 3,
  renderCardMesh,
  getCardId,
}: HandProps<T>) {
  const totalSpread = (cards.length - 1) * spreadAngle
  const startAngle = -totalSpread / 2 // Start from left

  return (
    <group position={position}>
      {cards.map((card, index) => {
        // Angle for this card in the fan (in radians)
        const angleDeg = startAngle + index * spreadAngle
        const angleRad = angleDeg * (Math.PI / 180)

        // Position cards in an arc
        const fanDirection = isOpponent ? -1 : 1
        const xOffset = Math.sin(angleRad) * spreadRadius
        const yOffset = -5*(1 - Math.cos(angleRad)) * 0.5 * fanDirection
        const zOffset = (cards.length - Math.abs(index - (cards.length - 1) / 2)) * 0.03
        const xRotation = 0.7
        const zRotation = -angleRad * fanDirection

        return (
          <group
            key={getCardId(card)}
            position={[xOffset, yOffset, zOffset]}
            rotation={[xRotation, 0, zRotation]}
          >
            <InteractiveCard
              card={card}
              faceDown={isOpponent}
              interactive={!isOpponent}
              renderCardMesh={renderCardMesh}
            />
          </group>
        )
      })}
    </group>
  )
}

type BattlefieldProps<T> = {
  cards?: T[]
  position?: [number, number, number]
  isOpponent?: boolean
  maxSlots?: number
  renderCardMesh: (card: T, faceDown: boolean) => React.ReactNode
  getCardId: (card: T) => string | number
}

function Battlefield<T>({ cards = [], position = [0, 0, 0], isOpponent = false, maxSlots = 7, renderCardMesh, getCardId }: BattlefieldProps<T>) {
  const slotWidth = CARD_WIDTH + 0.15

  return (
    <group position={position}>
      {/* Battlefield zone */}
      <mesh rotation={[-Math.PI / 2, 0, 0]} position={[0, 0, -0.02]}>
        <planeGeometry args={[slotWidth * maxSlots, CARD_HEIGHT + 0.3]} />
        <meshStandardMaterial color={COLORS.battlefield} transparent opacity={0.3} />
      </mesh>
      {/* Card slots */}
      {Array.from({ length: maxSlots }).map((_, index) => {
        const xPos = (index - (maxSlots - 1) / 2) * slotWidth
        const card = cards[index]

        return (
          <group key={card ? getCardId(card) : `slot-${index}`} position={[xPos, 0, 0]}>
            {/* Slot outline */}
            <mesh rotation={[-Math.PI / 2, 0, 0]} position={[0, 0, -0.01]}>
              <planeGeometry args={[CARD_WIDTH + 0.08, CARD_HEIGHT + 0.08]} />
              <meshStandardMaterial color="#1a3d1a" transparent opacity={0.5} />
            </mesh>
            {/* Card if present */}
            {card && (
              <group position={[0, 0, CARD_DEPTH / 2]} rotation={[isOpponent ? Math.PI : 0, 0, 0]}>
                <InteractiveCard
                  card={card}
                  faceDown={false}
                  interactive={true}
                  renderCardMesh={renderCardMesh}
                />
              </group>
            )}
          </group>
        )
      })}
    </group>
  )
}

type StackDisplayProps<T> = {
  stack: StackEffect<T>[]
  position?: [number, number, number]
  scale?: number
  renderCardMesh: (card: T, faceDown: boolean) => React.ReactNode
  getCardId: (card: T) => string | number
  renderButton?: () => React.ReactNode
}

function StackDisplay<T>({
  stack,
  position = [0, 0, 0],
  scale = 1,
  renderCardMesh,
  getCardId,
  renderButton,
}: StackDisplayProps<T>) {
  if (stack.length === 0) return null

  return (
    <group position={position} scale={scale}>
      {/* Stack label */}
      <Text
        position={[0, CARD_HEIGHT / 2 + 0.15, 0.1]}
        fontSize={0.12}
        color="#ddd"
        anchorX="center"
        anchorY="middle"
      >
        Stack ({stack.length})
      </Text>

      {/* Cards in stack - fanned tightly, newest on top */}
      {stack.map((effect, index) => {
        // Horizontal offset for tight fan
        const xOffset = (index - (stack.length - 1) / 2) * CARD_WIDTH * 0.35
        const zOffset = index * 0.02 // slight z offset so newer cards are on top

        return (
          <group
            key={getCardId(effect.card)}
            position={[xOffset, 0, zOffset + CARD_DEPTH / 2]}
          >
            <InteractiveCard
              card={effect.card}
              faceDown={false}
              interactive={true}
              renderCardMesh={renderCardMesh}
            />
          </group>
        )
      })}

      {/* Stack button */}
      {renderButton && (
        <Html
          position={[0, -CARD_HEIGHT / 2 - 0.25, 0.1]}
          center
          style={{ pointerEvents: 'auto' }}
        >
          {renderButton()}
        </Html>
      )}
    </group>
  )
}

type PlayerAvatarProps = {
  health: number
  isOpponent?: boolean
  position?: [number, number, number]
  scale?: number
}

function PlayerAvatar({
  health,
  isOpponent = false,
  position = [0, 0, 0],
  scale = 1,
}: PlayerAvatarProps) {
  const primaryColor = isOpponent ? '#c44' : '#48c'
  const secondaryColor = isOpponent ? '#822' : '#269'
  const frameColor = isOpponent ? '#fca' : '#adf'

  return (
    <group position={position} scale={scale}>
      {/* Main portrait - upright facing camera */}
      <group>
        {/* Frame */}
        <RoundedBox
          args={[1.4, 1.6, 0.1]}
          radius={0.1}
          smoothness={4}
        >
          <meshStandardMaterial color={frameColor} />
        </RoundedBox>

        {/* Portrait background */}
        <RoundedBox
          args={[1.2, 1.4, 0.12]}
          radius={0.08}
          smoothness={4}
          position={[0, 0.05, 0]}
        >
          <meshStandardMaterial color={primaryColor} />
        </RoundedBox>

        {/* Inner accent */}
        <RoundedBox
          args={[0.9, 0.9, 0.14]}
          radius={0.1}
          smoothness={4}
          position={[0, 0.15, 0]}
        >
          <meshStandardMaterial color={secondaryColor} />
        </RoundedBox>

        {/* Health display at bottom */}
        <group position={[0, -0.55, 0.1]}>
          {/* Health background */}
          <RoundedBox
            args={[0.7, 0.4, 0.08]}
            radius={0.05}
            smoothness={4}
          >
            <meshStandardMaterial color="#222" />
          </RoundedBox>
          {/* Health number */}
          <Text
            position={[0, 0, 0.06]}
            fontSize={0.28}
            color="#ff5555"
            anchorX="center"
            anchorY="middle"
          >
            {health}
          </Text>
        </group>
      </group>
    </group>
  )
}

// Step indicator type
export type Step = {
  id: string
  label: string
  isActive: boolean
}

type StepIndicatorProps = {
  steps: Step[]
  position?: [number, number, number]
  scale?: number
  isOpponentTurn?: boolean
}

function StepIndicator({
  steps,
  position = [0, 0, 0],
  scale = 1,
  isOpponentTurn = false,
}: StepIndicatorProps) {
  const stepSize = 0.3
  const stepGap = 0.15
  const totalWidth = steps.length * stepSize + (steps.length - 1) * stepGap
  const activeStep = steps.find((step) => step.isActive)

  // Colors change based on whose turn it is
  const activeColor = isOpponentTurn ? '#c44' : '#48c'
  const activeFrameColor = isOpponentTurn ? '#faa' : '#adf'
  const activeEmissive = isOpponentTurn ? '#c44' : '#48c'

  return (
    <group position={position} scale={scale}>
      {steps.map((step, index) => {
        const xOffset = index * (stepSize + stepGap) - totalWidth / 2 + stepSize / 2
        const inactiveColor = '#3a3a4a'
        const frameColor = step.isActive ? activeFrameColor : '#2a2a3a'

        return (
          <group key={step.id} position={[xOffset, 0, 0]}>
            {/* Frame */}
            <RoundedBox
              args={[stepSize + 0.06, stepSize + 0.06, 0.08]}
              radius={0.04}
              smoothness={4}
            >
              <meshStandardMaterial
                color={frameColor}
                emissive={step.isActive ? activeEmissive : '#000'}
                emissiveIntensity={step.isActive ? 0.3 : 0}
              />
            </RoundedBox>

            {/* Inner square */}
            <RoundedBox
              args={[stepSize, stepSize, 0.1]}
              radius={0.03}
              smoothness={4}
              position={[0, 0, 0.01]}
            >
              <meshStandardMaterial
                color={step.isActive ? activeColor : inactiveColor}
              />
            </RoundedBox>
          </group>
        )
      })}

      {/* Active step label */}
      {activeStep && (
        <Text
          position={[0, -stepSize / 2 - 0.2, 0.1]}
          fontSize={0.18}
          color={activeFrameColor}
          anchorX="center"
          anchorY="top"
        >
          {activeStep.label}
        </Text>
      )}
    </group>
  )
}

type PlayerAreaProps<T> = {
  isOpponent?: boolean
  deckCards?: T[]
  graveyardCards?: T[]
  handCards?: T[]
  battlefieldCards?: T[]
  renderCardMesh: (card: T, faceDown: boolean) => React.ReactNode
  renderEmptySlot: () => React.ReactNode
  getCardId: (card: T) => string | number
}

type GameBoardProps<T> = {
  yourHand: T[]
  yourBattlefield: T[]
  yourDeck: T[]
  yourGraveyard: T[]
  yourHealth: number
  opponentHand: T[]
  opponentBattlefield: T[]
  opponentDeck: T[]
  opponentGraveyard: T[]
  opponentHealth: number
  renderHtmlCard: (card: T, faceDown: boolean) => React.ReactNode
  renderCardMesh: (card: T, faceDown: boolean) => React.ReactNode
  renderEmptySlot: () => React.ReactNode
  getCardId: (card: T) => string | number
  onCardClick?: (card: T, zone: Zone) => void
  onCardHover?: (card: T | null) => void
  stack?: StackEffect<T>[] | null
  stackButton?: () => React.ReactNode
  steps?: Step[]
  isOpponentTurn?: boolean
  renderDialog?: () => React.ReactNode | null
  renderMessage?: () => React.ReactNode | null
  renderBottomBar?: () => React.ReactNode | null
}

function PlayerArea<T>({
  isOpponent = false,
  deckCards = [],
  graveyardCards = [],
  handCards = [],
  battlefieldCards = [],
  renderCardMesh,
  renderEmptySlot,
  getCardId,
}: PlayerAreaProps<T>) {
  const layout = useLayout()
  const { isPortrait, scale } = layout

  // Adjust positions based on orientation
  const handY = isOpponent ? (isPortrait ? 4.5 : 3.5) : isPortrait ? -4.5 : -3.5

  const battlefieldY = isOpponent ? (isPortrait ? 2 : 1.5) : isPortrait ? -2 : -1.5

  // In portrait: deck/graveyard go to the side but closer
  // In landscape: deck/graveyard go further to the side
  const sideX = isPortrait ? 2.5 : 5
  const sideY = isOpponent ? (isPortrait ? 3.2 : 2.5) : isPortrait ? -3.2 : -2.5

  const deckGraveyardScale = isPortrait ? 0.7 : 1
  const playerLabel = isOpponent ? "Opponent's" : 'Your'

  return (
    <group scale={[scale, scale, scale]}>
      {/* Hand */}
      <Hand
        cards={handCards}
        isOpponent={isOpponent}
        position={[0, handY, isOpponent ? 0 : 1]}
        spreadAngle={layout.handSpreadAngle}
        spreadRadius={layout.handSpreadRadius}
        renderCardMesh={renderCardMesh}
        getCardId={getCardId}
      />

      {/* Battlefield */}
      <Battlefield
        cards={battlefieldCards}
        isOpponent={isOpponent}
        position={[0, battlefieldY, 0]}
        maxSlots={layout.maxBattlefieldSlots}
        renderCardMesh={renderCardMesh}
        getCardId={getCardId}
      />

      {/* Deck (on the right side from player's perspective) */}
      <Deck
        cardCount={deckCards.length}
        cards={deckCards as unknown as T[]}
        label={`${playerLabel} Deck`}
        position={[sideX, sideY, 0]}
        scale={deckGraveyardScale}
        renderCardMesh={renderCardMesh}
      />

      {/* Graveyard (next to deck) */}
      <Graveyard
        cardCount={graveyardCards.length}
        cards={graveyardCards}
        label={`${playerLabel} Graveyard`}
        position={[sideX - (isPortrait ? 0.9 : 1.2), sideY, 0]}
        scale={deckGraveyardScale}
        renderCardMesh={renderCardMesh}
        renderEmptySlot={renderEmptySlot}
      />
    </group>
  )
}

function GameBoardScene<T>({
  yourHand,
  yourBattlefield,
  yourDeck,
  yourGraveyard,
  yourHealth,
  opponentHand,
  opponentBattlefield,
  opponentDeck,
  opponentGraveyard,
  opponentHealth,
  renderCardMesh,
  renderEmptySlot,
  getCardId,
  stack,
  stackButton,
  steps,
  isOpponentTurn,
}: GameBoardProps<T>) {
  const layout = useLayout()
  const { viewport, isPortrait, scale } = layout
  const { setSelectedCard, setExpandedPile } = useCardContext()

  // Click on empty space to deselect
  const handleBackgroundClick = useCallback(() => {
    setSelectedCard(null)
    setExpandedPile(null)
  }, [setSelectedCard, setExpandedPile])

  return (
    <group>
      {/* Table surface with divider */}
      <TableSurface onClick={handleBackgroundClick} />

      {/* Center divider line */}
      <mesh rotation={[-Math.PI / 2, 0, 0]} position={[0, 0, -0.05]}>
        <planeGeometry args={[isPortrait ? viewport.width * 0.8 : viewport.width, 0.05]} />
        <meshStandardMaterial color="#4a4a4a" />
      </mesh>

      {/* Stack display (on player's side, right of center) */}
      {stack && stack.length > 0 && (
        <StackDisplay
          stack={stack}
          position={[isPortrait ? 2.2 : 4, isPortrait ? -0.5 : -0.3, 0.5]}
          scale={scale * (isPortrait ? 0.8 : 0.9)}
          renderCardMesh={renderCardMesh}
          getCardId={getCardId}
          renderButton={stackButton}
        />
      )}

      {/* Player's area (bottom) */}
      <PlayerArea
        isOpponent={false}
        deckCards={yourDeck}
        graveyardCards={yourGraveyard}
        handCards={yourHand}
        battlefieldCards={yourBattlefield}
        renderCardMesh={renderCardMesh}
        renderEmptySlot={renderEmptySlot}
        getCardId={getCardId}
      />

      {/* Opponent's area (top) */}
      <PlayerArea
        isOpponent={true}
        deckCards={opponentDeck}
        graveyardCards={opponentGraveyard}
        handCards={opponentHand}
        battlefieldCards={opponentBattlefield}
        renderCardMesh={renderCardMesh}
        renderEmptySlot={renderEmptySlot}
        getCardId={getCardId}
      />

      {/* Player avatars with health - same vertical as deck/graveyard, on left side */}
      <PlayerAvatar
        health={yourHealth}
        isOpponent={false}
        position={[(isPortrait ? -2.5 : -5) * scale, (isPortrait ? -3.2 : -2.5) * scale, 1]}
        scale={scale * (isPortrait ? 0.7 : 0.85)}
      />
      <PlayerAvatar
        health={opponentHealth}
        isOpponent={true}
        position={[(isPortrait ? -2.5 : -5) * scale, (isPortrait ? 3.2 : 2.5) * scale, 1]}
        scale={scale * (isPortrait ? 0.7 : 0.85)}
      />

      {/* Step indicator in bottom-left corner */}
      {steps && steps.length > 0 && (
        <StepIndicator
          steps={steps}
          position={[(isPortrait ? -2 : -4) * scale, (isPortrait ? -5.5 : -4.5) * scale, 1]}
          scale={scale * (isPortrait ? 0.8 : 1)}
          isOpponentTurn={isOpponentTurn}
        />
      )}
    </group>
  )
}

// Preview card overlay (hover/long-press) - no actions
function PreviewCardOverlay<T>({ renderHtmlCard }: { renderHtmlCard: (card: T, faceDown: boolean) => React.ReactNode, getCardId: (card: T) => string | number }) {
  const { previewCard } = useCardContext()

  if (!previewCard) return null

  return (
    <Html
      center
      position={[0, 0, 5]}
      style={{
        pointerEvents: 'none',
        userSelect: 'none',
      }}
    >
    {renderHtmlCard(previewCard, false)}
    </Html>
  )
}

// Selected card overlay with action menu
function SelectedCardOverlay<T>({ renderHtmlCard, getCardId }: { renderHtmlCard: (card: T, faceDown: boolean) => React.ReactNode, getCardId: (card: T) => string | number }) {
  const { selectedCard, setSelectedCard } = useCardContext()

  if (!selectedCard) return null

  const handleAction = (action: string) => {
    console.log(`Action "${action}" triggered for card:`, getCardId(selectedCard))
    // In a real game, this would trigger game logic
    setSelectedCard(null)
  }

  const handleClose = () => {
    setSelectedCard(null)
  }

  return (
    <Html
      center
      position={[0, 0, 6]}
      style={{
        pointerEvents: 'none',
      }}
    >
      {/* Full screen container */}
      <div
        onClick={handleClose}
        style={{
          position: 'fixed',
          top: 0,
          left: 0,
          right: 0,
          bottom: 0,
          backgroundColor: 'rgba(0,0,0,0.5)',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          pointerEvents: 'auto',
        }}
      >
        {/* Card and menu container */}
        <div
          onClick={(e) => e.stopPropagation()}
          style={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            gap: '16px',
          }}
        >
        {/* Zoomed card */}
        {renderHtmlCard(selectedCard, false)}

        {/* Action menu */}
        <div
          style={{
            display: 'flex',
            flexDirection: 'column',
            gap: '8px',
            width: '200px',
          }}
        >
          <button
            onClick={() => handleAction('ability1')}
            style={{
              padding: '12px 16px',
              fontSize: '14px',
              fontWeight: 'bold',
              backgroundColor: '#4a7c59',
              color: 'white',
              border: 'none',
              borderRadius: '8px',
              cursor: 'pointer',
              boxShadow: '0 4px 12px rgba(0,0,0,0.3)',
              transition: 'transform 0.1s, background-color 0.1s',
            }}
            onMouseEnter={(e) => {
              e.currentTarget.style.backgroundColor = '#5a9c69'
              e.currentTarget.style.transform = 'scale(1.02)'
            }}
            onMouseLeave={(e) => {
              e.currentTarget.style.backgroundColor = '#4a7c59'
              e.currentTarget.style.transform = 'scale(1)'
            }}
          >
            🎯 Use Special Ability
          </button>

          <button
            onClick={() => handleAction('ability2')}
            style={{
              padding: '12px 16px',
              fontSize: '14px',
              fontWeight: 'bold',
              backgroundColor: '#7c4a4a',
              color: 'white',
              border: 'none',
              borderRadius: '8px',
              cursor: 'pointer',
              boxShadow: '0 4px 12px rgba(0,0,0,0.3)',
              transition: 'transform 0.1s, background-color 0.1s',
            }}
            onMouseEnter={(e) => {
              e.currentTarget.style.backgroundColor = '#9c5a5a'
              e.currentTarget.style.transform = 'scale(1.02)'
            }}
            onMouseLeave={(e) => {
              e.currentTarget.style.backgroundColor = '#7c4a4a'
              e.currentTarget.style.transform = 'scale(1)'
            }}
          >
            ⚔️ Attack Enemy
          </button>

          <button
            onClick={handleClose}
            style={{
              padding: '10px 16px',
              fontSize: '13px',
              backgroundColor: '#555',
              color: '#ccc',
              border: 'none',
              borderRadius: '8px',
              cursor: 'pointer',
              marginTop: '4px',
            }}
          >
            Cancel
          </button>
        </div>
        </div>
      </div>
    </Html>
  )
}

// Expanded pile overlay (deck or graveyard contents)
function ExpandedPileOverlay<T>({ renderHtmlCard, getCardId }: { renderHtmlCard: (card: T, faceDown: boolean) => React.ReactNode, getCardId: (card: T) => string | number }) {
  const { expandedPile, setExpandedPile, setSelectedCard } = useCardContext()
  const { isPortrait } = useLayout()

  if (!expandedPile) return null

  const handleClose = () => {
    setExpandedPile(null)
  }

  const handleCardClick = (card: T) => {
    setExpandedPile(null)
    setSelectedCard(card)
  }

  const isDeck = expandedPile.type === 'deck'
  const baseZ = isPortrait ? 7 : 4

  return (
    <Html
      center
      position={[0, 0, baseZ]}
      style={{
        pointerEvents: 'none',
      }}
    >
      {/* Full screen container */}
      <div
        onClick={handleClose}
        style={{
          position: 'fixed',
          top: 0,
          left: 0,
          right: 0,
          bottom: 0,
          backgroundColor: 'rgba(0,0,0,0.7)',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          pointerEvents: 'auto',
        }}
      >
        {/* Pile viewer container */}
        <div
          onClick={(e) => e.stopPropagation()}
          style={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            gap: '12px',
            maxWidth: '90vw',
          }}
        >
        {/* Title */}
        <div
          style={{
            fontSize: '20px',
            fontWeight: 'bold',
            color: 'white',
            textShadow: '0 2px 4px rgba(0,0,0,0.5)',
            display: 'flex',
            alignItems: 'center',
            gap: '8px',
          }}
        >
          <span>{isDeck ? '📚' : '💀'}</span>
          <span>{expandedPile.title}</span>
          <span style={{ fontSize: '14px', color: '#aaa' }}>
            ({expandedPile.cards.length} cards)
          </span>
        </div>

        {/* Scrollable card list */}
        <div
          style={{
            display: 'flex',
            gap: '12px',
            padding: '16px',
            overflowX: 'auto',
            overflowY: 'hidden',
            maxWidth: '85vw',
            backgroundColor: 'rgba(0,0,0,0.3)',
            borderRadius: '12px',
            border: `2px solid ${isDeck ? '#1e3a4c' : '#3d2c3d'}`,
          }}
        >
          {expandedPile.cards.map((card, index) => (
            <div
              key={getCardId(card)}
              onClick={() => handleCardClick(card)}
              style={{
                flexShrink: 0,
                width: '120px',
                height: '168px',
                backgroundColor: isDeck ? COLORS.cardBack : card.color,
                borderRadius: '8px',
                border: '2px solid #1a1a1a',
                boxShadow: '0 4px 12px rgba(0,0,0,0.4)',
                display: 'flex',
                flexDirection: 'column',
                padding: '8px',
                color: isDeck ? '#aaa' : '#333',
                fontFamily: 'system-ui, sans-serif',
                cursor: 'pointer',
                transition: 'transform 0.15s, box-shadow 0.15s',
              }}
              onMouseEnter={(e) => {
                e.currentTarget.style.transform = 'translateY(-8px) scale(1.05)'
                e.currentTarget.style.boxShadow = '0 8px 24px rgba(0,0,0,0.6)'
              }}
              onMouseLeave={(e) => {
                e.currentTarget.style.transform = 'translateY(0) scale(1)'
                e.currentTarget.style.boxShadow = '0 4px 12px rgba(0,0,0,0.4)'
              }}
            >
              {/* Card index for deck */}
              {isDeck && (
                <div
                  style={{
                    position: 'absolute',
                    top: '4px',
                    right: '4px',
                    fontSize: '10px',
                    color: '#666',
                  }}
                >
                  #{index + 1}
                </div>
              )}

              {renderHtmlCard(card, false)}
            </div>
          ))}
        </div>

        {/* Close button */}
        <button
          onClick={handleClose}
          style={{
            padding: '10px 24px',
            fontSize: '14px',
            backgroundColor: '#555',
            color: '#fff',
            border: 'none',
            borderRadius: '8px',
            cursor: 'pointer',
            boxShadow: '0 4px 12px rgba(0,0,0,0.3)',
          }}
        >
          Close
        </button>
        </div>
      </div>
    </Html>
  )
}

// Responsive camera that adjusts based on viewport
function ResponsiveCamera() {
  const { viewport, camera } = useThree()
  const isPortrait = viewport.height > viewport.width

  React.useEffect(() => {
    if (isPortrait) {
      // Portrait: pull camera back and adjust angle for vertical view
      camera.position.set(0, -6, 14)
    } else {
      // Landscape: standard view
      camera.position.set(0, -8, 12)
    }
    camera.lookAt(0, 0, 0)
    camera.updateProjectionMatrix()
  }, [isPortrait, camera])

  return null
}

function GameBoardCanvas<T>(props: GameBoardProps<T>) {
  return (
    <>
      {/* Responsive camera adjustment */}
      <ResponsiveCamera />

      {/* Lighting - enhanced for specular highlights */}
      <ambientLight intensity={0.3} />

      {/* Main overhead light - creates primary specular reflections */}
      <directionalLight
        position={[2, -3, 15]}
        intensity={1.2}
        castShadow
        shadow-mapSize-width={2048}
        shadow-mapSize-height={2048}
        shadow-camera-far={50}
        shadow-camera-left={-15}
        shadow-camera-right={15}
        shadow-camera-top={15}
        shadow-camera-bottom={-15}
      />

      {/* Secondary fill light from the side */}
      <directionalLight
        position={[-8, 2, 10]}
        intensity={0.5}
        color="#e8e0f0"
      />

      {/* Accent lights for dramatic specular highlights */}
      <spotLight
        position={[6, -6, 12]}
        angle={0.4}
        penumbra={0.5}
        intensity={0.8}
        color="#fff8e8"
        castShadow
      />
      <spotLight
        position={[-6, 6, 10]}
        angle={0.5}
        penumbra={0.6}
        intensity={0.6}
        color="#e8f0ff"
      />

      {/* Colored rim lights for atmosphere */}
      <pointLight position={[-8, 6, 4]} intensity={0.4} color="#6688cc" distance={20} />
      <pointLight position={[8, -6, 4]} intensity={0.4} color="#cc8866" distance={20} />
      <pointLight position={[0, 0, 8]} intensity={0.25} color="#ffffff" distance={15} />

      {/* Environment for realistic reflections */}
      <Environment preset="city" environmentIntensity={0.7} />

      {/* Game board scene */}
      <GameBoardScene {...props} />

      {/* Card overlays */}
      <PreviewCardOverlay renderHtmlCard={props.renderHtmlCard} getCardId={props.getCardId} />
      <SelectedCardOverlay renderHtmlCard={props.renderHtmlCard} getCardId={props.getCardId} />
      <ExpandedPileOverlay renderHtmlCard={props.renderHtmlCard} getCardId={props.getCardId} />
    </>
  )
}

export default function GameBoard<T>(props: GameBoardProps<T>) {
  const [previewCard, setPreviewCard] = useState<T | null>(null)
  const [selectedCard, setSelectedCard] = useState<T | null>(null)
  const [expandedPile, setExpandedPile] = useState<ExpandedPile<T> | null>(null)

  const dialogContent = props.renderDialog?.()
  const messageContent = props.renderMessage?.()
  const bottomBarContent = props.renderBottomBar?.()

  return (
    <CardContext.Provider value={{ previewCard, setPreviewCard, selectedCard, setSelectedCard: (card: T) => props.onCardClick?.(card, 'hand'), expandedPile, setExpandedPile }}>
      <div style={{ position: 'relative', width: '100dvw', height: '100dvh', userSelect: 'none', WebkitUserSelect: 'none' }}>
        <Canvas
          shadows
          dpr={[1, 2]}
          camera={{ position: [0, -8, 12], fov: 50, near: 0.1, far: 100 }}
          gl={{ antialias: true }}
          onCreated={({ gl }) => gl.setClearColor('#1a1a2e')}
        >
          <GameBoardCanvas {...props} />
        </Canvas>
        {messageContent && (
          <div
            style={{
              position: 'absolute',
              top: 0,
              left: 0,
              right: 0,
              display: 'flex',
              justifyContent: 'center',
              pointerEvents: 'none',
            }}
          >
            <div style={{ pointerEvents: 'auto' }}>
              {messageContent}
            </div>
          </div>
        )}
        {bottomBarContent && (
          <div
            style={{
              position: 'absolute',
              bottom: 0,
              left: 0,
              right: 0,
              pointerEvents: 'auto',
            }}
          >
            {bottomBarContent}
          </div>
        )}
        {dialogContent && (
          <div
            style={{
              position: 'absolute',
              top: 0,
              left: 0,
              right: 0,
              bottom: 0,
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              pointerEvents: 'none',
            }}
          >
            {dialogContent}
          </div>
        )}
      </div>
    </CardContext.Provider>
  )
}
