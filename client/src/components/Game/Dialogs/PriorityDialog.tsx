import { useLoadedGameContext } from "@/utils/GameContext";
import { Dialog, DialogHeader, DialogTitle } from "@chakra-ui/react";
export default function PriorityDialog() {
    const { gameState } = useLoadedGameContext();
    const priorityDialog = gameState.smallDialog;

    const stackOpened = gameState.screen.stack.effects.length > 0;

    if (!priorityDialog || stackOpened) {
        return null;
    }
    

    return (
        <Dialog>
            <DialogHeader>
                <DialogTitle>Priority</DialogTitle>
            </DialogHeader>
        </Dialog>
    );
}