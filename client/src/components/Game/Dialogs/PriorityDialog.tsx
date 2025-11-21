import { useLoadedGameContext } from "@/utils/LoadedGameContext";
import Dialog from "@/components/ui/Dialog";
import { PriorityDialogState } from "@/types";
import PassPriorityButton from "../PassPriorityButton";
import { Text } from "@chakra-ui/react";
export default function PriorityDialog() {
  const { gameState } = useLoadedGameContext();
  const priorityDialog = gameState.screen.smallDialog as PriorityDialogState;

  const stackOpened = gameState.screen.stack.effects.length > 0;

  if (!priorityDialog || stackOpened) {
    return null;
  }

  const isYourTurn = gameState.screen.you.isActive;

  return (
    <Dialog
      size="xs"
      isModal={false}
      transparent
      title="You have priority"
      footer={<PassPriorityButton />}
    >
      <Text textAlign="center" color="fg">
        {isYourTurn ? (
          <span>It&apos;s your turn.</span>
        ) : (
          <span>It&apos;s your opponent&apos;s turn.</span>
        )}
        <br />
        Play a card or ability, or pass priority
      </Text>
    </Dialog>
  );
}
