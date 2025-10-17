import { LoadedGameState } from "@/types";
import { createContext, useContext } from "react";

export const LoadedGameContext = createContext<LoadedGameContextType>({
  gameId: "",
  gameState: undefined,
  refresh: () => {},
});

export type LoadedGameContextType = {
  gameId: string;
  gameState: LoadedGameState | undefined;
  refresh: () => void;
};

export function useLoadedGameContext() {
  const { gameId, gameState, refresh } = useContext(LoadedGameContext);
  if (!gameState) {
    throw new Error("Game state not loaded");
  }
  return { gameId, gameState, refresh };
}
