import { ObjectIdContainer } from "./oid";

type CardType = "forward" | "backup" | "summon" | "monster";

type BaseCardState = {
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
    isVisibleInUi: boolean;
    colors: string[];
    isTapped: boolean;
    hasSummoningSickness: boolean;
    serial: string;
}

export type CardOnFieldState = BaseCardState & {
    isFrozen: boolean,
    isPlayable: boolean,
    isTargetOfSpell: boolean,
    isSelectedForCombat: boolean,
    isSelected: boolean,
    oid: ObjectIdContainer
}

export type CardOutsideFieldState = BaseCardState & {
    isPlayable: boolean,
    isSelected: boolean,
    oid: ObjectIdContainer
}

export type CardState = CardOnFieldState | CardOutsideFieldState;
