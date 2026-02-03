import { Box } from "@chakra-ui/react";
import { CardState } from "@/types/cards";

export default function CardPower({ card }: { card: CardState }) {
  return (
    <Box
      position="absolute"
      bottom="2px"
      right="0"
      bg="blackAlpha.900"
      px="6px"
      py="2px"
      w="100%"
      textAlign="right"
      borderBottomLeftRadius="sm"
      borderBottomRightRadius="sm"
      fontSize="1.6rem"
      fontStyle="italic"
      fontWeight="bold"
      lineHeight="1"
    >
      {card.damage > 0 && (
        <>
          <Box as="span" color={card.damage > 0 ? "red.500" : "white"}>
            {card.toughness - card.damage}
          </Box>
          <Box as="span" color={card.toughness !== card.baseToughness ? "yellow.400" : "white"}>
            {" / "}
            {card.toughness}
          </Box>
        </>
      )}
      {card.damage === 0 && (
        <>
          <Box as="span" color={card.toughness !== card.baseToughness ? "yellow.400" : "white"}>
            {card.toughness}
          </Box>
        </>
      )}
    </Box>
  );
}
