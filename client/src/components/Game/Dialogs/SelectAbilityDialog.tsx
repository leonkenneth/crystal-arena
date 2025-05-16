import { SelectAbilityDialogState } from "@/types";
import { useLoadedGameContext } from "@/utils/GameContext";
import { Dialog, Button, Portal } from "@chakra-ui/react";
import useDoAction from "@/utils/useDoAction";

export default function SelectAbilityDialog() {
    const { gameState } = useLoadedGameContext();
    const { oid, canCancel, descriptions } = gameState.screen.largeDialog as SelectAbilityDialogState;
    const doAction = useDoAction(oid);

    const onCancelClick = () => doAction("Cancel");

    return <Dialog.Root 
        open={true}
    >
        <Portal>
        <Dialog.Backdrop />
        <Dialog.Positioner>
            <Dialog.Content>
                <Dialog.Header>
                    <Dialog.Title color="fg">Select an ability</Dialog.Title>
            </Dialog.Header>
            <Dialog.Body>
                {descriptions.map((description, index) => (
                    <Button key={description} onClick={() => doAction("SetSelectedIndex", { index })}>
                        {description}
                    </Button>
                ))}
            </Dialog.Body>
            <Dialog.Footer>
                {canCancel && <Button onClick={onCancelClick}>Cancel</Button>}
            </Dialog.Footer>
                </Dialog.Content>
            </Dialog.Positioner>
        </Portal>
    </Dialog.Root>;
}