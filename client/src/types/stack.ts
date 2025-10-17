import { CardState } from "./cards";

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

export type StackState = { effects: EffectState[] };
