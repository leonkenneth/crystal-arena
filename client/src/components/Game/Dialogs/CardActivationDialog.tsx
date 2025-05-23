import { useLoadedGameContext } from "@/utils/GameContext";
import Dialog from "@/components/ui/Dialog";
import { CardActivationDialogState } from "@/types";

export default function PriorityDialog() {
    const { gameState } = useLoadedGameContext();
    const dialog = gameState.screen.largeDialog as CardActivationDialogState;

    const { title } = dialog;

    return (
        <Dialog isModal={false} title={title} />
    );
}