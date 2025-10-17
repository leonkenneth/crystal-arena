import Hand from "./Hand";
import BattlefieldRow from "./BattlefieldRow/index";
import PlayerName from "./PlayerName";
import LifeAndDamageZone from "./LifeAndDamageZone";
import CollapsedZone from "./CollapsedZone/index";
import MessageBox from "./Dialogs/MessageBox";
import { useLoadedGameContext } from "@/utils/LoadedGameContext";
import { VStack, HStack, StackSeparator, Box, Flex } from "@chakra-ui/react";
import Dialogs from "./Dialogs/index";
import Stack from "./Stack";
import CPPool from "./CPPool";
import PassPriorityButton from "./PassPriorityButton";
import Steps from "./Steps";
import { useState } from "react";
import { CardState } from "@/types";
import { ClientContext } from "@/utils/ClientContext";
import HoveredCard from "./HoveredCard";

export default function GameContent() {
  const { gameState } = useLoadedGameContext();
  const [hoveredCard, setHoveredCard] = useState<CardState | null>(null);
  const screen = gameState.screen;

  return (
    <ClientContext.Provider value={{ hoveredCard, setHoveredCard }}>
      <VStack
        h="100dvh"
        w="100vw"
        bg="cyan.950"
        overflowX="hidden"
        overflowY="auto"
        userSelect="none"
        gap={0}
      >
        {/* Opponent Area */}
        <VStack p={4} bg="cyan.900" w="full">
          <HStack flexShrink={0} align="flex-start" justify="space-between" w="full">
            <HStack flexShrink={1} align="flex-start" justify="flex-start">
              <Box>
                <PlayerName player={screen.opponent} />
                <LifeAndDamageZone
                  damageZone={screen.zones.opponentsDamageZone}
                  damageZoneName="Opponent's DamageZone"
                />
              </Box>
              <CPPool pool={screen.opponentsManaPool} />
            </HStack>
            <HStack>
              <CollapsedZone zone={screen.zones.opponentsMainDeck} name="Opponent's Deck" />
              <CollapsedZone zone={screen.zones.opponentsBreakZone} name="Opponent's BreakZone" />
            </HStack>
          </HStack>
          <HStack w="full" flexGrow={1} flexShrink={1} overflowX="auto" justifyContent="center">
            <Hand hand={screen.zones.opponentsHand} upsideDown />
          </HStack>
        </VStack>

        {/* Battlefield */}
        <VStack gap={4} w="full">
          <BattlefieldRow row={screen.opponentsBattlefield.row2} />
          <BattlefieldRow row={screen.opponentsBattlefield.row1} />
          <StackSeparator />
          <BattlefieldRow row={screen.yourBattlefield.row2} />
          <BattlefieldRow row={screen.yourBattlefield.row1} />
        </VStack>

        {/* Your Area */}
        <VStack p={4} w="full" bg="cyan.900">
          <HStack flexShrink={0} align="flex-start" justify="space-between" w="full">
            <HStack flexShrink={1} align="flex-start" justify="flex-start">
              <Box>
                <PlayerName player={screen.you} />
                <LifeAndDamageZone
                  damageZone={screen.zones.yourDamageZone}
                  damageZoneName="Your DamageZone"
                />
              </Box>
              <CPPool pool={screen.yourManaPool} />
            </HStack>
            <HStack>
              <CollapsedZone zone={screen.zones.yourMainDeck} name="Your Deck" />
              <CollapsedZone zone={screen.zones.yourBreakZone} name="Your BreakZone" />
            </HStack>
          </HStack>
        </VStack>
        <VStack w="full" position="sticky" bottom={0} zIndex="sticky" gap={0}>
          <HStack
            w="full"
            p={4}
            flexGrow={1}
            bg="cyan.900"
            flexShrink={1}
            overflowX="auto"
            justifyContent="center"
          >
            <Hand hand={screen.zones.yourHand} />
          </HStack>
          {/* Steps and Pass Priority Button */}
          <HStack
            w="full"
            p={4}
            justify="space-between"
            align="flex-start"
            maxW="100vw"
            bg="cyan.800"
          >
            <Flex flexGrow={1} flexShrink={1} overflowX="auto" h="full" placeItems="center">
              <Steps />
            </Flex>
            <Box flexGrow={0} flexShrink={0}>
              <PassPriorityButton />
            </Box>
          </HStack>
        </VStack>

        {/* Global dialogs */}
        {gameState.messageBox && <MessageBox messageBox={gameState.messageBox} />}
        <Dialogs />
        <Stack />
        <HoveredCard />
      </VStack>
    </ClientContext.Provider>
  );
}
