import { CardOutsideFieldState, SelectTargetDialogState } from "@/types";
import { useLoadedGameContext } from "@/utils/LoadedGameContext";
import { Button } from "@chakra-ui/react";
import useDoAction from "@/utils/useDoAction";
import Dialog from "@/components/ui/Dialog";
import { selection } from "@/utils/gameStateQueries";

function selectAttackerMessage(selection : CardOutsideFieldState[]) {
    if (selection.length === 0) {
        return "Skip attack";
    }

    if (selection.length === 1) {
        const selectedCard = selection[0];
        return `Attack with ${selectedCard.name}`;
    }

    return `Party attack with ${selection.length} forwards`;
}

function selectBlockerMessage(selection : CardOutsideFieldState[]) {
    if (selection.length === 0) {
        return "Don't block";
    }

    if (selection.length === 1) {
        const selectedCard = selection[0];
        return `Block with ${selectedCard.name}`;
    }

    throw new Error("Multiple blockers are not supported");
}

function validateTargetMessage(selection : CardOutsideFieldState[]) {
    if (selection.length === 0) {
        return "No target";
    }

    if (selection.length === 1) {
        const selectedCard = selection[0];
        return `Target ${selectedCard.name}`;
    }

    return `Validate ${selection.length} targets`;
}

export default function SelectTargetDialog() {
    const { gameState } = useLoadedGameContext();
    const { oid, canCancel, text } = gameState.screen.smallDialog as SelectTargetDialogState;
    const doAction = useDoAction(oid);

    const onCancelClick = () => doAction("Cancel");
    const onOkClick = () => doAction("Done");

    let validateButtonText = "";
    const selectedCards = selection(gameState, { only: "yours" });
    if (text === "Select an attacker.") {
        validateButtonText = selectAttackerMessage(selectedCards);
    } else if (text === "Select a blocker.") {
        validateButtonText = selectBlockerMessage(selectedCards);
    } else {
        validateButtonText = validateTargetMessage(selectedCards);
    }

    return <Dialog title={text} footer={<>{canCancel && <Button onClick={onCancelClick}>Cancel</Button>}
    <Button onClick={onOkClick}>{validateButtonText}</Button></>} isModal={false}>
        
    </Dialog>;
}