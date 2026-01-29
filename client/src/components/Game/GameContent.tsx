import { useState } from "react";
import { CardState } from "@/types";
import { LoadedGameContext, useLoadedGameContext } from "@/utils/LoadedGameContext";
import GameBoard from "../GameBoard";
import Card from "./Card";
import { RoundedBox, useTexture } from "@react-three/drei";
import Dialogs from "./Dialogs";
import MessageBox from "./Dialogs/MessageBox";
import ExpandedZoneDialog from "./CollapsedZone/ExpandedZoneDialog";
import { ChakraProvider } from "@chakra-ui/react";
import { system } from "@/components/ui/system";
import { getImageUrl } from "./Card/CardImage";
import { doAction } from "@/utils/useDoAction";
import PassPriorityButton from "./PassPriorityButton";
const CARD_WIDTH = 0.7
const CARD_HEIGHT = 1
const CARD_DEPTH = 0.02
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
import { ManaPoolState } from "@/types/player";

const COLORS = {
  cardFront: '#e8e0d5',
  cardBack: '#2a4858',
  cardBorder: '#1a1a1a',
  deck: '#1e3a4c',
  graveyard: '#3d2c3d',
  battlefield: '#2d5a3d',
  hand: '#4a3728',
}
import back from "@/assets/back.jpeg";
import { getStepsToDisplay } from "./Steps";

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
    'light', 'water', 'dark', 'fire', 'wind',
    'ice', 'earth', 'lightning', 'crystal', 'colorless', 'multi'
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

function getCardBorderColor(card: CardState): string | null {
  // Blue border for selected/targeted cards (higher priority)
  if (card.isSelected || card.isSelectedForCombat || card.isTargetOfSpell) {
    return '#4488ff';
  }
  // Green border for playable cards
  if (card.isPlayable) {
    return '#44cc44';
  }
  return null;
}

function CardMesh({ card }: { card: CardState }) {
  const imageUrl = getImageUrl(card);
  const frontTexture = useTexture(imageUrl);
  const backTexture = useTexture(back.src);
  const imageWidth = CARD_WIDTH * 0.96;
  const imageHeight = CARD_HEIGHT * 0.96;
  const faceDown = !card.isVisibleInUi;
  const borderColor = getCardBorderColor(card);

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
        <meshStandardMaterial
          color={faceDown ? COLORS.cardBack : COLORS.cardFront}
        />
      </RoundedBox>
      <mesh position={[0, 0, CARD_DEPTH / 2 + 0.002]}>
        <planeGeometry args={[imageWidth, imageHeight]} />
        <meshStandardMaterial map={faceDown ? backTexture : frontTexture} />
      </mesh>
    </group>
  )
}

function renderCardMesh(card: CardState): React.ReactNode {
  const isTapped = card.isTapped;
  const rotation: [number, number, number] = isTapped ? [0, 0, -Math.PI / 12] : [0, 0, 0];
  const scale = isTapped ? 0.95 : 1;

  return (
    <group rotation={rotation} scale={scale}>
      <CardMesh card={card} />
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
  )
}

type ExpandedZone = {
  type: 'deck' | 'graveyard' | 'exile' | 'prize';
  isOpponent: boolean;
} | null;

export default function GameContent() {
  const loadedGameContext = useLoadedGameContext();
  const gameState = loadedGameContext.gameState;
  const screen = gameState.screen;
  const [expandedZone, setExpandedZone] = useState<ExpandedZone>(null);

  const yourHand = screen.zones.yourHand.cards;
  const yourBattlefield = screen.yourBattlefield.row1.slots.flatMap((slot) => slot.permanents).concat(screen.yourBattlefield.row2.slots.flatMap((slot) => slot.permanents));
  const yourDeck = screen.zones.yourMainDeck.cards;
  const yourGraveyard = screen.zones.yourBreakZone.cards;
  const yourExile = screen.zones.yourRemoveFromPlay.cards;
  const yourPrizeCards = screen.zones.yourDamageZone.cards;
  const opponentHand = screen.zones.opponentsHand.cards;
  const opponentBattlefield = screen.opponentsBattlefield.row1.slots.flatMap((slot) => slot.permanents).concat(screen.opponentsBattlefield.row2.slots.flatMap((slot) => slot.permanents));
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

  const renderDialog = () => {
    return <Dialogs />;
  }

  const renderMessage = () => {
    const messageBox = gameState.messageBox;
    if (!messageBox) {
      return null;
    }
    return <MessageBox messageBox={messageBox} />;
  }

  const renderHtmlCard = (card: CardState): React.ReactNode => {
    return (
      <ChakraProvider value={system}>
      <LoadedGameContext.Provider value={loadedGameContext}>
        <Card card={card} size="xl" />
      </LoadedGameContext.Provider>
      </ChakraProvider>
    );
  }

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
  }

  const handleCardClick = (card: CardState | null) => {
    if (!card) {
      return;
    }
    return doAction(loadedGameContext.gameId, card.oid, "Select");
  }

  const handleDeckClick = (isOpponent: boolean) => {
    setExpandedZone({ type: 'deck', isOpponent });
  };

  const handleGraveyardClick = (isOpponent: boolean) => {
    setExpandedZone({ type: 'graveyard', isOpponent });
  };

  const handleExileClick = (isOpponent: boolean) => {
    setExpandedZone({ type: 'exile', isOpponent });
  };

  const handlePrizeCardsClick = (isOpponent: boolean) => {
    setExpandedZone({ type: 'prize', isOpponent });
  };

  const renderOverlay = () => {
    if (!expandedZone) return null;

    let cards;
    if (expandedZone.type === 'deck') {
      cards = expandedZone.isOpponent ? opponentDeck : yourDeck;
    } else if (expandedZone.type === 'graveyard') {
      cards = expandedZone.isOpponent ? opponentGraveyard : yourGraveyard;
    } else if (expandedZone.type === 'exile') {
      cards = expandedZone.isOpponent ? opponentsExile : yourExile;
    } else {
      cards = expandedZone.isOpponent ? opponentsPrizeCards : yourPrizeCards;
    }

    const ownerLabel = expandedZone.isOpponent ? "Opponent's" : "Your";
    const zoneLabelMap: Record<string, string> = {
      deck: 'Deck',
      graveyard: 'Graveyard',
      exile: 'Exile',
      prize: 'Damage Zone',
    };
    const zoneLabel = zoneLabelMap[expandedZone.type];
    const title = `${ownerLabel} ${zoneLabel}`;

    return (
      <ChakraProvider value={system}>
        <LoadedGameContext.Provider value={loadedGameContext}>
          <ExpandedZoneDialog
            title={title}
            cards={cards}
            onClose={() => setExpandedZone(null)}
          />
        </LoadedGameContext.Provider>
      </ChakraProvider>
    );
  };

  const canDropToZone = (card: CardState, zone: 'battlefield' | 'graveyard'): boolean => {
    if (!card.playableActivations) return false;

    if (zone === 'battlefield') {
      return card.playableActivations.some(
        (activation) => activation.playZone === 'Battlefield' || activation.playZone === 'Stack'
      );
    } else if (zone === 'graveyard') {
      return card.playableActivations.some(
        (activation) => activation.playZone === 'BreakZone'
      );
    }
    return false;
  };

  return (
  <GameBoard<CardState>
      onCardClick={handleCardClick}
      onDeckClick={handleDeckClick}
      onGraveyardClick={handleGraveyardClick}
      onExileClick={handleExileClick}
      onPrizeCardsClick={handlePrizeCardsClick}
      canDropToZone={canDropToZone}
      renderHtmlCard={renderHtmlCard}
      renderCardMesh={renderCardMesh}
      renderEmptySlot={renderEmptySlot}
      renderOverlay={renderOverlay}
      getCardId={(card: CardState) => card.cardId}
      steps={steps}
      renderMessage={renderMessage}
      renderBottomBar={() => null}
      renderDialog={renderDialog}
      yourHand={yourHand}
      yourBattlefield={yourBattlefield}
      yourDeck={yourDeck}
      yourGraveyard={yourGraveyard}
      yourExile={yourExile}
      yourPrizeCards={yourPrizeCards}
      yourHealth={yourHealth}
      opponentHand={opponentHand}
      opponentBattlefield={opponentBattlefield}
      opponentDeck={opponentDeck}
      opponentGraveyard={opponentGraveyard}
      opponentsExile={opponentsExile}
      opponentsPrizeCards={opponentsPrizeCards}
      opponentHealth={opponentHealth}
      stack={stack?.effects.map((effect) => ({
          card: effect.card,
          targetCardIds: effect.targets.filter((t) => t.targetType === "Card").map((c) => c.cardId.toString()),
        })) || []
      }
      stackButton={renderStackButton}
      isOpponentTurn={screen.opponent.isActive}
      yourAvatarSrc={human.src}
      opponentAvatarSrc={computer.src}
      manaPoolTextures={getManaPoolTextures(screen.yourManaPool)}
    />
  );
}
