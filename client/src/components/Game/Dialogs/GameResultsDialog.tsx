import { GameResultsDialogState } from "@/types";
import { useLoadedGameContext } from "@/utils/LoadedGameContext";
import Dialog from "@/components/ui/Dialog";
import { Text } from "@chakra-ui/react";
import Button from "@/components/ui/Button";

export default function GameResultsDialog() {
  const { gameState } = useLoadedGameContext();
  const { message } = gameState.screen.largeDialog as GameResultsDialogState;

  return (
    <Dialog
      title="Game results"
      footer={
        // @ts-expect-error - href is not defined in the ButtonProps type
        <Button variant="primary" as="a" href="/">
          New game
        </Button>
      }
    >
      <Text color="fg" textAlign="center">
        {message}
      </Text>
    </Dialog>
  );
}
