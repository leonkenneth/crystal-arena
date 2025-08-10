import { useLoadedGameContext } from "@/utils/LoadedGameContext";
import PriorityDialog from "./PriorityDialog";
import SelectTargetDialog from "./SelectTargetDialog";
import SelectAbilityDialog from "./SelectAbilityDialog";
import NextTurnDialog from "./NextTurnDialog";
import CardActivationDialog from "./CardActivationDialog";

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
        case "CardActivation":
            return <CardActivationDialog />;
        default:
            throw new Error(`Unimplemented dialog type: ${dialogType}`);
    }

    return null;
}