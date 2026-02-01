import { CardState } from "@/types/cards";
import { Center } from "@chakra-ui/react";
import Card from "../Card";

type Props = {
    card: CardState;
}

export default function CardDialogBody({ card }: Props) {
  return (
    <Center marginBottom="2rem">
      <Card size="lg" card={{...card, isTapped: false,isPlayable: true}} />
    </Center>
  );
}