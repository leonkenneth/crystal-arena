import { SelectTargetDialogState } from "@/types";
import { useLoadedGameContext } from "@/utils/LoadedGameContext";
import { Button } from "@chakra-ui/react";
import useDoAction from "@/utils/useDoAction";
import Dialog from "@/components/ui/Dialog";

export default function SelectTargetDialog() {
  const { gameState } = useLoadedGameContext();
  const { oid, canCancel, text } = gameState.screen.smallDialog as SelectTargetDialogState;
  const doAction = useDoAction(oid);

  const onCancelClick = () => doAction("Cancel");
  const onOkClick = () => doAction("Done");

  return (
    <Dialog
      title={text}
      footer={
        <>
          {canCancel && <Button onClick={onCancelClick}>Cancel</Button>}
          <Button onClick={onOkClick}>OK</Button>
        </>
      }
      isModal={false}
    ></Dialog>
  );
}
