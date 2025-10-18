import { MessageBoxState } from "@/types";
import { Text, HStack } from "@chakra-ui/react";
import Button from "@/components/ui/Button";
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
      <HStack gap={3}>
        <Button variant="primary" onClick={() => onCallback("Yes")}>
          Yes
        </Button>
        <Button variant="secondary" onClick={() => onCallback("No")}>
          No
        </Button>
      </HStack>
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
      {message && <Text color="fg" textAlign="center">{message}</Text>}
    </Dialog>
  );
}
