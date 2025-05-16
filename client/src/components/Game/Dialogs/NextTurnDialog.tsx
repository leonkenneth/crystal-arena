import { NextTurnDialogState } from "@/types";
import { useLoadedGameContext } from "@/utils/GameContext";
import { Dialog, Button, Portal, Text } from "@chakra-ui/react";
import useDoAction from "@/utils/useDoAction";

export default function NextTurnDialog() {
    const { gameState } = useLoadedGameContext();
    const { message } = gameState.screen.largeDialog as NextTurnDialogState;
    return <Dialog.Root 
        open={true}
    >
        <Portal>
        <Dialog.Backdrop />
        <Dialog.Positioner>
            <Dialog.Content>
                <Dialog.Header>
                    <Dialog.Title color="fg">New turn starts</Dialog.Title>
            </Dialog.Header>
            <Dialog.Body>
                <Text color="fg">{message}</Text>
            </Dialog.Body>
                </Dialog.Content>
            </Dialog.Positioner>
        </Portal>
    </Dialog.Root>;
}