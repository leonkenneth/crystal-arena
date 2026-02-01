import { SelectAbilityDialogState } from "@/types";
import { useLoadedGameContext } from "@/utils/LoadedGameContext";
import Button from "@/components/ui/Button";
import useDoAction from "@/utils/useDoAction";
import Dialog from "@/components/ui/Dialog";
import CardText from "../Card/CardText";
import CardDialogBody from "./CardDialogBody";

function AbilityButton({ description, onClick }: { description: string; onClick: () => void }) {
  return (
    <Button
      variant="secondary"
      display="block"
      onClick={onClick}
      marginBottom="0.5rem"
      width="100%"
    >
      <CardText text={description} />
    </Button>
  );
}

export default function SelectAbilityDialog() {
  const { gameState } = useLoadedGameContext();
  const { oid, canCancel, descriptions, owningCard } = (gameState.screen.largeDialog || gameState.screen.smallDialog) as SelectAbilityDialogState;
  const doAction = useDoAction(oid);

  const onCancelClick = () => doAction("Cancel");

  return (
    <Dialog
      title="Select an ability"
      compact={false}
      footer={
        canCancel && (
          <Button variant="secondary" onClick={onCancelClick}>
            Cancel
          </Button>
        )
      }
    >
      <CardDialogBody card={owningCard} />
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
