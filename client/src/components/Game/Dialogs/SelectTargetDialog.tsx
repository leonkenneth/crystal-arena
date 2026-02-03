import { CardOutsideFieldState, SelectTargetDialogState } from "@/types";
import { useLoadedGameContext } from "@/utils/LoadedGameContext";
import { HStack } from "@chakra-ui/react";
import Button from "@/components/ui/Button";
import useDoAction from "@/utils/useDoAction";
import Dialog from "@/components/ui/Dialog";
import { selection } from "@/utils/gameStateQueries";

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

function getInstructionsFromValidator(minCount: number, maxCount: number): string {
  if (minCount === maxCount) {
    return `Select exactly ${minCount} card${minCount !== 1 ? "s" : ""}.`;
  }
  if (minCount === 0) {
    return `Select up to ${maxCount} card${maxCount !== 1 ? "s" : ""}.`;
  }
  return `Select ${minCount} to ${maxCount} cards.`;
}

function isSelectionValid(selectionCount: number, minCount: number, maxCount: number): boolean {
  return selectionCount >= minCount && selectionCount <= maxCount;
}

export default function SelectTargetDialog() {
  const { gameState } = useLoadedGameContext();
  const { oid, canCancel, text, instructions, targetValidator } = gameState.screen
    .smallDialog as SelectTargetDialogState;
  const doAction = useDoAction(oid);

  const onCancelClick = () => doAction("Cancel");
  const onOkClick = () => doAction("Done");

  const selectedCards = selection(gameState, { only: "yours" });
  const selectionCount = selectedCards.length;

  let validateButtonText = "";
  if (text === "Select an attacker.") {
    validateButtonText = selectAttackerMessage(selectedCards);
  } else if (text === "Select a blocker.") {
    validateButtonText = selectBlockerMessage(selectedCards);
  } else {
    validateButtonText = validateTargetMessage(selectedCards);
  }

  const displayInstructions =
    instructions ||
    (targetValidator
      ? getInstructionsFromValidator(targetValidator.minCount, targetValidator.maxCount)
      : null);

  const isSubmitDisabled = targetValidator
    ? !isSelectionValid(selectionCount, targetValidator.minCount, targetValidator.maxCount)
    : false;

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
          <Button variant="primary" onClick={onOkClick} disabled={isSubmitDisabled}>
            {validateButtonText}
          </Button>
        </HStack>
      }
      isModal={false}
    >
      {displayInstructions && <p className="text-center">{displayInstructions}</p>}
    </Dialog>
  );
}
