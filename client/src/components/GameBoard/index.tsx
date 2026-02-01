"use client";

import React, { useState, useCallback, useMemo, useRef, useEffect } from "react";
import { Canvas, useThree, ThreeEvent, useFrame } from "@react-three/fiber";
import { RoundedBox, Text, Html, Environment, Billboard } from "@react-three/drei";
import * as THREE from "three";
import useCardContext, { CardContext, DragState } from "./useCardContext";
import useCardInteraction from "./useCardInteraction";
import { useDebugRerender } from "../../hooks/useDebugRerender";
import { useTextureWithPlaceholder } from "./useTextureWithPlaceholder";
import { CardPositionsContext, useCardPositions, useCardPositionsProvider } from "./useCardPositions";
import TargetArrow3D from "./TargetArrow3D";

// Create a procedural felt texture for the game board
// eslint-disable-next-line react-compiler/react-compiler
function useBoardTextures() {
  "use no memo";
  return useMemo(() => {
    const size = 1024;
    const canvas = document.createElement("canvas");
    canvas.width = size;
    canvas.height = size;
    const ctx = canvas.getContext("2d");

    // Fallback if canvas context is not available - create solid color textures
    if (!ctx) {
      // Create a simple solid color canvas for fallback
      const fallbackCanvas = document.createElement("canvas");
      fallbackCanvas.width = 16;
      fallbackCanvas.height = 16;
      const fallbackCtx = fallbackCanvas.getContext("2d");
      if (fallbackCtx) {
        fallbackCtx.fillStyle = "#1a1a2e";
        fallbackCtx.fillRect(0, 0, 16, 16);
      }
      const colorTexture = new THREE.CanvasTexture(fallbackCanvas);
      colorTexture.needsUpdate = true;

      const roughnessCanvas = document.createElement("canvas");
      roughnessCanvas.width = 16;
      roughnessCanvas.height = 16;
      const roughnessCtx = roughnessCanvas.getContext("2d");
      if (roughnessCtx) {
        roughnessCtx.fillStyle = "#b0b0b0";
        roughnessCtx.fillRect(0, 0, 16, 16);
      }
      const roughnessTexture = new THREE.CanvasTexture(roughnessCanvas);
      roughnessTexture.needsUpdate = true;

      const normalCanvas = document.createElement("canvas");
      normalCanvas.width = 16;
      normalCanvas.height = 16;
      const normalCtx = normalCanvas.getContext("2d");
      if (normalCtx) {
        normalCtx.fillStyle = "#8080ff";
        normalCtx.fillRect(0, 0, 16, 16);
      }
      const normalTexture = new THREE.CanvasTexture(normalCanvas);
      normalTexture.needsUpdate = true;

      return { colorTexture, roughnessTexture, normalTexture };
    }

    // Base color - rich dark blue-green felt
    const gradient = ctx.createRadialGradient(
      size / 2,
      size / 2,
      0,
      size / 2,
      size / 2,
      size * 0.7
    );
    gradient.addColorStop(0, "#1e2a3a");
    gradient.addColorStop(0.5, "#1a1a2f");
    gradient.addColorStop(1, "#151525");
    ctx.fillStyle = gradient;
    ctx.fillRect(0, 0, size, size);

    // Add fine noise/grain for felt fiber texture
    for (let i = 0; i < 40000; i++) {
      const x = Math.random() * size;
      const y = Math.random() * size;
      const brightness = Math.random() * 0.2;
      const hue = 200 + Math.random() * 40; // Blue-ish hue variation
      const sat = 30 + Math.random() * 20;
      const light = 15 + brightness * 25;
      ctx.fillStyle = `hsla(${hue}, ${sat}%, ${light}%, ${0.2 + Math.random() * 0.4})`;
      ctx.fillRect(x, y, 1 + Math.random() * 1.5, 1 + Math.random() * 1.5);
    }

    // Add diagonal weave pattern for felt texture
    ctx.strokeStyle = "rgba(25, 35, 55, 0.2)";
    ctx.lineWidth = 1;
    for (let i = -size; i < size * 2; i += 6) {
      ctx.beginPath();
      ctx.moveTo(i, 0);
      ctx.lineTo(i + size, size);
      ctx.stroke();
    }
    ctx.strokeStyle = "rgba(35, 25, 45, 0.15)";
    for (let i = -size; i < size * 2; i += 6) {
      ctx.beginPath();
      ctx.moveTo(i + size, 0);
      ctx.lineTo(i, size);
      ctx.stroke();
    }

    // Add subtle wear patterns / lighter patches
    for (let i = 0; i < 30; i++) {
      const x = Math.random() * size;
      const y = Math.random() * size;
      const radius = 40 + Math.random() * 80;
      const patchGradient = ctx.createRadialGradient(x, y, 0, x, y, radius);
      patchGradient.addColorStop(0, "rgba(50, 55, 80, 0.12)");
      patchGradient.addColorStop(0.5, "rgba(45, 50, 75, 0.06)");
      patchGradient.addColorStop(1, "rgba(40, 45, 70, 0)");
      ctx.fillStyle = patchGradient;
      ctx.fillRect(x - radius, y - radius, radius * 2, radius * 2);
    }

    // Add darker worn spots
    for (let i = 0; i < 15; i++) {
      const x = Math.random() * size;
      const y = Math.random() * size;
      const radius = 20 + Math.random() * 40;
      const darkGradient = ctx.createRadialGradient(x, y, 0, x, y, radius);
      darkGradient.addColorStop(0, "rgba(10, 10, 20, 0.15)");
      darkGradient.addColorStop(1, "rgba(10, 10, 20, 0)");
      ctx.fillStyle = darkGradient;
      ctx.fillRect(x - radius, y - radius, radius * 2, radius * 2);
    }

    // Add subtle wood grain border effect at edges
    const edgeGradient = ctx.createLinearGradient(0, 0, size, 0);
    edgeGradient.addColorStop(0, "rgba(60, 45, 30, 0.1)");
    edgeGradient.addColorStop(0.05, "rgba(60, 45, 30, 0)");
    edgeGradient.addColorStop(0.95, "rgba(60, 45, 30, 0)");
    edgeGradient.addColorStop(1, "rgba(60, 45, 30, 0.1)");
    ctx.fillStyle = edgeGradient;
    ctx.fillRect(0, 0, size, size);

    const colorTexture = new THREE.CanvasTexture(canvas);
    colorTexture.wrapS = THREE.RepeatWrapping;
    colorTexture.wrapT = THREE.RepeatWrapping;
    colorTexture.repeat.set(3, 3);
    colorTexture.needsUpdate = true;

    // Create a roughness map for specular variation
    const roughnessCanvas = document.createElement("canvas");
    roughnessCanvas.width = 512;
    roughnessCanvas.height = 512;
    const roughnessCtx = roughnessCanvas.getContext("2d");

    let roughnessTexture: THREE.Texture;
    if (roughnessCtx) {
      // Base roughness (felt is fairly rough)
      roughnessCtx.fillStyle = "#b0b0b0"; // ~0.7 roughness
      roughnessCtx.fillRect(0, 0, 512, 512);

      // Add variation for worn/polished spots (lower roughness = more specular)
      for (let i = 0; i < 50; i++) {
        const x = Math.random() * 512;
        const y = Math.random() * 512;
        const radius = 20 + Math.random() * 60;
        const roughnessGradient = roughnessCtx.createRadialGradient(x, y, 0, x, y, radius);
        const variation = Math.random() > 0.5 ? 0.85 : 0.65; // Vary between rougher and smoother
        const colorVal = Math.floor(variation * 255);
        roughnessGradient.addColorStop(0, `rgb(${colorVal}, ${colorVal}, ${colorVal})`);
        roughnessGradient.addColorStop(1, "#b0b0b0");
        roughnessCtx.fillStyle = roughnessGradient;
        roughnessCtx.fillRect(x - radius, y - radius, radius * 2, radius * 2);
      }

      // Add fine noise to roughness
      for (let i = 0; i < 8000; i++) {
        const x = Math.random() * 512;
        const y = Math.random() * 512;
        const val = 140 + Math.random() * 80;
        roughnessCtx.fillStyle = `rgb(${val}, ${val}, ${val})`;
        roughnessCtx.fillRect(x, y, 2, 2);
      }

      roughnessTexture = new THREE.CanvasTexture(roughnessCanvas);
      roughnessTexture.wrapS = THREE.RepeatWrapping;
      roughnessTexture.wrapT = THREE.RepeatWrapping;
      roughnessTexture.repeat.set(3, 3);
      roughnessTexture.needsUpdate = true;
    } else {
      roughnessTexture = new THREE.Texture();
      roughnessTexture.needsUpdate = true;
    }

    // Create a normal map for surface detail
    const normalCanvas = document.createElement("canvas");
    normalCanvas.width = 256;
    normalCanvas.height = 256;
    const normalCtx = normalCanvas.getContext("2d");

    let normalTexture: THREE.Texture;
    if (normalCtx) {
      // Neutral normal (pointing up)
      normalCtx.fillStyle = "#8080ff";
      normalCtx.fillRect(0, 0, 256, 256);

      // Add subtle bumps for felt texture
      for (let i = 0; i < 3000; i++) {
        const x = Math.random() * 256;
        const y = Math.random() * 256;
        const nx = 128 + (Math.random() - 0.5) * 30;
        const ny = 128 + (Math.random() - 0.5) * 30;
        normalCtx.fillStyle = `rgb(${nx}, ${ny}, 255)`;
        normalCtx.fillRect(x, y, 1 + Math.random(), 1 + Math.random());
      }

      normalTexture = new THREE.CanvasTexture(normalCanvas);
      normalTexture.wrapS = THREE.RepeatWrapping;
      normalTexture.wrapT = THREE.RepeatWrapping;
      normalTexture.repeat.set(6, 6);
      normalTexture.needsUpdate = true;
    } else {
      normalTexture = new THREE.Texture();
      normalTexture.needsUpdate = true;
    }

    return { colorTexture, roughnessTexture, normalTexture };
  }, []);
}

// Table surface with divider
function TableSurface() {
  const { setPreviewCard } = useCardContext();
  const { colorTexture, roughnessTexture, normalTexture } = useBoardTextures();

  return (
    <group>
      {/* Table surface with enhanced materials */}
      <mesh position={[0, 0, -0.2]} receiveShadow onPointerOver={() => setPreviewCard(null)}>
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
  );
}

// Card dimensions (roughly standard card proportions)
const CARD_WIDTH = 0.7;
const CARD_HEIGHT = 1;
const CARD_DEPTH = 0.02;

const COLORS = {
  cardFront: "#e8e0d5",
  cardBack: "#2a4858",
  cardBorder: "#1a1a1a",
  deck: "#1e3a4c",
  graveyard: "#3d2c3d",
  exile: "#2c2c3d",
  battlefield: "#2d5a3d",
  hand: "#4a3728",
};

// Expanded pile data
export type ExpandedPile<T> = {
  type: "deck" | "graveyard" | "exile" | "prize";
  cards: T[];
  title: string;
};

// Stack effect type
export type StackEffect<T> = {
  card: T;
  targetCardIds: string[];
};

// Hook to get layout info based on viewport
function useLayout() {
  const { viewport } = useThree();
  const isPortrait = viewport.height > viewport.width;
  const aspect = viewport.width / viewport.height;

  // Scale factor based on viewport size
  const baseScale = isPortrait ? Math.min(1, viewport.width / 5) : Math.min(1, viewport.width / 12);

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
  };
}

// Wrapper component that adds interaction to rendered cards
type InteractiveCardProps<T> = {
  card: T;
  cardId?: string;
  interactive: boolean;
  renderCardMesh: (card: T) => React.ReactNode;
};

function InteractiveCard<T>({ card, cardId, interactive, renderCardMesh }: InteractiveCardProps<T>) {
  const { isHovered, ...interactions } = useCardInteraction<T>(interactive ? card : null);
  const { dragState } = useCardContext();
  const { registerPosition, unregisterPosition } = useCardPositions();
  const groupRef = useRef<THREE.Group>(null);
  const worldPosition = useRef(new THREE.Vector3());

  // Check if this card is being dragged
  const isBeingDragged = dragState?.card === card;

  // Unregister position on unmount
  useEffect(() => {
    return () => {
      if (cardId) {
        unregisterPosition(cardId);
      }
    };
  }, [cardId, unregisterPosition]);

  // Animate hover effect - small zoom towards camera
  useFrame((_, delta) => {
    if (!groupRef.current) return undefined;

    // Register world position for target arrows
    if (cardId) {
      groupRef.current.getWorldPosition(worldPosition.current);
      registerPosition(cardId, worldPosition.current);
    }

    // Hide card while dragging
    if (isBeingDragged) {
      groupRef.current.scale.setScalar(0);
      return undefined;
    }

    const targetZ = isHovered ? 0.15 : 0;
    const targetScale = isHovered ? 1.08 : 1;

    // Smooth lerp towards target values
    const lerpSpeed = 12 * delta;
    groupRef.current.position.z += (targetZ - groupRef.current.position.z) * lerpSpeed;
    groupRef.current.scale.x += (targetScale - groupRef.current.scale.x) * lerpSpeed;
    groupRef.current.scale.y += (targetScale - groupRef.current.scale.y) * lerpSpeed;
    groupRef.current.scale.z += (targetScale - groupRef.current.scale.z) * lerpSpeed;
  });

  return (
    <group ref={groupRef} {...(interactive ? interactions : {})}>
      {renderCardMesh(card)}
    </group>
  );
}

type DeckProps<T> = {
  cardCount?: number;
  position?: [number, number, number];
  scale?: number;
  cards?: T[];
  label?: string;
  renderCardMesh: (card: T) => React.ReactNode;
  onClick?: () => void;
};

function Deck<T>({
  cardCount = 30,
  position = [0, 0, 0],
  scale = 1,
  cards = [],
  renderCardMesh,
  onClick,
}: DeckProps<T>) {
  const stackHeight = Math.min(cardCount, 30) * 0.01;

  const handleClick = useCallback(
    (e: ThreeEvent<MouseEvent>) => {
      e.stopPropagation();
      onClick?.();
    },
    [onClick]
  );

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
        {cards.length > 0 && renderCardMesh(cards[0]!)}
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
  );
}

type GraveyardProps<T> = {
  cardCount?: number;
  position?: [number, number, number];
  scale?: number;
  cards?: T[];
  label?: string;
  isOpponent?: boolean;
  renderCardMesh: (card: T) => React.ReactNode;
  renderEmptySlot: () => React.ReactNode;
  onClick?: () => void;
  canDropToZone?: (card: T, zone: DropZone) => boolean;
};

function Graveyard<T>({
  cardCount = 5,
  position = [0, 0, 0],
  scale = 1,
  cards = [],
  label = "Graveyard",
  isOpponent = false,
  renderCardMesh,
  renderEmptySlot,
  onClick,
  canDropToZone,
}: GraveyardProps<T>) {
  const stackHeight = Math.min(cardCount, 20) * 0.008;
  const topCard = cards.length > 0 ? cards[cards.length - 1] : null;

  const handleClick = useCallback(
    (e: ThreeEvent<MouseEvent>) => {
      e.stopPropagation();
      onClick?.();
    },
    [onClick]
  );

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
            {topCard && renderCardMesh(topCard)}
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
      {/* Dropzone overlay (player side only) */}
      {!isOpponent && (
        <DropzoneBox
          width={CARD_WIDTH * 1.5}
          height={CARD_HEIGHT * 1.5}
          zone="graveyard"
          canDropToZone={canDropToZone}
        />
      )}
    </group>
  );
}

type ExileProps<T> = {
  cardCount?: number;
  position?: [number, number, number];
  scale?: number;
  cards?: T[];
  label?: string;
  renderCardMesh: (card: T) => React.ReactNode;
  renderEmptySlot: () => React.ReactNode;
  onClick?: () => void;
};

function Exile<T>({
  cardCount = 0,
  position = [0, 0, 0],
  scale = 1,
  cards = [],
  label = "Exile",
  renderCardMesh,
  renderEmptySlot,
  onClick,
}: ExileProps<T>) {
  const stackHeight = Math.min(cardCount, 20) * 0.008;
  const topCard = cards.length > 0 ? cards[cards.length - 1] : null;

  const handleClick = useCallback(
    (e: ThreeEvent<MouseEvent>) => {
      e.stopPropagation();
      onClick?.();
    },
    [onClick]
  );

  if (cardCount === 0) return null;

  return (
    <group position={position} scale={scale} onClick={handleClick}>
      {/* Exile zone marker */}
      <mesh position={[0, 0, -0.01]} rotation={[-Math.PI / 2, 0, 0]}>
        <planeGeometry args={[CARD_WIDTH + 0.1, CARD_HEIGHT + 0.1]} />
        <meshStandardMaterial color={COLORS.exile} transparent opacity={0.5} />
      </mesh>
      {/* Stack of cards */}
      <RoundedBox
        args={[CARD_WIDTH, CARD_HEIGHT, stackHeight]}
        radius={0.03}
        smoothness={4}
        position={[0, 0, stackHeight / 2]}
      >
        <meshStandardMaterial color={COLORS.exile} />
      </RoundedBox>
      {/* Top card (face up in exile) */}
      <group position={[0, 0, stackHeight + CARD_DEPTH / 2]}>
        {topCard && renderCardMesh(topCard)}
        {!topCard && renderEmptySlot()}
      </group>
      {label && (
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
  );
}

type PrizeCardsProps<T> = {
  cards?: T[];
  position?: [number, number, number];
  scale?: number;
  renderCardMesh: (card: T) => React.ReactNode;
  onClick?: () => void;
};

function PrizeCards<T>({
  cards = [],
  position = [0, 0, 0],
  scale = 1,
  renderCardMesh,
  onClick,
}: PrizeCardsProps<T>) {
  const cardSpacing = CARD_HEIGHT * 0.25; // Tight vertical spacing for prize cards
  const totalHeight = (cards.length - 1) * cardSpacing;

  const handleClick = useCallback(
    (e: ThreeEvent<MouseEvent>) => {
      e.stopPropagation();
      onClick?.();
    },
    [onClick]
  );

  if (cards.length === 0) return null;

  return (
    <group position={position} scale={scale} onClick={handleClick}>
      {/* Cards arranged vertically with overlap */}
      {cards.map((card, index) => {
        const yOffset = index * cardSpacing - totalHeight / 2;
        const zOffset = index * 0.02; // Slight z-stacking

        return (
          <group key={index} position={[0, yOffset, zOffset]} scale={0.6}>
            {renderCardMesh(card)}
          </group>
        );
      })}
    </group>
  );
}

type SideHandProps<T> = {
  cards?: T[];
  position?: [number, number, number];
  scale?: number;
  renderCardMesh: (card: T) => React.ReactNode;
  getCardId: (card: T) => string | number;
  onClick?: () => void;
};

function SideHand<T>({
  cards = [],
  position = [0, 0, 0],
  scale = 1,
  renderCardMesh,
  getCardId,
  onClick,
}: SideHandProps<T>) {
  const cardSpacing = CARD_WIDTH * 0.5; // Horizontal spacing with overlap
  const totalWidth = (cards.length - 1) * cardSpacing;

  const handleClick = useCallback(
    (e: ThreeEvent<MouseEvent>) => {
      e.stopPropagation();
      onClick?.();
    },
    [onClick]
  );

  if (cards.length === 0) return null;

  return (
    <group position={position} scale={scale} onClick={handleClick}>
      {/* Cards arranged horizontally, standing upright */}
      {cards.map((card, index) => {
        const xOffset = index * cardSpacing - totalWidth / 2;
        const zOffset = index * 0.02; // Slight z-stacking

        return (
          <group key={getCardId(card)} position={[xOffset, 0, zOffset]} scale={0.6}>
            {renderCardMesh(card)}
          </group>
        );
      })}
    </group>
  );
}

type HandProps<T> = {
  cards?: T[];
  isOpponent?: boolean;
  position?: [number, number, number];
  spreadAngle?: number;
  spreadRadius?: number;
  renderCardMesh: (card: T, faceDown: boolean) => React.ReactNode;
  getCardId: (card: T) => string | number;
  onClick?: () => void;
};

function Hand<T>({
  cards = [],
  isOpponent = false,
  position = [0, 0, 0],
  spreadAngle = 12,
  spreadRadius = 3,
  renderCardMesh,
  getCardId,
  onClick,
}: HandProps<T>) {
  const totalSpread = (cards.length - 1) * spreadAngle;
  const startAngle = -totalSpread / 2; // Start from left

  const handleClick = useCallback(
    (e: ThreeEvent<MouseEvent>) => {
      e.stopPropagation();
      onClick?.();
    },
    [onClick]
  );

  return (
    <group position={position} onClick={onClick ? handleClick : undefined}>
      {cards.map((card, index) => {
        // Angle for this card in the fan (in radians)
        const angleDeg = startAngle + index * spreadAngle;
        const angleRad = angleDeg * (Math.PI / 180);

        // Position cards in an arc
        const fanDirection = isOpponent ? -1 : 1;
        const xOffset = Math.sin(angleRad) * spreadRadius;
        const yOffset = -5 * (1 - Math.cos(angleRad)) * 0.5 * fanDirection;
        const zOffset = (cards.length - Math.abs(index - (cards.length - 1) / 2)) * 0.03;
        const xRotation = 0.7;
        const zRotation = -angleRad * fanDirection;

        return (
          <group
            key={getCardId(card)}
            position={[xOffset, yOffset, zOffset]}
            rotation={[xRotation, 0, zRotation]}
          >
            <InteractiveCard
              card={card}
              cardId={String(getCardId(card))}
              interactive={!isOpponent}
              renderCardMesh={(card) => renderCardMesh(card, false)}
            />
          </group>
        );
      })}
    </group>
  );
}

type BattlefieldProps<T> = {
  cards?: T[];
  position?: [number, number, number];
  isOpponent?: boolean;
  maxSlots?: number;
  renderCardMesh: (card: T) => React.ReactNode;
  getCardId: (card: T) => string | number;
  canDropToZone?: (card: T, zone: DropZone) => boolean;
};

type DropzoneBoxProps<T> = {
  width: number;
  height?: number;
  depth?: number;
  position?: [number, number, number];
  color?: string;
  hoverColor?: string;
  zone: DropZone;
  canDropToZone?: (card: T, zone: DropZone) => boolean;
};

function DropzoneBox<T>({
  width,
  height = 2.4,
  depth = 0.05,
  position = [0, 0, 0.5],
  color = "#ff4444",
  hoverColor = "#44ff44",
  zone,
  canDropToZone,
}: DropzoneBoxProps<T>) {
  const [isHovered, setIsHovered] = useState(false);
  const { dragState, setHoveredDropZone } = useCardContext();
  const isDragging = dragState !== null;
  const canDrop = isDragging && canDropToZone && canDropToZone(dragState?.card as T, zone);
  const isActive = canDrop && isHovered;

  return (
    <mesh
      position={position}
      onPointerEnter={(e) => {
        if (!canDrop) return undefined;
        e.stopPropagation();
        setIsHovered(true);
        setHoveredDropZone(zone);
      }}
      onPointerLeave={() => {
        setIsHovered(false);
        setHoveredDropZone(null);
      }}
    >
      <boxGeometry args={[width, height, depth]} />
      <meshStandardMaterial
        color={isActive ? hoverColor : color}
        transparent
        opacity={isActive ? 0.2 : 0}
      />
    </mesh>
  );
}

function Battlefield<T>({
  cards = [],
  position = [0, 0, 0],
  isOpponent = false,
  maxSlots = 7,
  renderCardMesh,
  getCardId,
  canDropToZone,
}: BattlefieldProps<T>) {
  const slotWidth = CARD_WIDTH + 0.15;
  const { isPortrait } = useLayout();
  const dropzoneY = isPortrait ? 0.5 : 0;

  return (
    <group position={position}>
      {/* Battlefield zone */}
      <mesh rotation={[-Math.PI / 2, 0, 0]} position={[0, 0, -0.02]}>
        <planeGeometry args={[slotWidth * maxSlots, CARD_HEIGHT + 0.3]} />
        <meshStandardMaterial color={COLORS.battlefield} transparent opacity={0.3} />
      </mesh>
      {/* Dropzone overlay (player side only) */}
      {!isOpponent && (
        <DropzoneBox
          width={slotWidth * maxSlots}
          position={[0, dropzoneY, 0.5]}
          zone="battlefield"
          canDropToZone={canDropToZone}
        />
      )}
      {/* Card slots */}
      {Array.from({ length: maxSlots }).map((_, index) => {
        const xPos = (index - (maxSlots - 1) / 2) * slotWidth;
        const card = cards[index];

        return (
          <group key={card ? getCardId(card) : `slot-${index}`} position={[xPos, 0, 0]}>
            {/* Slot outline */}
            <mesh rotation={[-Math.PI / 2, 0, 0]} position={[0, 0, -0.01]}>
              <planeGeometry args={[CARD_WIDTH + 0.08, CARD_HEIGHT + 0.08]} />
              <meshStandardMaterial color="#1a3d1a" transparent opacity={0.5} />
            </mesh>
            {/* Card if present */}
            {card && (
              <group position={[0, 0, CARD_DEPTH / 2]}>
                <InteractiveCard
                  card={card}
                  cardId={String(getCardId(card))}
                  interactive={true}
                  renderCardMesh={renderCardMesh}
                />
              </group>
            )}
          </group>
        );
      })}
    </group>
  );
}

type StackDisplayProps<T> = {
  stack: StackEffect<T>[];
  position?: [number, number, number];
  scale?: number;
  renderCardMesh: (card: T) => React.ReactNode;
  getCardId: (card: T) => string | number;
  renderButton?: () => React.ReactNode;
};

function StackDisplay<T>({
  stack,
  position = [0, 0, 0],
  scale = 1,
  renderCardMesh,
  getCardId,
  renderButton,
}: StackDisplayProps<T>) {
  if (stack.length === 0) return null;

  return (
    <Billboard position={position} scale={scale}>
      {/* Stack label */}
      <Text
        position={[0, CARD_HEIGHT + 1.3, 0.1]}
        fontSize={0.12}
        color="#ddd"
        anchorX="center"
        anchorY="middle"
        scale={2}
      >
        Stack ({stack.length})
      </Text>

      {/* Cards in stack - fanned like a hand, facing camera */}
      {stack.map((effect, index) => {
        const spreadAngle = 12;
        const spreadRadius = 2.5;
        const totalSpread = (stack.length - 1) * spreadAngle;
        const startAngle = -totalSpread / 2;

        const angleDeg = startAngle + index * spreadAngle;
        const angleRad = angleDeg * (Math.PI / 180);

        const xOffset = Math.sin(angleRad) * spreadRadius;
        const yOffset = -(1 - Math.cos(angleRad)) * 2;
        const zOffset = (stack.length - Math.abs(index - (stack.length - 1) / 2)) * 0.03;

        return (
          <group
            key={getCardId(effect.card)}
            position={[xOffset, yOffset + CARD_HEIGHT, zOffset]}
            scale={2}
            rotation={[0, 0, angleRad]}
          >
            <InteractiveCard
              card={effect.card}
              cardId={String(getCardId(effect.card))}
              interactive={true}
              renderCardMesh={renderCardMesh}
            />
          </group>
        );
      })}

      {/* Stack button */}
      {renderButton && (
        <Html position={[0, -CARD_HEIGHT / 2 - 0.25, 0.1]} center style={{ pointerEvents: "auto" }}>
          {renderButton()}
        </Html>
      )}
    </Billboard>
  );
}

type PlayerAvatarProps = {
  health: number;
  isOpponent?: boolean;
  position?: [number, number, number];
  scale?: number;
  avatarSrc?: string;
};

// Shader for rounded corners on avatar
const roundedAvatarShader = {
  vertexShader: `
    varying vec2 vUv;
    void main() {
      vUv = uv;
      gl_Position = projectionMatrix * modelViewMatrix * vec4(position, 1.0);
    }
  `,
  fragmentShader: `
    uniform sampler2D map;
    uniform float radius;
    varying vec2 vUv;

    float roundedBoxSDF(vec2 p, vec2 b, float r) {
      vec2 q = abs(p) - b + r;
      return length(max(q, 0.0)) - r;
    }

    void main() {
      vec2 uv = vUv - 0.5;
      float d = roundedBoxSDF(uv, vec2(0.5), radius);
      if (d > 0.0) discard;

      vec4 texColor = texture2D(map, vUv);
      gl_FragColor = texColor;
    }
  `,
};

// Inner component that loads and displays the avatar texture with rounded corners
function AvatarImage({ src }: { src: string }) {
  const texture = useTextureWithPlaceholder(src);
  const materialRef = useRef<THREE.ShaderMaterial>(null);

  // Create uniforms once
  const uniforms = useMemo(
    () => ({
      map: { value: texture },
      radius: { value: 0.15 },
    }),
    [texture]
  );

  // Update texture uniform when it changes
  useEffect(() => {
    if (materialRef.current) {
      materialRef.current.uniforms.map.value = texture;
      materialRef.current.needsUpdate = true;
    }
  }, [texture]);

  return (
    <mesh position={[0, 0.15, 0.08]}>
      <planeGeometry args={[1.0, 1.0]} />
      <shaderMaterial
        ref={materialRef}
        attach="material"
        uniforms={uniforms}
        vertexShader={roundedAvatarShader.vertexShader}
        fragmentShader={roundedAvatarShader.fragmentShader}
        transparent
      />
    </mesh>
  );
}

function PlayerAvatar({
  health,
  isOpponent = false,
  position = [0, 0, 0],
  scale = 1,
  avatarSrc,
}: PlayerAvatarProps) {
  const primaryColor = isOpponent ? "#c44" : "#48c";
  const secondaryColor = isOpponent ? "#822" : "#269";
  const frameColor = isOpponent ? "#fca" : "#adf";

  return (
    <group position={position} scale={scale}>
      {/* Main portrait - upright facing camera */}
      <group>
        {/* Frame */}
        <RoundedBox args={[1.4, 1.6, 0.1]} radius={0.1} smoothness={4}>
          <meshStandardMaterial color={frameColor} />
        </RoundedBox>

        {/* Portrait background */}
        <RoundedBox args={[1.2, 1.4, 0.12]} radius={0.08} smoothness={4} position={[0, 0.05, 0]}>
          <meshStandardMaterial color={primaryColor} />
        </RoundedBox>

        {/* Avatar image or fallback color */}
        {avatarSrc ? (
          <AvatarImage src={avatarSrc} />
        ) : (
          <RoundedBox args={[0.9, 0.9, 0.14]} radius={0.1} smoothness={4} position={[0, 0.15, 0]}>
            <meshStandardMaterial color={secondaryColor} />
          </RoundedBox>
        )}

        {/* Health display at bottom */}
        <group position={[0, -0.55, 0.1]}>
          {/* Health background */}
          <RoundedBox args={[0.7, 0.4, 0.08]} radius={0.05} smoothness={4}>
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
  );
}

// Step indicator type
export type Step = {
  id: string;
  label: string;
  isActive: boolean;
};

type StepIndicatorProps = {
  steps: Step[];
  position?: [number, number, number];
  scale?: number;
  isOpponentTurn?: boolean;
};

function StepIndicator({
  steps,
  position = [0, 0, 0],
  scale = 1,
  isOpponentTurn = false,
}: StepIndicatorProps) {
  const stepSize = 0.3;
  const stepGap = 0.15;
  const totalWidth = steps.length * stepSize + (steps.length - 1) * stepGap;
  const activeStep = steps.find((step) => step.isActive);

  // Colors change based on whose turn it is
  const activeColor = isOpponentTurn ? "#c44" : "#48c";
  const activeFrameColor = isOpponentTurn ? "#faa" : "#adf";
  const activeEmissive = isOpponentTurn ? "#c44" : "#48c";

  return (
    <group position={position} scale={scale}>
      {steps.map((step, index) => {
        const xOffset = index * (stepSize + stepGap) - totalWidth / 2 + stepSize / 2;
        const inactiveColor = "#3a3a4a";
        const frameColor = step.isActive ? activeFrameColor : "#2a2a3a";

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
                emissive={step.isActive ? activeEmissive : "#000"}
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
              <meshStandardMaterial color={step.isActive ? activeColor : inactiveColor} />
            </RoundedBox>
          </group>
        );
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
  );
}

// Mana sphere with translucent shell and billboard icon inside
function ManaSphere({
  src,
  position,
  visible = true,
  delay = 0,
}: {
  src: string;
  position: [number, number, number];
  visible?: boolean;
  delay?: number;
}) {
  const texture = useTextureWithPlaceholder(src);
  const groupRef = useRef<THREE.Group>(null);
  const animationState = useRef({
    currentScale: 0,
    targetScale: visible ? 1 : 0,
    delayRemaining: delay,
  });

  // Update target when visible changes
  React.useEffect(() => {
    animationState.current.targetScale = visible ? 1 : 0;
    if (visible) {
      animationState.current.delayRemaining = delay;
    }
  }, [visible, delay]);

  useFrame((_, delta) => {
    if (!groupRef.current) return undefined;

    const state = animationState.current;

    // Handle delay
    if (state.delayRemaining > 0) {
      state.delayRemaining -= delta;
      return undefined;
    }

    // Spring animation towards target
    const diff = state.targetScale - state.currentScale;
    if (Math.abs(diff) > 0.001) {
      // Spring physics: faster when far, with slight overshoot
      const spring = 8;
      const damping = 0.8;
      state.currentScale += diff * spring * delta * damping;

      // Add slight bounce overshoot when appearing
      if (state.targetScale === 1 && state.currentScale > 0.95 && state.currentScale < 1.05) {
        state.currentScale = Math.min(state.currentScale, 1.08);
      }

      groupRef.current.scale.setScalar(Math.max(0, state.currentScale));
    } else {
      state.currentScale = state.targetScale;
      groupRef.current.scale.setScalar(state.targetScale);
    }
  });

  return (
    <group ref={groupRef} position={position} scale={0}>
      {/* Translucent outer sphere */}
      <mesh renderOrder={1}>
        <sphereGeometry args={[0.15, 32, 32]} />
        <meshStandardMaterial
          color="#88ccff"
          transparent
          opacity={0.25}
          roughness={0.1}
          metalness={0.3}
          envMapIntensity={1.5}
          depthWrite={false}
        />
      </mesh>
      {/* Billboard icon inside */}
      <Billboard follow={true} lockX={false} lockY={false} lockZ={false}>
        <mesh renderOrder={2}>
          <planeGeometry args={[0.2, 0.2]} />
          <meshBasicMaterial map={texture} transparent depthTest={false} side={THREE.DoubleSide} />
        </mesh>
      </Billboard>
    </group>
  );
}

type ManaPoolProps = {
  textures: string[];
  position?: [number, number, number];
  scale?: number;
};

function ManaPool({ textures, position = [0, 0, 0], scale = 1 }: ManaPoolProps) {
  if (textures.length === 0) return null;

  // Spiral parameters
  const baseRadius = 0.15;
  const radiusGrowth = 0.08;
  const angleStep = Math.PI * 0.6; // Golden angle-ish for nice distribution
  const staggerDelay = 0.08; // Delay between each sphere appearing

  return (
    <group position={position} scale={scale}>
      {textures.map((src, index) => {
        const angle = index * angleStep;
        const radius = baseRadius + index * radiusGrowth;
        const xOffset = Math.cos(angle) * radius;
        const yOffset = Math.sin(angle) * radius;
        const zOffset = index * 0.05; // Slight z-stacking for depth

        return (
          <ManaSphere
            key={`${src}-${index}`}
            src={src}
            position={[xOffset, yOffset, zOffset]}
            visible={true}
            delay={index * staggerDelay}
          />
        );
      })}
    </group>
  );
}

type PlayerAreaProps<T> = {
  isOpponent?: boolean;
  deckCards?: T[];
  graveyardCards?: T[];
  exileCards?: T[];
  handCards?: T[];
  sideHandCards?: T[];
  battlefieldFrontRowCards?: T[];
  battlefieldBackRowCards?: T[];
  renderCardMesh: (card: T) => React.ReactNode;
  renderEmptySlot: () => React.ReactNode;
  getCardId: (card: T) => string | number;
  onDeckClick?: () => void;
  onGraveyardClick?: () => void;
  onExileClick?: () => void;
  onHandClick?: () => void;
  onSideHandClick?: () => void;
  canDropToZone?: (card: T, zone: DropZone) => boolean;
};

type DropZone = "battlefield" | "graveyard";

type GameBoardProps<T> = {
  yourHand: T[];
  yourSideHand?: T[];
  yourBattlefieldFrontRow: T[];
  yourBattlefieldBackRow: T[];
  yourDeck: T[];
  yourGraveyard: T[];
  yourExile: T[];
  yourPrizeCards: T[];
  yourHealth: number;
  yourAvatarSrc?: string;
  opponentHand: T[];
  opponentSideHand?: T[];
  opponentBattlefieldFrontRow: T[];
  opponentBattlefieldBackRow: T[];
  opponentDeck: T[];
  opponentGraveyard: T[];
  opponentsExile: T[];
  opponentsPrizeCards: T[];
  opponentHealth: number;
  opponentAvatarSrc?: string;
  renderHtmlCard: (card: T) => React.ReactNode;
  renderCardMesh: (card: T) => React.ReactNode;
  renderEmptySlot: () => React.ReactNode;
  getCardId: (card: T) => string | number;
  onCardClick?: (card: T | null) => void;
  onCardHover?: (card: T | null) => void;
  onDeckClick?: (isOpponent: boolean) => void;
  onGraveyardClick?: (isOpponent: boolean) => void;
  onExileClick?: (isOpponent: boolean) => void;
  onPrizeCardsClick?: (isOpponent: boolean) => void;
  onOpponentsHandClick?: () => void;
  onSideHandClick?: () => void;
  onOpponentSideHandClick?: () => void;
  onCardDragEnd?: (card: T, zone: string | null) => void;
  canDropToZone?: (card: T, zone: DropZone) => boolean;
  stack?: StackEffect<T>[] | null;
  stackButton?: () => React.ReactNode;
  steps?: Step[];
  isOpponentTurn?: boolean;
  manaPoolTextures?: string[];
  renderDialog?: () => React.ReactNode | null;
  renderMessage?: () => React.ReactNode | null;
  renderBottomBar?: () => React.ReactNode | null;
  renderOverlay?: () => React.ReactNode | null;
};

function PlayerArea<T>({
  isOpponent = false,
  deckCards = [],
  graveyardCards = [],
  exileCards = [],
  handCards = [],
  sideHandCards = [],
  battlefieldFrontRowCards = [],
  battlefieldBackRowCards = [],
  renderCardMesh,
  renderEmptySlot,
  getCardId,
  onDeckClick,
  onGraveyardClick,
  onExileClick,
  onHandClick,
  onSideHandClick,
  canDropToZone,
}: PlayerAreaProps<T>) {
  const layout = useLayout();
  const { isPortrait, scale } = layout;

  // Adjust positions based on orientation
  const handY = isOpponent ? (isPortrait ? 4.5 : 3.5) : isPortrait ? -4.5 : -3.5;
  const sideHandY = isOpponent ? (isPortrait ? 5.5 : 4.5) : isPortrait ? -5.5 : -4.5;

  // Front row is closer to center, back row is closer to the player's side
  const rowSpacing = isPortrait ? 1.2 : 1.4;
  const battlefieldCenterY = isOpponent ? (isPortrait ? 2 : 1.5) : isPortrait ? -2 : -1.5;
  const battlefieldFrontRowY = isOpponent
    ? battlefieldCenterY - rowSpacing / 2
    : battlefieldCenterY + rowSpacing / 2;
  const battlefieldBackRowY = isOpponent
    ? battlefieldCenterY + rowSpacing / 2
    : battlefieldCenterY - rowSpacing / 2;

  // In portrait: deck/graveyard go to the side but closer
  // In landscape: deck/graveyard go further to the side
  const sideX = isPortrait ? 2.5 : 5;
  const sideY = isOpponent ? (isPortrait ? 3.2 : 2.5) : isPortrait ? -3.2 : -2.5;

  const deckGraveyardScale = isPortrait ? 0.7 : 1;
  const playerLabel = isOpponent ? "Opponent's" : "Your";

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
        onClick={onHandClick}
      />

      {/* Side Hand (laid flat on table, below battlefield) */}
      {sideHandCards.length > 0 && (
        <SideHand
          cards={sideHandCards}
          position={[0, sideHandY, 0]}
          scale={deckGraveyardScale}
          renderCardMesh={renderCardMesh}
          getCardId={getCardId}
          onClick={onSideHandClick}
        />
      )}

      {/* Battlefield Front Row */}
      <Battlefield
        cards={battlefieldFrontRowCards}
        isOpponent={isOpponent}
        position={[0, battlefieldFrontRowY, 0]}
        maxSlots={layout.maxBattlefieldSlots}
        renderCardMesh={renderCardMesh}
        getCardId={getCardId}
        canDropToZone={canDropToZone}
      />

      {/* Battlefield Back Row */}
      <Battlefield
        cards={battlefieldBackRowCards}
        isOpponent={isOpponent}
        position={[0, battlefieldBackRowY, 0]}
        maxSlots={layout.maxBattlefieldSlots}
        renderCardMesh={renderCardMesh}
        getCardId={getCardId}
        canDropToZone={canDropToZone}
      />

      {/* Deck (on the right side from player's perspective) */}
      <Deck
        cardCount={deckCards.length}
        cards={deckCards as unknown as T[]}
        label={`${playerLabel} Deck`}
        position={[sideX, sideY, 0]}
        scale={deckGraveyardScale}
        renderCardMesh={renderCardMesh}
        onClick={onDeckClick}
      />

      {/* Exile (to the right of deck, only shown if cards present) */}
      <Exile
        cardCount={exileCards.length}
        cards={exileCards}
        label="Exile"
        position={[sideX + (isPortrait ? 0.9 : 1.2), sideY, 0]}
        scale={deckGraveyardScale}
        renderCardMesh={renderCardMesh}
        renderEmptySlot={renderEmptySlot}
        onClick={onExileClick}
      />

      {/* Graveyard (next to deck) */}
      <Graveyard
        cardCount={graveyardCards.length}
        cards={graveyardCards}
        label={`${playerLabel} Graveyard`}
        position={[sideX - (isPortrait ? 0.9 : 1.2), sideY, 0]}
        scale={deckGraveyardScale}
        isOpponent={isOpponent}
        renderCardMesh={renderCardMesh}
        renderEmptySlot={renderEmptySlot}
        onClick={onGraveyardClick}
        canDropToZone={canDropToZone}
      />
    </group>
  );
}

function GameBoardScene<T>({
  yourHand,
  yourSideHand,
  yourBattlefieldFrontRow,
  yourBattlefieldBackRow,
  yourDeck,
  yourGraveyard,
  yourExile,
  yourPrizeCards,
  yourHealth,
  yourAvatarSrc,
  opponentHand,
  opponentSideHand,
  opponentBattlefieldFrontRow,
  opponentBattlefieldBackRow,
  opponentDeck,
  opponentGraveyard,
  opponentsExile,
  opponentsPrizeCards,
  opponentHealth,
  opponentAvatarSrc,
  renderCardMesh,
  renderEmptySlot,
  getCardId,
  onDeckClick,
  onGraveyardClick,
  onExileClick,
  onPrizeCardsClick,
  onOpponentsHandClick,
  onSideHandClick,
  onOpponentSideHandClick,
  canDropToZone,
  stack,
  stackButton,
  steps,
  isOpponentTurn,
  manaPoolTextures,
}: GameBoardProps<T>) {
  const layout = useLayout();
  const { viewport, isPortrait, scale } = layout;

  return (
    <group>
      {/* Table surface with divider */}
      <TableSurface />

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
        exileCards={yourExile}
        handCards={yourHand}
        sideHandCards={yourSideHand}
        battlefieldFrontRowCards={yourBattlefieldFrontRow}
        battlefieldBackRowCards={yourBattlefieldBackRow}
        renderCardMesh={renderCardMesh}
        renderEmptySlot={renderEmptySlot}
        getCardId={getCardId}
        onDeckClick={onDeckClick ? () => onDeckClick(false) : undefined}
        onGraveyardClick={onGraveyardClick ? () => onGraveyardClick(false) : undefined}
        onExileClick={onExileClick ? () => onExileClick(false) : undefined}
        onSideHandClick={onSideHandClick}
        canDropToZone={canDropToZone}
      />

      {/* Opponent's area (top) */}
      <PlayerArea
        isOpponent={true}
        deckCards={opponentDeck}
        graveyardCards={opponentGraveyard}
        exileCards={opponentsExile}
        handCards={opponentHand}
        sideHandCards={opponentSideHand}
        battlefieldFrontRowCards={opponentBattlefieldFrontRow}
        battlefieldBackRowCards={opponentBattlefieldBackRow}
        renderCardMesh={renderCardMesh}
        renderEmptySlot={renderEmptySlot}
        getCardId={getCardId}
        onDeckClick={onDeckClick ? () => onDeckClick(true) : undefined}
        onGraveyardClick={onGraveyardClick ? () => onGraveyardClick(true) : undefined}
        onExileClick={onExileClick ? () => onExileClick(true) : undefined}
        onHandClick={onOpponentsHandClick}
        onSideHandClick={onOpponentSideHandClick}
      />

      {/* Player avatars with health - same vertical as deck/graveyard, on left side */}
      <PlayerAvatar
        health={yourHealth}
        isOpponent={false}
        position={[(isPortrait ? -2.5 : -5) * scale, (isPortrait ? -3.2 : -2.5) * scale, 1]}
        scale={scale * (isPortrait ? 0.7 : 0.85)}
        avatarSrc={yourAvatarSrc}
      />
      <PlayerAvatar
        health={opponentHealth}
        isOpponent={true}
        position={[(isPortrait ? -2.5 : -5) * scale, (isPortrait ? 3.2 : 2.5) * scale, 1]}
        scale={scale * (isPortrait ? 0.7 : 0.85)}
        avatarSrc={opponentAvatarSrc}
      />

      {/* Prize cards - below each player's avatar */}
      <PrizeCards
        cards={yourPrizeCards}
        position={[(isPortrait ? -2.5 : -5) * scale, (isPortrait ? -3.9 : -3.3) * scale, 0.5]}
        scale={scale * (isPortrait ? 0.7 : 0.85)}
        renderCardMesh={renderCardMesh}
        onClick={onPrizeCardsClick ? () => onPrizeCardsClick(false) : undefined}
      />
      <PrizeCards
        cards={opponentsPrizeCards}
        position={[(isPortrait ? -2.5 : -5) * scale, (isPortrait ? 3.9 : 3.3) * scale, 0.5]}
        scale={scale * (isPortrait ? 0.7 : 0.85)}
        renderCardMesh={renderCardMesh}
        onClick={onPrizeCardsClick ? () => onPrizeCardsClick(true) : undefined}
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

      {/* Mana pool in bottom-right corner */}
      {manaPoolTextures && manaPoolTextures.length > 0 && (
        <ManaPool
          textures={manaPoolTextures}
          position={[(isPortrait ? 2 : 4) * scale, (isPortrait ? -5.5 : -4.5) * scale, 1]}
          scale={scale * (isPortrait ? 0.8 : 1)}
        />
      )}
    </group>
  );
}

// Preview card overlay (hover/long-press) - no actions
function PreviewCardOverlay<T>({
  renderHtmlCard,
}: {
  renderHtmlCard: (card: T) => React.ReactNode;
  getCardId: (card: T) => string | number;
}) {
  const { previewCard } = useCardContext();

  if (!previewCard) return null;

  return (
    <Html
      center
      position={[0, 0, 5]}
      style={{
        pointerEvents: "none",
        userSelect: "none",
      }}
    >
      {renderHtmlCard(previewCard)}
    </Html>
  );
}

// Dragged card overlay - 3D card that follows pointer
function DraggedCardOverlay<T>({
  renderCardMesh,
}: {
  renderCardMesh: (card: T) => React.ReactNode;
}) {
  const { dragState } = useCardContext();
  const { camera } = useThree();
  const groupRef = useRef<THREE.Group>(null);

  // Convert screen coordinates to 3D world position
  useFrame(() => {
    if (!groupRef.current || !dragState) return undefined;

    // Convert screen position to normalized device coordinates (-1 to 1)
    const x = (dragState.currentPosition.x / window.innerWidth) * 2 - 1;
    const y = -(dragState.currentPosition.y / window.innerHeight) * 2 + 1;

    // Create a vector at the pointer position
    const vector = new THREE.Vector3(x, y, 0.5);
    vector.unproject(camera);

    // Get direction from camera to the unprojected point
    const dir = vector.sub(camera.position).normalize();

    // Calculate intersection with a plane at z = 2 (in front of board)
    const distance = (2 - camera.position.z) / dir.z;
    const pos = camera.position.clone().add(dir.multiplyScalar(distance));

    // Update position with slight offset
    groupRef.current.position.set(pos.x, pos.y, 3);
  });

  if (!dragState) return null;

  return (
    <group ref={groupRef}>
      <group rotation={[0, 0, -0.1]} scale={1.1}>
        {/* Render the card mesh with transparency */}
        <group>{renderCardMesh(dragState.card)}</group>
        {/* Add a semi-transparent overlay to indicate dragging */}
        <mesh position={[0, 0, 0.03]}>
          <planeGeometry args={[CARD_WIDTH, CARD_HEIGHT]} />
          <meshBasicMaterial color="#88ccff" transparent opacity={0.3} />
        </mesh>
      </group>
    </group>
  );
}

// Camera bounds for vertical panning
const CAMERA_BOUNDS = {
  minY: -6,
  maxY: 6,
};

// Responsive camera with vertical touch/drag panning
function ResponsiveCamera() {
  "use no memo";
  const { viewport, camera, gl } = useThree();
  const { dragState, isCardInteracting } = useCardContext();
  const isPortrait = viewport.height > viewport.width;

  // Store base camera position and current offset
  const basePosition = useRef({ x: 0, y: isPortrait ? -6 : -8, z: isPortrait ? 14 : 12 });
  const offsetY = useRef(0);
  const isPanning = useRef(false);
  const lastTouchY = useRef(0);
  const velocityY = useRef(0);
  // Track if touch started on a card to prevent panning
  const touchStartedOnCard = useRef(false);

  // Update base position when orientation changes
  React.useEffect(() => {
    basePosition.current = {
      x: 0,
      y: isPortrait ? -6 : -8,
      z: isPortrait ? 14 : 12,
    };
    // Reset offset on orientation change
    offsetY.current = 0;
    camera.position.set(basePosition.current.x, basePosition.current.y, basePosition.current.z);
    camera.lookAt(0, 0, 0);
    camera.updateProjectionMatrix();
  }, [isPortrait, camera]);

  // Clamp offset within bounds
  const clampOffsetY = (y: number) => {
    return Math.max(CAMERA_BOUNDS.minY, Math.min(CAMERA_BOUNDS.maxY, y));
  };

  // Handle touch/mouse events
  React.useEffect(() => {
    const canvas = gl.domElement;

    const handleStart = (clientY: number) => {
      // Don't start panning if a card is being interacted with or dragged
      if (dragState || isCardInteracting) {
        touchStartedOnCard.current = true;
        return undefined;
      }
      isPanning.current = true;
      lastTouchY.current = clientY;
      velocityY.current = 0;
    };

    const handleMove = (clientY: number) => {
      // Don't pan if card interaction/drag is active or touch started on card
      if (!isPanning.current || dragState || isCardInteracting || touchStartedOnCard.current)
        return undefined;

      const deltaY = (clientY - lastTouchY.current) * 0.02;

      // Store velocity for momentum
      velocityY.current = deltaY;

      // Update offset (inverted for natural drag feel)
      offsetY.current = clampOffsetY(offsetY.current + deltaY);

      // Update camera position
      camera.position.set(
        basePosition.current.x,
        basePosition.current.y + offsetY.current,
        basePosition.current.z
      );
      camera.lookAt(0, offsetY.current, 0);

      lastTouchY.current = clientY;
    };

    const handleEnd = () => {
      isPanning.current = false;
      touchStartedOnCard.current = false;
    };

    // Touch events
    const onTouchStart = (e: TouchEvent) => {
      if (e.touches.length === 1) {
        handleStart(e.touches[0].clientY);
      }
    };

    const onTouchMove = (e: TouchEvent) => {
      if (e.touches.length === 1) {
        // Prevent pull-to-refresh browser behavior only if we're panning
        if (isPanning.current && !touchStartedOnCard.current) {
          e.preventDefault();
        }
        handleMove(e.touches[0].clientY);
      }
    };

    const onTouchEnd = () => {
      handleEnd();
    };

    // Mouse events (for testing on desktop)
    const onMouseDown = (e: MouseEvent) => {
      // Only handle middle mouse button or when holding shift
      if (e.button === 1 || e.shiftKey) {
        handleStart(e.clientY);
        e.preventDefault();
      }
    };

    const onMouseMove = (e: MouseEvent) => {
      handleMove(e.clientY);
    };

    const onMouseUp = () => {
      handleEnd();
    };

    canvas.addEventListener("touchstart", onTouchStart, { passive: true });
    canvas.addEventListener("touchmove", onTouchMove, { passive: false });
    canvas.addEventListener("touchend", onTouchEnd);
    canvas.addEventListener("mousedown", onMouseDown);
    canvas.addEventListener("mousemove", onMouseMove);
    canvas.addEventListener("mouseup", onMouseUp);
    canvas.addEventListener("mouseleave", onMouseUp);

    return () => {
      canvas.removeEventListener("touchstart", onTouchStart);
      canvas.removeEventListener("touchmove", onTouchMove);
      canvas.removeEventListener("touchend", onTouchEnd);
      canvas.removeEventListener("mousedown", onMouseDown);
      canvas.removeEventListener("mousemove", onMouseMove);
      canvas.removeEventListener("mouseup", onMouseUp);
      canvas.removeEventListener("mouseleave", onMouseUp);
    };
  }, [camera, gl, dragState, isCardInteracting]);

  // Momentum animation
  useFrame(() => {
    // Don't apply momentum if card is being dragged
    if (dragState) return undefined;

    if (!isPanning.current && Math.abs(velocityY.current) > 0.001) {
      // Apply friction
      velocityY.current *= 0.92;

      // Update offset with momentum
      offsetY.current = clampOffsetY(offsetY.current + velocityY.current);

      // Update camera
      camera.position.set(
        basePosition.current.x,
        basePosition.current.y + offsetY.current,
        basePosition.current.z
      );
      camera.lookAt(0, offsetY.current, 0);
    }
  });

  return null;
}

function GameBoardCanvas<T>(props: GameBoardProps<T>) {
  useDebugRerender("GameBoardCanvas", props as Record<string, unknown>);
  const cardPositionsValue = useCardPositionsProvider();

  return (
    <CardPositionsContext.Provider value={cardPositionsValue}>
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
      <directionalLight position={[-8, 2, 10]} intensity={0.5} color="#e8e0f0" />

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

      {/* Environment for realistic reflections (background disabled) */}
      <Environment preset="night" environmentIntensity={0.7} background={false} />

      {/* Game board scene */}
      <GameBoardScene {...props} />

      {/* Card overlays */}
      <PreviewCardOverlay renderHtmlCard={props.renderHtmlCard} getCardId={props.getCardId} />
      <DraggedCardOverlay renderCardMesh={props.renderCardMesh} />

      {/* Target arrows for stack effects */}
      {props.stack?.map((effect) =>
        effect.targetCardIds.map((targetId) => (
          <TargetArrow3D
            key={`arrow-${props.getCardId(effect.card)}-${targetId}`}
            fromCardId={String(props.getCardId(effect.card))}
            toCardId={targetId}
          />
        ))
      )}
    </CardPositionsContext.Provider>
  );
}

export default function GameBoard<T>(props: GameBoardProps<T>) {
  useDebugRerender("GameBoard", props as Record<string, unknown>);
  const { onCardDragEnd, onCardClick } = props;
  const [previewCard, setPreviewCard] = useState<T | null>(null);
  const [dragState, setDragState] = useState<DragState<T>>(null);
  const [isCardInteracting, setIsCardInteracting] = useState(false);
  const [hoveredDropZone, setHoveredDropZone] = useState<string | null>(null);

  const handleCardClick = useCallback(
    (card: T | null) => {
      onCardClick?.(card);
    },
    [onCardClick]
  );

  const handleDragEnd = useCallback(
    (card: T, zone: string | null) => {
      onCardDragEnd?.(card, zone);
    },
    [onCardDragEnd]
  );

  // Global pointer move handler for drag tracking
  useEffect(() => {
    if (!dragState) return undefined;

    const handlePointerMove = (e: PointerEvent) => {
      setDragState((prev) =>
        prev
          ? {
              ...prev,
              currentPosition: { x: e.clientX, y: e.clientY },
            }
          : null
      );
    };

    const handlePointerUp = () => {
      if (dragState) {
        handleDragEnd(dragState.card, hoveredDropZone);
        setDragState(null);
        setHoveredDropZone(null);
        setIsCardInteracting(false);
      }
    };

    window.addEventListener("pointermove", handlePointerMove);
    window.addEventListener("pointerup", handlePointerUp);

    return () => {
      window.removeEventListener("pointermove", handlePointerMove);
      window.removeEventListener("pointerup", handlePointerUp);
    };
  }, [dragState, handleDragEnd, hoveredDropZone]);

  const dialogContent = props.renderDialog?.();
  const messageContent = props.renderMessage?.();
  const bottomBarContent = props.renderBottomBar?.();
  const overlayContent = props.renderOverlay?.();

  const contextValue = useMemo(
    () => ({
      previewCard,
      setPreviewCard,
      dragState,
      setDragState,
      onCardClick: handleCardClick,
      onDragEnd: handleDragEnd,
      isCardInteracting,
      setIsCardInteracting,
      hoveredDropZone,
      setHoveredDropZone,
    }),
    [previewCard, dragState, handleCardClick, handleDragEnd, isCardInteracting, hoveredDropZone]
  );

  return (
    <CardContext.Provider value={contextValue}>
      <div
        style={{
          position: "relative",
          width: "100dvw",
          height: "100dvh",
          userSelect: "none",
          WebkitUserSelect: "none",
        }}
      >
        <Canvas
          shadows
          dpr={[1, 2]}
          camera={{ position: [0, -8, 12], fov: 50, near: 0.1, far: 100 }}
          gl={{ antialias: true }}
          onCreated={({ gl }) => gl.setClearColor("#1a1a2e")}
        >
          <React.Suspense fallback={null}>
            <GameBoardCanvas {...props} />
          </React.Suspense>
        </Canvas>
        {messageContent && (
          <div
            style={{
              position: "absolute",
              top: 0,
              left: 0,
              right: 0,
              display: "flex",
              justifyContent: "center",
              pointerEvents: "none",
            }}
          >
            <div style={{ pointerEvents: "auto" }}>{messageContent}</div>
          </div>
        )}
        {bottomBarContent && (
          <div
            style={{
              position: "absolute",
              bottom: 0,
              left: 0,
              right: 0,
              pointerEvents: "auto",
            }}
          >
            {bottomBarContent}
          </div>
        )}
        {dialogContent && (
          <div
            style={{
              position: "absolute",
              top: 0,
              left: 0,
              right: 0,
              bottom: 0,
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              pointerEvents: "none",
            }}
          >
            {dialogContent}
          </div>
        )}
        {overlayContent && (
          <div
            style={{
              position: "absolute",
              top: 0,
              left: 0,
              right: 0,
              bottom: 0,
              pointerEvents: "auto",
            }}
          >
            {overlayContent}
          </div>
        )}
      </div>
    </CardContext.Provider>
  );
}
