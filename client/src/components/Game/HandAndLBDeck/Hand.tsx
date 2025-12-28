import { CardOutsideFieldState } from "@/types";
import CardInHand from "./CardInHand";
import { Box, VStack, HStack, Text } from "@chakra-ui/react";

type Props = {
  cards?: CardOutsideFieldState[];
  toggleText: string;
  showToggleText: boolean;
  onClickToggle?: () => void;
};

export default function Hand({ cards, toggleText, onClickToggle, showToggleText }: Props) {
  if (!cards || cards.length === 0) {
    return <span>No card in hand</span>;
  }

  return (
    <VStack w="full" gap={1}>
    <HStack
      w="full"
      justifyContent="center"
      overflowX="scroll"
      flexShrink={0}
      flexGrow={0}
      gap={2}
      style={{
        pointerEvents: "none",
        scrollSnapType: "x mandatory",
      }}
    >
      {cards.map((card) => {
        return (
          <div key={card.cardId}>
            <CardInHand card={card} />
          </div>
        );
      })}
    </HStack>
      {showToggleText && <Box pointerEvents="auto">
        <Text color="fg.muted" fontSize="sm" cursor="pointer" onClick={onClickToggle} _hover={{ textDecoration: "underline" }}>
          {toggleText}
        </Text>
      </Box>} 
    </VStack>
  );
}
