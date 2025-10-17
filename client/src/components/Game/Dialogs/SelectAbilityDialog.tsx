import { SelectAbilityDialogState } from "@/types";
import { useLoadedGameContext } from "@/utils/LoadedGameContext";
import { Button } from "@chakra-ui/react";
import useDoAction from "@/utils/useDoAction";
import Dialog from "@/components/ui/Dialog";
import CardText from "../Card/CardText";

function AbilityButton({ description, onClick }: { description: string; onClick: () => void }) {
  return (
    <Button display="block" onClick={onClick} marginBottom="0.5rem" width="100%">
      <CardText text={description} />
    </Button>
  );
}

export default function SelectAbilityDialog() {
  const { gameState } = useLoadedGameContext();
  const dialog = gameState.screen.largeDialog || gameState.screen.smallDialog;
  const { oid, canCancel, descriptions } = dialog as SelectAbilityDialogState;
  const doAction = useDoAction(oid);

  const onCancelClick = () => doAction("Cancel");

  return (
    <Dialog
      title="Select an ability"
      footer={canCancel && <Button onClick={onCancelClick}>Cancel</Button>}
    >
      {descriptions.map((description, index) => (
        <AbilityButton
          key={description}
          description={description}
          onClick={() => doAction("SetSelectedIndex", { index })}
        />
      ))}
    </Dialog>
  );
}
