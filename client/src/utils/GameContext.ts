import { LoadedGameState } from "@/types";
import { createContext, useContext } from "react";

export const GameContext = createContext<GameContextType>({
    gameId: "",
    gameState: undefined,
    refresh: () => {},
});

export type GameContextType = {
    gameId: string;
    gameState: LoadedGameState | undefined;
    refresh: () => void;
}

export function useLoadedGameContext() {
    const { gameId, gameState, refresh } = useContext(GameContext);
    if (!gameState) {
        throw new Error("Game state not loaded");
    }
    return { gameId, gameState, refresh };
}