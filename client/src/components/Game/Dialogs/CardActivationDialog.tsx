import { useLoadedGameContext } from "@/utils/LoadedGameContext";
import Dialog from "@/components/ui/Dialog";
import { CardActivationDialogState } from "@/types";

export default function PriorityDialog() {
  const { gameState } = useLoadedGameContext();
  const dialog = gameState.screen.largeDialog as unknown as CardActivationDialogState;

  const { title } = dialog;

  // @ts-expect-error - children is not defined in the DialogProps type
  return <Dialog isModal={false} title={title} />;
}
