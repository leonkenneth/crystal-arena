import { CardState } from "@/types";
import { Button, CloseButton, Dialog, HStack, Portal, VStack, Text } from "@chakra-ui/react";
import Card from "../Card";

type Props = {
    onClose: () => void;
    cards: CardState[];
    title: string;
}

function DialogBody({ cards, title }: { cards: CardState[], title: string }) {
    if (cards.length === 0) {
        return (
            <VStack w="full" justify="center" align="center" overflow="scroll">
                <Text color="fg">No cards in {title}</Text>
            </VStack>
        )
    }

    return (
      <VStack w="full" justify="center" align="center" overflow="scroll">
        <HStack w="full" justify="center" align="center" overflow="scroll">
            {cards.map((card) => (
                <Card card={card} key={card.cardId} containerProps={{
                    size: "large"
                }} />
            ))}
        </HStack>
        <Text color="fg">{cards.length > 1 ? `${cards.length} cards in ${title}` : title}</Text>
        </VStack>
    )
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
                    <Button variant="outline" onClick={onClose}>Close</Button>
                  </Dialog.ActionTrigger>
                </Dialog.Footer>
                <Dialog.CloseTrigger asChild onClick={onClose}>
                  <CloseButton size="sm" />
                </Dialog.CloseTrigger>
              </Dialog.Content>
            </Dialog.Positioner>
          </Portal>
        </Dialog.Root>
      )
}