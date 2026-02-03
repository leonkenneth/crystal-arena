import { CardState } from "@/types";
import { Box, CloseButton, Dialog, HStack, Portal, VStack, Text } from "@chakra-ui/react";
import Button from "@/components/ui/Button";
import Card from "../Card";

type Props = {
  onClose: () => void;
  cards: CardState[];
  title: string;
};

function DialogBody({ cards, title }: { cards: CardState[]; title: string }) {
  if (cards.length === 0) {
    return (
      <VStack w="full" justify="center" align="center" overflow="scroll">
        <Text color="fg">No cards in {title}</Text>
      </VStack>
    );
  }

  return (
    <VStack w="full" justify="center" align="center">
      <Box w="full" overflowX="auto">
        <HStack justify="center" align="center" minW="min-content" mx="auto">
          {cards.map((card) => (
            <Card
              card={card}
              key={card.cardId}
              containerProps={{
                size: "lg",
              }}
            />
          ))}
        </HStack>
      </Box>
      <Text color="fg">{cards.length > 1 ? `${cards.length} cards in ${title}` : title}</Text>
    </VStack>
  );
}
export default function ExpandedZoneDialog({ title, cards, onClose }: Props) {
  return (
    <Dialog.Root size="full" motionPreset="slide-in-bottom" open>
      <Portal>
        <Dialog.Backdrop />
        <Dialog.Positioner>
          <Dialog.Content>
            <Dialog.Header>
              <Dialog.Title color="white">{title}</Dialog.Title>
            </Dialog.Header>
            <Dialog.Body display="flex" justifyContent="center" alignItems="center" h="full">
              <DialogBody cards={cards} title={title} />
            </Dialog.Body>
            <Dialog.Footer>
              <Dialog.ActionTrigger asChild>
                <Button variant="secondary" onClick={onClose}>
                  Close
                </Button>
              </Dialog.ActionTrigger>
            </Dialog.Footer>
            <Dialog.CloseTrigger asChild onClick={onClose}>
              <CloseButton size="sm" />
            </Dialog.CloseTrigger>
          </Dialog.Content>
        </Dialog.Positioner>
      </Portal>
    </Dialog.Root>
  );
}
