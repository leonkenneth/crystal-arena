import { CardState } from "./cards";
import { ObjectIdContainer } from "./oid";

export type TargetTypeAndId = {
  targetType: "Card"; // Probably needs Effects and so on
  cardId: number;
};

export type EffectState = {
  type: "TriggeredAbility" | "ActivatedAbility" | "CastRule";
  text: string;
  card: CardState;
  controllerId: number;
  targets: TargetTypeAndId[];
};

export type StackState = { effects: EffectState[]; oid: ObjectIdContainer };
