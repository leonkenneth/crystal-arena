import { SelectTargetDialogState } from "@/types";
import { useLoadedGameContext } from "@/utils/GameContext";
import { Dialog, Button, Portal } from "@chakra-ui/react";
import useDoAction from "@/utils/useDoAction";

export default function SelectTargetDialog() {
    const { gameState } = useLoadedGameContext();
    const { oid, canCancel, text } = gameState.screen.smallDialog as SelectTargetDialogState;
    const doAction = useDoAction(oid);

    const onCancelClick = () => doAction("Cancel");
    const onOkClick = () => doAction("Done");

    return <Dialog.Root 
        open={true}
        modal={false}
        closeOnInteractOutside={false}
    >
        <Portal>
            {/* <Dialog.Backdrop /> */}
            <Dialog.Positioner pointerEvents="none">
                <Dialog.Content>
                    <Dialog.Header>
                        <Dialog.Title color="fg">{text}</Dialog.Title>
            </Dialog.Header>
            <Dialog.Footer>
                {canCancel && <Button onClick={onCancelClick}>Cancel</Button>}
                <Button onClick={onOkClick}>OK</Button>
                </Dialog.Footer>
                </Dialog.Content>
            </Dialog.Positioner>
        </Portal>
    </Dialog.Root>;
}