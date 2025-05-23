import { CurrentDialogState, MessageBoxState, LargeDialogState, SmallDialogState, QuitGameDialogState } from "./dialogs"
import { ZonesState, BattlefieldState } from "./zones"
import { ManaPoolState, PlayerState } from "./player"
import { StackState } from "./stack"
import { StepsState } from "./steps"

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

export type LoadedGameState = { 
    loaded: true,
    id: string,
    screen: PlayScreenState,
    messageBox: MessageBoxState,
    currentDialog: CurrentDialogState | null
}

export type GameState = { loaded: false } | LoadedGameState;