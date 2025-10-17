import { MessageBoxState } from "@/types";
import { Text } from "@chakra-ui/react";
import { Button } from "@chakra-ui/react";
import { get } from "@/utils/api";
import { useLoadedGameContext } from "@/utils/LoadedGameContext";
import Dialog from "@/components/ui/Dialog";
function ButtonSet({
  type,
  onCallback,
}: {
  type: "YesNo" | "Cancel" | "CancelOk";
  onCallback: (button: string) => void;
}) {
  if (type === "YesNo") {
    return (
      <>
        <Button colorScheme="green" mr={3} onClick={() => onCallback("Yes")}>
          Yes
        </Button>
        <Button variant="ghost" onClick={() => onCallback("No")}>
          No
        </Button>
      </>
    );
  }
  throw new Error(`Unknown button type: ${type}`);
}

type Props = {
  messageBox: MessageBoxState;
};

export default function MessageBox({ messageBox }: Props) {
  const { gameId, refresh } = useLoadedGameContext();
  const handleCallback = async (buttonClicked: string) => {
    await get(`/games/${gameId}/callback/${messageBox.callbackId}/${buttonClicked}`);
    refresh();
  };

  const hasTitle = messageBox.title !== "";
  const title = hasTitle ? messageBox.title : messageBox.message;
  const message = hasTitle ? messageBox.message : null;

  return (
    <Dialog
      title={title}
      footer={<ButtonSet type={messageBox.buttons} onCallback={handleCallback} />}
    >
      {message && <Text color="fg">{message}</Text>}
    </Dialog>
  );
}
