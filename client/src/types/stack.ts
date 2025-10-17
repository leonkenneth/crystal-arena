import { CardState } from "./cards";

export type TargetTypeAndId = {
  type: "Card"; // Probably needs Effects and so on
  id: number;
};

export type EffectState = {
  type: "TriggeredAbility" | "ActivatedAbility" | "CastRule";
  text: string;
  card: CardState;
  controllerId: number;
  target: TargetTypeAndId;
};

export type StackState = { effects: EffectState[] };
