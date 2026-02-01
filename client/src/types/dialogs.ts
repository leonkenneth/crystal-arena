import { ObjectIdContainer } from "./oid";
import { CardState } from "./cards";

export type QuitGameDialogState = { type: "QuitGame" };

export type MessageBoxState = {
  title: string;
  message: string;
  buttons: "YesNo" | "Cancel" | "CancelOk";
  callbackId: string;
};

export type CurrentDialogState = {
  callbackId: string;
};

export type PriorityDialogState = {
  callbackId: string;
  type: "Priority";
};

export type SelectTargetDialogState = {
  oid: ObjectIdContainer;
  type: "SelectTarget";
  canCancel: boolean;
  text: string;
  instructions: string;
  triggerMessage: string;
  owningCard: CardState;
  targetValidator: {
    message: string;
    minCount: number;
    maxCount: number;
  };
};

export type SelectAbilityDialogState = {
  type: "SelectAbility";
  oid: ObjectIdContainer;
  descriptions: string[];
  canCancel: boolean;
  owningCard: CardState;
};

export type NextTurnDialogState = {
  type: "NextTurnDialog";
  turnNumber: number;
  message: string;
};

export type GameResultsDialogState = {
  type: "GameResults";
  message: string;
};

export type CardActivationDialogState = {
  type: "CardActivation";
  title: string;
};

export type SmallDialogState =
  | PriorityDialogState
  | SelectTargetDialogState
  | CardActivationDialogState;
export type LargeDialogState =
  | SelectAbilityDialogState
  | NextTurnDialogState
  | GameResultsDialogState;
