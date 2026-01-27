import { CardState } from "@/types";
import { LoadedGameContext, useLoadedGameContext } from "@/utils/LoadedGameContext";
import GameBoard from "../GameBoard";
import Card from "./Card";
import { RoundedBox } from "@react-three/drei";
import Dialogs from "./Dialogs";
import MessageBox from "./Dialogs/MessageBox";
import { ChakraProvider } from "@chakra-ui/react";
import { system } from "@/components/ui/system";
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
function renderCard(card: CardState, faceDown: boolean): React.ReactNode {
  const color = COLORS.cardFront;
  return (
    <group>
      <RoundedBox args={[CARD_WIDTH, CARD_HEIGHT, CARD_DEPTH]} radius={0.03} smoothness={4}>
        <meshStandardMaterial color={faceDown ? COLORS.cardBack : color} />
      </RoundedBox>
      {/* Card border/frame */}
      <RoundedBox
        args={[CARD_WIDTH * 0.92, CARD_HEIGHT * 0.92, CARD_DEPTH + 0.005]}
        radius={0.02}
        smoothness={4}
        position={[0, 0, 0.001]}
      >
        <meshStandardMaterial color={faceDown ? COLORS.cardBack : color} />
      </RoundedBox>
    </group>
  )
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

export default function GameContent() {
  const loadedGameContext = useLoadedGameContext();
  const gameState = loadedGameContext.gameState;
  const screen = gameState.screen;

  const yourHand = screen.zones.yourHand.cards;
  const yourBattlefield = screen.yourBattlefield.row1.slots.flatMap((slot) => slot.permanents).concat(screen.yourBattlefield.row2.slots.flatMap((slot) => slot.permanents));
  const yourDeck = screen.zones.yourMainDeck.cards;
  const yourGraveyard = screen.zones.yourBreakZone.cards;
  const opponentHand = screen.zones.opponentsHand.cards;
  const opponentBattlefield = screen.opponentsBattlefield.row1.slots.flatMap((slot) => slot.permanents).concat(screen.opponentsBattlefield.row2.slots.flatMap((slot) => slot.permanents));
  const opponentDeck = screen.zones.opponentsMainDeck.cards;
  const opponentGraveyard = screen.zones.opponentsBreakZone.cards;
  const yourHealth = 25;
  const opponentHealth = 18;

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

  const renderHtmlCard = (card: CardState, faceDown: boolean): React.ReactNode => {
    return (
      <ChakraProvider value={system}>
      <LoadedGameContext.Provider value={loadedGameContext}>
        <Card card={card} size="xl" />
      </LoadedGameContext.Provider>
      </ChakraProvider>
    );
  }

  return (
  <GameBoard<CardState>
      renderHtmlCard={renderHtmlCard}
      renderCard={renderCard}
      renderEmptySlot={renderEmptySlot}
      getCardId={(card: CardState) => card.cardId}
      stack={null}
      steps={[]}
      renderMessage={renderMessage}
      renderBottomBar={() => null}
      renderDialog={renderDialog}
      yourHand={yourHand}
      yourBattlefield={yourBattlefield}
      yourDeck={yourDeck}
      yourGraveyard={yourGraveyard}
      yourHealth={yourHealth}
      opponentHand={opponentHand}
      opponentBattlefield={opponentBattlefield}
      opponentDeck={opponentDeck}
      opponentGraveyard={opponentGraveyard}
      opponentHealth={opponentHealth}
    />
  );
}
