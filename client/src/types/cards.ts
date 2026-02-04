import { ObjectIdContainer } from "./oid";

type PlayZone = "Battlefield" | "Stack" | "BreakZone";

export type PlayableActivationState = {
  abilityId: string;
  playZone: PlayZone;
};

type CardType = "forward" | "backup" | "summon" | "monster";

type BaseCardState = {
  type: "Permanent" | "Spell" | "SelectableCard";
  cardId: number;
  cardTypes: CardType[];
  jobs: string[];
  categories: string[];
  name: string;
  hasXInCost: boolean;
  manaCost: string;
  text: string;
  power: number;
  toughness: number;
  basePower: number;
  baseToughness: number;
  damage: number;
  isVisibleInUi: boolean;
  colors: string[];
  isTapped: boolean;
  hasSummoningSickness: boolean;
  serial: string;
  playableActivations?: PlayableActivationState[];
};

export type CardOnFieldState = BaseCardState & {
  isFrozen: boolean;
  isPlayable: boolean;
  isTargetOfSpell: boolean;
  isSelectedForCombat: boolean;
  isSelected: boolean;
  oid: ObjectIdContainer;
};

export type CardOutsideFieldState = BaseCardState & {
  isPlayable: boolean;
  isSelected: boolean;
  oid: ObjectIdContainer;
};

export type CardState = CardOnFieldState | CardOutsideFieldState;
