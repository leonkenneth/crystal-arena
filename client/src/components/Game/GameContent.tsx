import { useState, useRef } from "react";
import { useFrame } from "@react-three/fiber";
import * as THREE from "three";
import MenuButton from "./MenuButton";
import { CardState } from "@/types";
import { LoadedGameContext, useLoadedGameContext } from "@/utils/LoadedGameContext";
import GameBoard from "../GameBoard";
import Card from "./Card";
import { RoundedBox } from "@react-three/drei";
import Dialogs from "./Dialogs";
import MessageBox from "./Dialogs/MessageBox";
import ExpandedZoneDialog from "./CollapsedZone/ExpandedZoneDialog";
import { ChakraProvider } from "@chakra-ui/react";
import { system } from "@/components/ui/system";
import { getImageUrl } from "./Card/CardImage";
import { doAction } from "@/utils/useDoAction";
import { isTargeted } from "@/utils/gameStateQueries";
import PassPriorityButton from "./PassPriorityButton";
import { useTextureWithPlaceholder } from "../GameBoard/useTextureWithPlaceholder";
import { getStepsToDisplay } from "./Steps";
import human from "@/assets/human.png";
import computer from "@/assets/computer.png";
import lightIcon from "@/assets/icons/light.png";
import waterIcon from "@/assets/icons/water.png";
import darkIcon from "@/assets/icons/dark.png";
import fireIcon from "@/assets/icons/fire.png";
import windIcon from "@/assets/icons/wind.png";
import iceIcon from "@/assets/icons/ice.png";
import earthIcon from "@/assets/icons/earth.png";
import lightningIcon from "@/assets/icons/lightning.png";
import crystalIcon from "@/assets/icons/crystal.png";
import dullIcon from "@/assets/icons/dull.png";
import back from "@/assets/back.jpeg";
import { ManaPoolState } from "@/types/player";

const CARD_WIDTH = 0.7;
const CARD_HEIGHT = 1;
const CARD_DEPTH = 0.02;

const COLORS = {
  cardFront: "#e8e0d5",
  cardBack: "#2a4858",
  cardBorder: "#1a1a1a",
  deck: "#1e3a4c",
  graveyard: "#3d2c3d",
  battlefield: "#2d5a3d",
  hand: "#4a3728",
};

const manaIconMap: Record<string, string> = {
  light: lightIcon.src,
  water: waterIcon.src,
  dark: darkIcon.src,
  fire: fireIcon.src,
  wind: windIcon.src,
  ice: iceIcon.src,
  earth: earthIcon.src,
  lightning: lightningIcon.src,
  crystal: crystalIcon.src,
  colorless: dullIcon.src,
  multi: crystalIcon.src, // Use crystal for multi
};

function getManaPoolTextures(manaPool: ManaPoolState): string[] {
  const textures: string[] = [];

  const manaTypes: (keyof ManaPoolState)[] = [
    "light",
    "water",
    "dark",
    "fire",
    "wind",
    "ice",
    "earth",
    "lightning",
    "crystal",
    "colorless",
    "multi",
  ];

  for (const manaType of manaTypes) {
    const count = manaPool[manaType];
    const icon = manaIconMap[manaType];
    if (icon && count > 0) {
      for (let i = 0; i < count; i++) {
        textures.push(icon);
      }
    }
  }

  return textures;
}

function isSelectedOrTargeted(card: CardState): boolean {
  // @ts-expect-error - isSelectedForCombat is not defined on the CardOutsideFieldState type
  return card.isSelected || card.isSelectedForCombat || card.isTargetOfSpell;
}

function getCardBorderColor(card: CardState): string | null {
  // Red border for combat selection (highest priority)
  // @ts-expect-error - isSelectedForCombat is not defined on the CardOutsideFieldState type
  if (card.isSelectedForCombat) {
    return "#ff4444";
  }
  // Blue border for selected/targeted cards
  if (isSelectedOrTargeted(card)) {
    return "#4488ff";
  }
  // Green border for playable cards
  if (card.isPlayable) {
    return "#44cc44";
  }
  return null;
}

// Animated whirlwind effect for summoning sickness
function SummoningSicknessWhirlwind() {
  const groupRef = useRef<THREE.Group>(null);
  const wispRefs = useRef<THREE.Mesh[]>([]);

  // Create wisp data - positions around a spiral
  const wispCount = 6;
  const wisps = Array.from({ length: wispCount }, (_, i) => ({
    angle: (i / wispCount) * Math.PI * 2,
    radius: 0.15 + (i % 2) * 0.1,
    speed: 1.5 + (i % 3) * 0.3,
    yOffset: (i / wispCount) * 0.3 - 0.15,
  }));

  useFrame((state) => {
    if (!groupRef.current) return;

    const time = state.clock.elapsedTime;

    wispRefs.current.forEach((mesh, i) => {
      if (!mesh) return;
      const wisp = wisps[i];
      const angle = wisp.angle + time * wisp.speed;
      mesh.position.x = Math.cos(angle) * wisp.radius;
      mesh.position.y = wisp.yOffset + Math.sin(time * 2 + i) * 0.05;
      mesh.rotation.z = angle + Math.PI / 2;
    });
  });

  return (
    <group ref={groupRef} position={[0, 0, CARD_DEPTH / 2 + 0.008]}>
      {wisps.map((wisp, i) => (
        <mesh
          key={i}
          ref={(el) => {
            if (el) wispRefs.current[i] = el;
          }}
          position={[Math.cos(wisp.angle) * wisp.radius, wisp.yOffset, 0]}
          rotation={[0, 0, wisp.angle + Math.PI / 2]}
        >
          <planeGeometry args={[0.18, 0.04]} />
          <meshBasicMaterial color="#778899" transparent opacity={0.5} />
        </mesh>
      ))}
    </group>
  );
}

// Animated halo effect for buff/debuff
function StatHalo({ color, direction }: { color: string; direction: "up" | "down" }) {
  const ringRef = useRef<THREE.Mesh>(null);
  const materialRef = useRef<THREE.MeshBasicMaterial>(null);
  const isUp = direction === "up";
  const startY = isUp ? -CARD_HEIGHT / 2 : CARD_HEIGHT / 2;
  const endY = isUp ? CARD_HEIGHT / 2 + 0.1 : -CARD_HEIGHT / 2 - 0.1;

  useFrame((_, delta) => {
    if (!ringRef.current || !materialRef.current) return;

    // Move in direction and reset
    ringRef.current.position.y += delta * 0.4 * (isUp ? 1 : -1);
    const pastEnd = isUp ? ringRef.current.position.y > endY : ringRef.current.position.y < endY;

    if (pastEnd) {
      ringRef.current.position.y = startY;
      materialRef.current.opacity = 0.6;
    }

    // Fade out as it moves
    const totalDistance = Math.abs(endY - startY);
    const currentDistance = Math.abs(ringRef.current.position.y - startY);
    const progress = currentDistance / totalDistance;
    materialRef.current.opacity = 0.6 * (1 - progress);

    // Expand slightly as it moves
    const scale = 1 + progress * 0.3;
    ringRef.current.scale.set(scale, scale, 1);
  });

  return (
    <mesh
      ref={ringRef}
      position={[0, startY, CARD_DEPTH / 2 + 0.003]}
      rotation={[Math.PI / 2, 0, 0]}
    >
      <torusGeometry args={[0.25, 0.02, 8, 32]} />
      <meshBasicMaterial ref={materialRef} color={color} transparent opacity={0.6} />
    </mesh>
  );
}

function CardMesh({ card }: { card: CardState }) {
  const imageUrl = getImageUrl(card);
  const frontTexture = useTextureWithPlaceholder(imageUrl);
  const backTexture = useTextureWithPlaceholder(back.src);
  const imageWidth = CARD_WIDTH * 0.96;
  const imageHeight = CARD_HEIGHT * 0.96;
  const faceDown = !card.isVisibleInUi;
  const borderColor = getCardBorderColor(card);
  const hasDamage = card.damage > 0;
  const hasBuffedToughness = card.toughness > card.baseToughness;
  const hasDebuffedToughness = card.toughness < card.baseToughness;

  return (
    <group>
      {/* Colored border (rendered behind the card) */}
      {borderColor && (
        <RoundedBox
          args={[CARD_WIDTH + 0.06, CARD_HEIGHT + 0.06, CARD_DEPTH]}
          radius={0.04}
          smoothness={4}
          position={[0, 0, -0.01]}
        >
          <meshStandardMaterial
            color={borderColor}
            emissive={borderColor}
            emissiveIntensity={0.5}
          />
        </RoundedBox>
      )}
      {/* Card body */}
      <RoundedBox args={[CARD_WIDTH, CARD_HEIGHT, CARD_DEPTH]} radius={0.03} smoothness={4}>
        <meshStandardMaterial color={faceDown ? COLORS.cardBack : COLORS.cardFront} />
      </RoundedBox>
      <mesh position={[0, 0, CARD_DEPTH / 2 + 0.002]}>
        <planeGeometry args={[imageWidth, imageHeight]} />
        <meshStandardMaterial map={faceDown ? backTexture : frontTexture} />
      </mesh>
      {/* Buff indicator - rising yellow halo */}
      {hasBuffedToughness && !faceDown && <StatHalo color="#ffdd44" direction="up" />}
      {/* Debuff indicator - falling purple halo */}
      {hasDebuffedToughness && !faceDown && <StatHalo color="#aa44dd" direction="down" />}
      {/* Damage indicator - red scratch mark */}
      {hasDamage && !faceDown && (
        <group position={[0, 0, CARD_DEPTH / 2 + 0.005]}>
          {/* Main diagonal scratch */}
          <mesh rotation={[0, 0, Math.PI / 4]}>
            <planeGeometry args={[0.45, 0.06]} />
            <meshBasicMaterial color="#dd2222" />
          </mesh>
          {/* Second scratch line */}
          <mesh rotation={[0, 0, Math.PI / 4]} position={[0.08, -0.08, 0]}>
            <planeGeometry args={[0.35, 0.05]} />
            <meshBasicMaterial color="#bb1111" />
          </mesh>
          {/* Third scratch line */}
          <mesh rotation={[0, 0, Math.PI / 4]} position={[-0.08, 0.08, 0]}>
            <planeGeometry args={[0.3, 0.045]} />
            <meshBasicMaterial color="#cc1818" />
          </mesh>
        </group>
      )}
      {/* Summoning sickness - grey filter + swirling whirlwind */}
      {card.hasSummoningSickness && !faceDown && (
        <>
          <mesh position={[0, 0, CARD_DEPTH / 2 + 0.004]}>
            <planeGeometry args={[imageWidth, imageHeight]} />
            <meshBasicMaterial color="#667788" transparent opacity={0.3} />
          </mesh>
          <SummoningSicknessWhirlwind />
        </>
      )}
      {/* Frozen state - ice crystals at corners and edges */}
      {"isFrozen" in card && card.isFrozen && !faceDown && (
        <>
          {/* Large ice crystals at corners */}
          {[
            {
              pos: [CARD_WIDTH / 2 + 0.02, CARD_HEIGHT / 2 - 0.05, CARD_DEPTH / 2 + 0.02],
              rot: [0.3, 0, -0.5],
            },
            {
              pos: [-CARD_WIDTH / 2 - 0.02, CARD_HEIGHT / 2 - 0.05, CARD_DEPTH / 2 + 0.02],
              rot: [0.3, 0, 0.5],
            },
            {
              pos: [CARD_WIDTH / 2 + 0.02, -CARD_HEIGHT / 2 + 0.05, CARD_DEPTH / 2 + 0.02],
              rot: [-0.3, 0, -2.6],
            },
            {
              pos: [-CARD_WIDTH / 2 - 0.02, -CARD_HEIGHT / 2 + 0.05, CARD_DEPTH / 2 + 0.02],
              rot: [-0.3, 0, 2.6],
            },
          ].map((crystal, i) => (
            <mesh
              key={`corner-${i}`}
              position={crystal.pos as [number, number, number]}
              rotation={crystal.rot as [number, number, number]}
            >
              <coneGeometry args={[0.06, 0.18, 4]} />
              <meshStandardMaterial color="#66ddff" emissive="#66ddff" emissiveIntensity={0.8} />
            </mesh>
          ))}
          {/* Smaller ice crystals on edges */}
          {[
            { pos: [0, CARD_HEIGHT / 2 + 0.02, CARD_DEPTH / 2 + 0.02], rot: [0.4, 0, Math.PI] },
            { pos: [0, -CARD_HEIGHT / 2 - 0.02, CARD_DEPTH / 2 + 0.02], rot: [-0.4, 0, 0] },
            { pos: [CARD_WIDTH / 2 + 0.02, 0, CARD_DEPTH / 2 + 0.02], rot: [0, 0.4, -Math.PI / 2] },
            {
              pos: [-CARD_WIDTH / 2 - 0.02, 0, CARD_DEPTH / 2 + 0.02],
              rot: [0, -0.4, Math.PI / 2],
            },
          ].map((crystal, i) => (
            <mesh
              key={`edge-${i}`}
              position={crystal.pos as [number, number, number]}
              rotation={crystal.rot as [number, number, number]}
            >
              <coneGeometry args={[0.04, 0.12, 4]} />
              <meshStandardMaterial color="#66ddff" emissive="#66ddff" emissiveIntensity={0.8} />
            </mesh>
          ))}
        </>
      )}
    </group>
  );
}

function renderEmptySlot(): React.ReactNode {
  return (
    <group>
      <RoundedBox args={[CARD_WIDTH, CARD_HEIGHT, CARD_DEPTH]} radius={0.03} smoothness={4}>
        <meshStandardMaterial color={COLORS.cardBack} />
      </RoundedBox>
    </group>
  );
}

type ExpandedZone = {
  type: "deck" | "graveyard" | "exile" | "prize" | "opponentsHand" | "opponentsLimitBreak";
  isOpponent: boolean;
} | null;

export default function GameContent() {
  const loadedGameContext = useLoadedGameContext();
  const gameState = loadedGameContext.gameState;
  const screen = gameState.screen;
  const [expandedZone, setExpandedZone] = useState<ExpandedZone>(null);
  const [handsSwapped, setHandsSwapped] = useState(false);

  const yourHand = screen.zones.yourHand.cards;
  const yourLimitBreak = screen.zones.yourLimitBreak.cards;
  const yourBattlefieldFrontRow = screen.yourBattlefield.row2.slots.flatMap(
    (slot) => slot.permanents
  );
  const yourBattlefieldBackRow = screen.yourBattlefield.row1.slots.flatMap(
    (slot) => slot.permanents
  );
  const yourDeck = screen.zones.yourMainDeck.cards;
  const yourGraveyard = screen.zones.yourBreakZone.cards;
  const yourExile = screen.zones.yourRemoveFromPlay.cards;
  const yourPrizeCards = screen.zones.yourDamageZone.cards;
  const opponentHand = screen.zones.opponentsHand.cards;
  const opponentSideHand = screen.zones.opponentsLimitBreak.cards;

  // Swap hand and side hand when handsSwapped is true
  const displayedHand = handsSwapped ? yourLimitBreak : yourHand;
  const displayedSideHand = handsSwapped ? yourHand : yourLimitBreak;
  const opponentBattlefieldFrontRow = screen.opponentsBattlefield.row1.slots.flatMap(
    (slot) => slot.permanents
  );
  const opponentBattlefieldBackRow = screen.opponentsBattlefield.row2.slots.flatMap(
    (slot) => slot.permanents
  );
  const opponentDeck = screen.zones.opponentsMainDeck.cards;
  const opponentGraveyard = screen.zones.opponentsBreakZone.cards;
  const opponentsExile = screen.zones.opponentsRemoveFromPlay.cards;
  const opponentsPrizeCards = screen.zones.opponentsDamageZone.cards;
  const stack = screen.stack;
  const yourHealth = screen.you.life;
  const opponentHealth = screen.opponent.life;
  const stepsToDisplay = getStepsToDisplay(screen.steps.steps);
  const steps = stepsToDisplay.map((step) => ({
    id: step.name,
    label: step.name,
    isActive: step.isCurrent,
  }));

  const renderCardMesh = (
    card: CardState,
    isTopOfCollapsedZone: boolean,
    collapsedZoneCards: CardState[]
  ): React.ReactNode => {
    const isTapped = card.isTapped;
    const rotation: [number, number, number] = isTapped ? [0, 0, -Math.PI / 12] : [0, 0, 0];
    const scale = isTapped ? 0.95 : 1;

    // Check if card is targeted using stack effects (same as HTML Card component)
    const cardIsTargeted = isTargeted(gameState, card.cardId);

    // Check if card is the top of the stack (first effect)
    const isTopOfStack = stack?.effects.length > 0 && stack.effects[0].card.cardId === card.cardId;

    // Override visual states for collapsed zone top cards, targeted cards, or top of stack
    let displayCard = card;
    const needsOverride =
      cardIsTargeted ||
      isTopOfStack ||
      (isTopOfCollapsedZone &&
        (collapsedZoneCards.some((c) => c.isPlayable) ||
          collapsedZoneCards.some((c) => c.isSelected) ||
          collapsedZoneCards.some((c) => isTargeted(gameState, c.cardId))));

    if (needsOverride) {
      const anyPlayable =
        isTopOfCollapsedZone && !card.isPlayable && collapsedZoneCards.some((c) => c.isPlayable);
      const anySelected =
        isTopOfCollapsedZone && !card.isSelected && collapsedZoneCards.some((c) => c.isSelected);
      const anyTargeted =
        cardIsTargeted ||
        (isTopOfCollapsedZone && collapsedZoneCards.some((c) => isTargeted(gameState, c.cardId)));

      displayCard = {
        ...card,
        ...(anyPlayable && { isPlayable: true }),
        ...((anySelected || isTopOfStack) && { isSelected: true }),
        ...(anyTargeted && { isTargetOfSpell: true }),
      };
    }

    return (
      <group rotation={rotation} scale={scale}>
        <CardMesh card={displayCard} />
      </group>
    );
  };

  const renderDialog = () => {
    return <Dialogs />;
  };

  const renderMessage = () => {
    const messageBox = gameState.messageBox;
    if (!messageBox) {
      return null;
    }
    return <MessageBox messageBox={messageBox} />;
  };

  const renderHtmlCard = (card: CardState): React.ReactNode => {
    return (
      <ChakraProvider value={system}>
        <LoadedGameContext.Provider value={loadedGameContext}>
          <Card card={card} size="xl" overrideIsTapped={false} />
        </LoadedGameContext.Provider>
      </ChakraProvider>
    );
  };

  const renderStackButton = () => {
    if (stack?.effects.length === 0) {
      return null;
    }
    return (
      <ChakraProvider value={system}>
        <LoadedGameContext.Provider value={loadedGameContext}>
          <PassPriorityButton />
        </LoadedGameContext.Provider>
      </ChakraProvider>
    );
  };

  const handleCardClick = (card: CardState | null) => {
    if (!card) {
      return;
    }
    const effectIndex = stack?.effects.findIndex(
      (e) => e.card.cardId === card.cardId
    );
    if (effectIndex !== undefined && effectIndex >= 0 && stack?.oid) {
      return doAction(loadedGameContext.gameId, stack.oid, "SelectEffect", {
        index: effectIndex,
      });
    }
    return doAction(loadedGameContext.gameId, card.oid, "Select");
  };

  const handleDeckClick = (isOpponent: boolean) => {
    setExpandedZone({ type: "deck", isOpponent });
  };

  const handleGraveyardClick = (isOpponent: boolean) => {
    setExpandedZone({ type: "graveyard", isOpponent });
  };

  const handleExileClick = (isOpponent: boolean) => {
    setExpandedZone({ type: "exile", isOpponent });
  };

  const handlePrizeCardsClick = (isOpponent: boolean) => {
    setExpandedZone({ type: "prize", isOpponent });
  };

  const handleOpponentsHandClick = () => {
    setExpandedZone({ type: "opponentsHand", isOpponent: true });
  };

  const handleOpponentSideHandClick = () => {
    setExpandedZone({ type: "opponentsLimitBreak", isOpponent: true });
  };

  const handleSideHandClick = () => {
    setHandsSwapped((prev) => !prev);
  };

  const renderOverlay = () => {
    if (!expandedZone) return null;

    let cards;
    if (expandedZone.type === "deck") {
      cards = expandedZone.isOpponent ? opponentDeck : yourDeck;
    } else if (expandedZone.type === "graveyard") {
      cards = expandedZone.isOpponent ? opponentGraveyard : yourGraveyard;
    } else if (expandedZone.type === "exile") {
      cards = expandedZone.isOpponent ? opponentsExile : yourExile;
    } else if (expandedZone.type === "opponentsHand") {
      cards = opponentHand;
    } else if (expandedZone.type === "opponentsLimitBreak") {
      cards = opponentSideHand;
    } else {
      cards = expandedZone.isOpponent ? opponentsPrizeCards : yourPrizeCards;
    }

    const ownerLabel = expandedZone.isOpponent ? "Opponent's" : "Your";
    const zoneLabelMap: Record<string, string> = {
      deck: "Deck",
      graveyard: "Graveyard",
      exile: "Exile",
      prize: "Damage Zone",
      opponentsHand: "Hand",
      opponentsLimitBreak: "Limit Break",
    };
    const zoneLabel = zoneLabelMap[expandedZone.type];
    const title = `${ownerLabel} ${zoneLabel}`;

    return (
      <ChakraProvider value={system}>
        <LoadedGameContext.Provider value={loadedGameContext}>
          <ExpandedZoneDialog title={title} cards={cards} onClose={() => setExpandedZone(null)} />
        </LoadedGameContext.Provider>
      </ChakraProvider>
    );
  };

  const canDropToZone = (card: CardState, zone: "battlefield" | "graveyard"): boolean => {
    if (!card.playableActivations) return false;

    if (zone === "battlefield") {
      return card.playableActivations.some(
        (activation) => activation.playZone === "Battlefield" || activation.playZone === "Stack"
      );
    } else if (zone === "graveyard") {
      return card.playableActivations.some((activation) => activation.playZone === "BreakZone");
    }
    return false;
  };

  const handleCardDragEnd = (card: CardState, zone: string | null) => {
    if (!zone || !card.playableActivations) return;

    let matchingActivation;
    if (zone === "battlefield") {
      matchingActivation = card.playableActivations.find(
        (activation) => activation.playZone === "Battlefield" || activation.playZone === "Stack"
      );
    } else if (zone === "graveyard") {
      matchingActivation = card.playableActivations.find(
        (activation) => activation.playZone === "BreakZone"
      );
    }

    if (matchingActivation) {
      doAction(loadedGameContext.gameId, card.oid, "ActivateAbilityFromAbilityId", {
        abilityId: matchingActivation.abilityId,
      });
    }
  };

  return (
    <>
      <MenuButton />
      <GameBoard<CardState>
        onCardClick={handleCardClick}
        onDeckClick={handleDeckClick}
        onGraveyardClick={handleGraveyardClick}
        onExileClick={handleExileClick}
        onPrizeCardsClick={handlePrizeCardsClick}
        onOpponentsHandClick={handleOpponentsHandClick}
        onSideHandClick={handleSideHandClick}
        canDropToZone={canDropToZone}
        onCardDragEnd={handleCardDragEnd}
        renderHtmlCard={renderHtmlCard}
        renderCardMesh={renderCardMesh}
        renderEmptySlot={renderEmptySlot}
        renderOverlay={renderOverlay}
        getCardId={(card: CardState) => card.cardId}
        steps={steps}
        renderMessage={renderMessage}
        renderBottomBar={() => null}
        renderDialog={renderDialog}
        yourHand={displayedHand}
        yourSideHand={displayedSideHand}
        yourBattlefieldFrontRow={yourBattlefieldFrontRow}
        yourBattlefieldBackRow={yourBattlefieldBackRow}
        yourDeck={yourDeck}
        yourGraveyard={yourGraveyard}
        yourExile={yourExile}
        yourPrizeCards={yourPrizeCards}
        yourHealth={yourHealth}
        opponentHand={opponentHand}
        opponentSideHand={opponentSideHand}
        opponentBattlefieldFrontRow={opponentBattlefieldFrontRow}
        opponentBattlefieldBackRow={opponentBattlefieldBackRow}
        opponentDeck={opponentDeck}
        opponentGraveyard={opponentGraveyard}
        opponentsExile={opponentsExile}
        opponentsPrizeCards={opponentsPrizeCards}
        opponentHealth={opponentHealth}
        onOpponentSideHandClick={handleOpponentSideHandClick}
        stack={
          stack?.effects.map((effect) => ({
            card: effect.card,
            targetCardIds: effect.targets
              .filter((t) => t.targetType === "Card")
              .map((c) => c.cardId.toString()),
          })) || []
        }
        stackButton={renderStackButton}
        isOpponentTurn={screen.opponent.isActive}
        yourAvatarSrc={human.src}
        opponentAvatarSrc={computer.src}
        manaPoolTextures={getManaPoolTextures(screen.yourManaPool)}
      />
    </>
  );
}
