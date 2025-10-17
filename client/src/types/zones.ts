import { CardOnFieldState, CardOutsideFieldState } from "./cards";

export type SlotState = {
  permanents: CardOnFieldState[];
};

export type BattlefieldRowState = {
  slots: SlotState[];
};

export type BattlefieldState = {
  row1: BattlefieldRowState;
  row2: BattlefieldRowState;
};

export type ZoneState = { cards: CardOutsideFieldState[] };
export type BreakZoneState = ZoneState & { type: "BreakZone" };
export type HandState = ZoneState & { type: "Hand" };
export type MainDeckState = ZoneState & { type: "MainDeck" };
export type RemoveFromPlayState = ZoneState & { type: "RemoveFromPlay" };
export type DamageZoneState = ZoneState & { type: "DamageZone" };

export type ZonesState = {
  opponentsBreakZone: BreakZoneState;
  opponentsHand: HandState;
  opponentsMainDeck: MainDeckState;
  opponentsRemoveFromPlay: RemoveFromPlayState;
  opponentsDamageZone: DamageZoneState;
  yourBreakZone: BreakZoneState;
  yourHand: HandState;
  yourMainDeck: MainDeckState;
  yourRemoveFromPlay: RemoveFromPlayState;
  yourDamageZone: DamageZoneState;
};
