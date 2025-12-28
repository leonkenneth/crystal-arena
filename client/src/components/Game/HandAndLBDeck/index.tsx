import { useState } from "react";
import { HStack, VStack, Text, Box} from "@chakra-ui/react";
import { HandState, LimitBreakState } from "@/types";
import Hand from "./Hand";
import LimitBreak from "./LimitBreak";

type Props = {
  hand: HandState;
  limitBreak: LimitBreakState;
};

export default function HandAndLBDeck({ hand, limitBreak }: Props) {
  const [showLimitBreak, setShowLimitBreak] = useState(false);
  const lbCount = limitBreak.cards.length;

  let shownCards = hand.cards,
      showToggleText = lbCount > 0,
      toggleText = "Switch to LB";

  const onClickToggle = () => {
    setShowLimitBreak(!showLimitBreak);
  }

  if (showLimitBreak) {
    shownCards = limitBreak.cards;
    showToggleText = true;
    toggleText = "Back to Hand";
  }

  return (
    <VStack w="full" gap={1}>
      <HStack
        w="full"
        p={4}
        flexGrow={1}
        pointerEvents="none"
        bg="transparent"
        flexShrink={1}
        overflowX="auto"
        justifyContent="center"
      >
        <Hand cards={shownCards} showToggleText={showToggleText} toggleText={toggleText} onClickToggle={onClickToggle} onZoomOut={() => setShowLimitBreak(false)} />
      </HStack>
    </VStack>
  );
}


