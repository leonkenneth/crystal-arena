import { GameResultsDialogState } from "@/types";
import { useLoadedGameContext } from "@/utils/LoadedGameContext";
import Dialog from "@/components/ui/Dialog";
import { Text, Button } from "@chakra-ui/react";

export default function GameResultsDialog() {
  const { gameState } = useLoadedGameContext();
  const { message } = gameState.screen.largeDialog as GameResultsDialogState;

  return (
    <Dialog
      title="Game results"
      footer={
        // @ts-ignore
        <Button as="a" href="/">
          New game
        </Button>
      }
    >
      <Text color="fg">{message}</Text>
    </Dialog>
  );
}
