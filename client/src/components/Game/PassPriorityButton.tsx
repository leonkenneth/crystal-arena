import Button from "@/components/ui/Button";
import { useLoadedGameContext } from "@/utils/LoadedGameContext";
import { get } from "@/utils/api";

export default function PassPriorityButton() {
  const { gameId, gameState } = useLoadedGameContext();
  const priorityEnabled = gameState.screen.smallDialog?.type === "Priority";
  const priorityCallbackId = gameState.currentDialog?.callbackId;
  const label = gameState.screen.stack.effects.length > 0 ? "Resolve" : "Pass";

  const passPriority = async () => {
    return get(
      `/games/${gameId}/callback/${priorityCallbackId}/${btoa(JSON.stringify({ Type: "PassPriority" }))}`
    );
  };

  return (
    <Button
      variant="primary"
      onClick={passPriority}
      disabled={!priorityEnabled || !priorityCallbackId}
    >
      {label}
    </Button>
  );
}
