export type ObjectIdContainer = {
    oid: string;
}

export type ManaPoolState = {
    light: number,
    water: number,
    dark: number,
    fire: number,
    wind: number,
    ice: number,
    earth: number,
    lightning: number,
    crystal: number,
    colorless: number,
    multi: number
}

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

export type SlotState = {
    permanents: CardOnFieldState[],
}

export type BattlefieldRowState = {
    slots: SlotState[]
}

export type BattlefieldState = {
    row1: BattlefieldRowState,
    row2: BattlefieldRowState,
}

export type PlayerState = {
    handCount: number;
    mainDeckCount: number;
    breakZoneCount: number;
    life: number;
    isActive: boolean;
    playerName: string;
    oid: ObjectIdContainer;
}

export type ZoneState = { cards: CardOutsideFieldState[] }
export type BreakZoneState = ZoneState & { type: "BreakZone" }
export type HandState = ZoneState & { type: "Hand" }
export type MainDeckState = ZoneState & { type: "MainDeck" }
export type RemoveFromPlayState = ZoneState & { type: "RemoveFromPlay" }
export type DamageZoneState = ZoneState & { type: "DamageZone" }

export type ZonesState = {
    opponentsBreakZone: BreakZoneState,
    opponentsHand: HandState,
    opponentsMainDeck: MainDeckState,
    opponentsRemoveFromPlay: RemoveFromPlayState,
    opponentsDamageZone: DamageZoneState,
    yourBreakZone: BreakZoneState,
    yourHand: HandState,
    yourMainDeck: MainDeckState,
    yourRemoveFromPlay: RemoveFromPlayState,
    yourDamageZone: DamageZoneState,
}

export type StepState = {
    name: string;
    isCurrent: boolean;
    autoPass: string;
    oid: ObjectIdContainer;
}

export type TargetTypeAndId = {
    type: "Card", // Probably needs Effects and so on
    id: number;
}

export type EffectState = {
    type: "TriggeredAbility" | "ActivatedAbility" | "CastRule",
    text: string;
    card: CardState;
    controllerId: number;
    target: TargetTypeAndId;
}

export type QuitGameDialogState = { type: "QuitGame" }
export type StackState = { effects: EffectState[] }
export type StepsState = { steps: StepState[] }
export type TurnState = { number: number }

export type PlayScreenState = {
    yourManaPool: ManaPoolState,
    opponentsManaPool: ManaPoolState,
    opponentsBattlefield: BattlefieldState,
    yourBattlefield: BattlefieldState,
    you: PlayerState,
    opponent: PlayerState,
    searchInProgressMessage: string | null,
    zones: ZonesState,
    quitGameDialog: QuitGameDialogState,
    stack: StackState,
    steps: StepsState,
    turnNumber: TurnState
    smallDialog: SmallDialogState | null,
    largeDialog: LargeDialogState | null
}

export type MessageBoxState = {
    title: string;
    message: string;
    buttons: "YesNo" | "Cancel" | "CancelOk";
    callbackId: string;
}

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
};

export type SmallDialogState = PriorityDialogState | SelectTargetDialogState;

export type SelectAbilityDialogState = {
    type: "SelectAbility";
    oid: ObjectIdContainer;
    descriptions: string[];
    canCancel: boolean;
}

export type NextTurnDialogState = {
    type: "NextTurnDialog";
    turnNumber: number;
    message: string;
}

export type LargeDialogState = SelectAbilityDialogState | NextTurnDialogState | null;

export type LoadedGameState = { 
    loaded: true,
    id: string,
    screen: PlayScreenState,
    messageBox: MessageBoxState,
    currentDialog: CurrentDialogState | null
}

export type GameState = { loaded: false } | LoadedGameState;