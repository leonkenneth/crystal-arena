import { NextTurnDialogState } from "@/types";
import { useLoadedGameContext } from "@/utils/LoadedGameContext";
import Dialog from "@/components/ui/Dialog";
import { Text } from "@chakra-ui/react";

export default function NextTurnDialog() {
  const { gameState } = useLoadedGameContext();
  const { message } = gameState.screen.largeDialog as NextTurnDialogState;

  return (
    <Dialog title="New turn starts">
      <Text color="fg" textAlign="center">
        {message}
      </Text>
    </Dialog>
  );
}
