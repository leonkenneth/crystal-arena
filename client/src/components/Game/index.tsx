"use client";

import { useQuery, useQueryClient } from "@tanstack/react-query";
import { get } from "@/utils/api";
import { GameState } from "@/types";
import GameContent from "./GameContent";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { GameContext } from "@/utils/GameContext";
import { useEffect } from "react";

function Game({ id }: { id: string }) {
    const refreshInterval = 1000;
    const queryClient = useQueryClient();
    const queryResult = useQuery<GameState>({
        queryKey: ["game", id],
        queryFn: async () => {
            const response = await get(`/games/${id}`);
            return response.json();
        },
        placeholderData: (prev) => prev
    });
    const { data: gameState, isFetching, isError, isRefetching } = queryResult;
    console.log(JSON.stringify(queryResult));

    useEffect(() => {
        let timeoutId: NodeJS.Timeout;
        
        const pollGame = async () => {
            await queryClient.invalidateQueries({ queryKey: ["game", id] });
            timeoutId = setTimeout(pollGame, refreshInterval);
        };

        pollGame();

        return () => {
            clearTimeout(timeoutId);
        };
    }, [id, queryClient]);

    // @ts-ignore
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

    return <GameContext.Provider value={{ 
        gameId: id, 
        gameState,
        refresh: () => queryClient.invalidateQueries({ queryKey: ["game", id] })
    }}>
        <GameContent />
    </GameContext.Provider>;
}

export default function GameWithQueryProvider({ id }: { id: string }) {
    const queryClient = new QueryClient();
    return <QueryClientProvider client={queryClient}>
        <Game id={id} />
    </QueryClientProvider>;
}