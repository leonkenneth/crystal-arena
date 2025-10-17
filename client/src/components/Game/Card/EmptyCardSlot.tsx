import { CardContainer } from ".";
import { Card as ChakraCard } from "@chakra-ui/react";

export default function EmptyCardSlot() {
  return (
    <CardContainer isPlayable={false} border="dashed" borderWidth="1px">
      <ChakraCard.Body></ChakraCard.Body>
    </CardContainer>
  );
}
