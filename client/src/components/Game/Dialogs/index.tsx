import { useLoadedGameContext } from "@/utils/GameContext";
import PriorityDialog from "./PriorityDialog";
import SelectTargetDialog from "./SelectTargetDialog";
import SelectAbilityDialog from "./SelectAbilityDialog";
import NextTurnDialog from "./NextTurnDialog";

export default function Dialogs() {
    const { gameState } = useLoadedGameContext();
    const smallDialog = gameState.screen.smallDialog;
    const largeDialog = gameState.screen.largeDialog;

    if (!smallDialog && !largeDialog) {
        return null;
    }
    
    const dialogType = largeDialog?.type || smallDialog?.type;
    switch (dialogType) {
        case "SelectTarget":
            return <SelectTargetDialog />;
        case "SelectAbility":
            return <SelectAbilityDialog />;
        case "NextTurnDialog":
            return <NextTurnDialog />;
        case "Priority":
            return <PriorityDialog />;
        default:
            throw new Error(`Unimplemented dialog type: ${dialogType}`);
    }

    return null;
}