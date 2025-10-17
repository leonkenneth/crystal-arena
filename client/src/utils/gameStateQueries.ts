import {
  LoadedGameState,
  CardState,
  ZoneType,
  BattlefieldRowState,
  TargetTypeAndId,
} from "@/types";

export function getCard(gameState: LoadedGameState, id: number) {
  return cards(gameState).find((card) => card.cardId === id) || null;
}

export function targets(gameState: LoadedGameState): TargetTypeAndId[] {
  return gameState.screen.stack.effects.flatMap((effect) => effect.targets);
}

export function isTargeted(gameState: LoadedGameState, id: number) {
  return targets(gameState).some((target) => target.cardId === id);
}

export function zones(
  gameState: LoadedGameState,
  options: { only?: "yours" | "opponents" } = {}
): ZoneState[] {
  const zones = gameState.screen.zones;
  const yourZones = [
    zones.yourBreakZone,
    zones.yourHand,
    zones.yourMainDeck,
    zones.yourRemoveFromPlay,
    zones.yourDamageZone,
  ];
  const opponentsZones = [
    zones.opponentsBreakZone,
    zones.opponentsHand,
    zones.opponentsMainDeck,
    zones.opponentsRemoveFromPlay,
    zones.opponentsDamageZone,
  ];

  if (options.only === "yours") {
    return yourZones;
  }
  if (options.only === "opponents") {
    return opponentsZones;
  }
  return [...yourZones, ...opponentsZones];
}

export function battleFieldRows(
  gameState: LoadedGameState,
  options: { only?: "yours" | "opponents" } = {}
): BattlefieldRowState[] {
  const yourBattlefieldRows = [
    gameState.screen.yourBattlefield.row1,
    gameState.screen.yourBattlefield.row2,
  ];
  const opponentsBattlefieldRows = [
    gameState.screen.opponentsBattlefield.row1,
    gameState.screen.opponentsBattlefield.row2,
  ];

  if (options.only === "yours") {
    return yourBattlefieldRows;
  }

  if (options.only === "opponents") {
    return opponentsBattlefieldRows;
  }
  return [...yourBattlefieldRows, ...opponentsBattlefieldRows];
}

export function cards(
  gameState: LoadedGameState,
  options: { only?: "yours" | "opponents" } = {}
): CardState[] {
  const zoneCards = zones(gameState, options).flatMap((zone) => zone.cards);
  const battleFieldCards = battleFieldRows(gameState, options).flatMap((battleFieldRow) => {
    return battleFieldRow.slots.flatMap((slot) => slot.permanents);
  });
  return [...zoneCards, ...battleFieldCards];
}

export function selection(
  gameState: LoadedGameState,
  options: { only?: "yours" | "opponents" } = {}
): CardState[] {
  // @ts-ignore
  return cards(gameState, options).filter(
    (card: CardState) => card.isSelected || card.isSelectedForCombat
  );
}

export function getCardZone(gameState: LoadedGameState, id: number): ZoneType | null {
  for (const zone of Object.values(gameState.screen.zones)) {
    for (const card of zone.cards) {
      if (card.cardId === id) {
        return zone.type;
      }
    }
  }
  const battleFieldCards = [
    gameState.screen.yourBattlefield.row1,
    gameState.screen.yourBattlefield.row2,
    gameState.screen.opponentsBattlefield.row1,
    gameState.screen.opponentsBattlefield.row2,
  ].flatMap((battleFieldRow) => {
    return battleFieldRow.slots.flatMap((slot) => slot.permanents);
  });
  for (const card of battleFieldCards) {
    if (card.cardId === id) {
      return "Battlefield";
    }
  }
  return null;
}
