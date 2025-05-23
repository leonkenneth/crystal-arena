import { useLoadedGameContext } from "@/utils/GameContext";
import Dialog from "@/components/ui/Dialog";
import { PriorityDialogState } from "@/types";
import PassPriorityButton from "../PassPriorityButton";
import { Text } from "@chakra-ui/react";
export default function PriorityDialog() {
    const { gameState } = useLoadedGameContext();
    const priorityDialog = gameState.screen.smallDialog as PriorityDialogState;

    const stackOpened = gameState.screen.stack.effects.length > 0;

    if (!priorityDialog || stackOpened) {
        return null;
    }

    return (
        <Dialog isModal={false} title="You have priority" footer={<PassPriorityButton />}>
            <Text textAlign="center" color="fg">Play a card or ability, or pass priority</Text>
        </Dialog>
    );
}