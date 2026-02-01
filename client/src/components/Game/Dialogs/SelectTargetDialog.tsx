import { CardOutsideFieldState, SelectTargetDialogState } from "@/types";
import { useLoadedGameContext } from "@/utils/LoadedGameContext";
import { HStack, Text } from "@chakra-ui/react";
import Button from "@/components/ui/Button";
import useDoAction from "@/utils/useDoAction";
import Dialog from "@/components/ui/Dialog";
import { selection } from "@/utils/gameStateQueries";
import CardDialogBody from "./CardDialogBody";

function selectAttackerMessage(selection: CardOutsideFieldState[]) {
  if (selection.length === 0) {
    return "Skip attack";
  }

  if (selection.length === 1) {
    const selectedCard = selection[0];
    return `Attack with ${selectedCard.name}`;
  }

  return `Party attack with ${selection.length} forwards`;
}

function selectBlockerMessage(selection: CardOutsideFieldState[]) {
  if (selection.length === 0) {
    return "Don't block";
  }

  if (selection.length === 1) {
    const selectedCard = selection[0];
    return `Block with ${selectedCard.name}`;
  }

  throw new Error("Multiple blockers are not supported");
}

function validateTargetMessage(selection: CardOutsideFieldState[]) {
  if (selection.length === 0) {
    return "No target";
  }

  if (selection.length === 1) {
    const selectedCard = selection[0];
    return `Target ${selectedCard.name}`;
  }

  return `Validate ${selection.length} targets`;
}

function getValidatorInstructions(targetValidator: { message: string; minCount: number; maxCount: number }, selectedCards: CardOutsideFieldState[]) {
  const minCount = targetValidator.minCount;
  const maxCount = targetValidator.maxCount;

  if (minCount === maxCount) {
    return `Select exactly ${minCount} target(s) (currently ${selectedCards.length})`;
  }
  if (minCount === 0) {
    return `Select at most ${maxCount} target(s) (currently ${selectedCards.length})`;
  }

  return `Select at least ${minCount} and at most ${maxCount} targets (currently ${selectedCards.length})`;
}

export default function SelectTargetDialog() {
  const { gameState } = useLoadedGameContext();
  const { oid, canCancel, text, owningCard, targetValidator } = gameState.screen.smallDialog as SelectTargetDialogState;
  const doAction = useDoAction(oid);

  const onCancelClick = () => doAction("Cancel");
  const onOkClick = () => doAction("Done");

  let validateButtonText = "";
  const selectedCards = selection(gameState, { only: "yours" });
  const isValidatable = selectedCards.length >= targetValidator.minCount && selectedCards.length <= targetValidator.maxCount;
  const validatorInstructions = getValidatorInstructions(targetValidator, selectedCards);
  if (text === "Select an attacker.") {
    validateButtonText = selectAttackerMessage(selectedCards);
  } else if (text === "Select a blocker.") {
    validateButtonText = selectBlockerMessage(selectedCards);
  } else {
    validateButtonText = validateTargetMessage(selectedCards);
  }

  return (
    <Dialog
      title={text}
      footer={
        <HStack gap={3}>
          {canCancel && (
            <Button variant="secondary" onClick={onCancelClick}>
              Cancel
            </Button>
          )}
          <Button variant="primary" onClick={onOkClick} disabled={!isValidatable}>
            {validateButtonText}
          </Button>
        </HStack>
      }
      isModal={false}
    >
      <CardDialogBody card={owningCard} />
      <Text textAlign="center" fontSize="sm" color="fg">
        {validatorInstructions}
      </Text>
    </Dialog>
  );
}
