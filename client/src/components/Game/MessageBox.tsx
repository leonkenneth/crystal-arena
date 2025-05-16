import { MessageBoxState } from "@/types";
import { Dialog, Text } from "@chakra-ui/react";
import {
    Button,
} from "@chakra-ui/react";
import { get } from "@/utils/api";
import { useLoadedGameContext } from "@/utils/GameContext";

function ButtonSet({ type, onCallback }: { type: "YesNo" | "Cancel" | "CancelOk", onCallback: (button: string) => void }) {
    if (type === "YesNo") {
        return <>
            <Button colorScheme="green" mr={3} onClick={() => onCallback("Yes")}>
                Yes
            </Button>
            <Button variant="ghost" onClick={() => onCallback("No")}>
                No
            </Button>
        </>
    }
    return null; // Handle other button types as needed
}

type Props = {
    messageBox: MessageBoxState;
}

export default function MessageBox({ messageBox }: Props) {
    const { gameId, refresh } = useLoadedGameContext();
    const handleCallback = async (buttonClicked: string) => {
        await get(`/games/${gameId}/callback/${messageBox.callbackId}/${buttonClicked}`);
        refresh();
    }

    const hasTitle = messageBox.title !== "";
    const title = hasTitle ? messageBox.title : messageBox.message;
    const message = hasTitle ? messageBox.message : null;

    return (
        <Dialog.Root open={true}>
            <Dialog.Backdrop />
            <Dialog.Positioner>
                <Dialog.Content>
                    <Dialog.Header>
                        <Dialog.Title color="fg">{title}</Dialog.Title>
                    </Dialog.Header>
                    {message && <Dialog.Body>
                        <Text color="fg">{message}</Text>
                    </Dialog.Body>}
                    <Dialog.Footer>
                        <ButtonSet type={messageBox.buttons} onCallback={handleCallback} />
                    </Dialog.Footer>
                </Dialog.Content>
            </Dialog.Positioner>
        </Dialog.Root>
    );
}