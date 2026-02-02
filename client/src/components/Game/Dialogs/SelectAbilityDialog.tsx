import { SelectAbilityDialogState } from "@/types";
import { useLoadedGameContext } from "@/utils/LoadedGameContext";
import Button from "@/components/ui/Button";
import useDoAction from "@/utils/useDoAction";
import Dialog from "@/components/ui/Dialog";
import CardText from "../Card/CardText";
import Card from "../Card";
import { getCard } from "@/utils/gameStateQueries";
import { Box } from "@chakra-ui/react";

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
  const dialog = gameState.screen.largeDialog || gameState.screen.smallDialog;
  const { oid, owningCardId, canCancel, descriptions } = dialog as SelectAbilityDialogState;
  const doAction = useDoAction(oid);
  const card = getCard(gameState, owningCardId);

  const onCancelClick = () => doAction("Cancel");

  return (
    <Dialog
      compact={false}
      footer={
        canCancel && (
          <Button variant="secondary" onClick={onCancelClick}>
            Cancel
          </Button>
        )
      }
    >
      {card && (
        <Box marginBottom="1rem" marginTop="-12rem" display="flex" justifyContent="center">
          <Card card={{ ...card, isPlayable: true }} size="xl" isInteractable={false} />
        </Box>
      )}
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
