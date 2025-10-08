"use client";

import { useQuery, useQueryClient } from "@tanstack/react-query";
import { get } from "@/utils/api";
import { GameState } from "@/types";
import GameContent from "./GameContent";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { LoadedGameContext } from "@/utils/LoadedGameContext";

function Game({ id }: { id: string }) {
    const refreshInterval = 1000;
    const queryClient = useQueryClient();
    const queryResult = useQuery<GameState>({
        queryKey: ["game", id],
        queryFn: async ({ signal }) => {
            const response = await get(`/games/${id}`, { signal });
            return response.json();
        },
        placeholderData: (prev) => prev,
        refetchInterval: refreshInterval,
        refetchIntervalInBackground: true,
    });
    const { data: gameState, isFetching, isError, isRefetching } = queryResult;

    // Polling is handled by react-query's refetchInterval. No manual timers.

    // @ts-expect-error - error property may not exist on GameState type
    if (gameState?.error?.match(/not found/i)) {
        return <div>Game not found</div>;
    }

    if (isFetching && !isRefetching) {
        return <div>Loading...</div>;
    }

    if (isError) {
        return <div>Error loading game</div>;
    }

    if (gameState === undefined) {
        throw new Error("Game not found");
    }

    if (!gameState.loaded) {
        return <div>Loading...</div>;
    }

    return <LoadedGameContext.Provider value={{ 
        gameId: id, 
        gameState,
        refresh: () => {
            // Cancel any in-flight request for this query, then trigger a refetch
            queryClient.cancelQueries({ queryKey: ["game", id] });
            queryClient.invalidateQueries({ queryKey: ["game", id] });
        }
    }}>
        <GameContent />
    </LoadedGameContext.Provider>;
}

export default function GameWithQueryProvider({ id }: { id: string }) {
    const queryClient = new QueryClient();
    return <QueryClientProvider client={queryClient}>
        <Game id={id} />
    </QueryClientProvider>;
}