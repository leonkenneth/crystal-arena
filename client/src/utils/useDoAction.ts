import { useCallback } from "react";
import { useLoadedGameContext } from "./LoadedGameContext";
import { get } from "./api";
import { ObjectIdContainer } from "@/types";

function capitalize(str: string) {
    return str.charAt(0).toUpperCase() + str.slice(1);
}

export async function doAction(gameId: string, oid: ObjectIdContainer, action: string, otherParams?: {}) {
    const capitalizedParams = Object.fromEntries(
        Object.entries(otherParams || {}).map(([key, value]) => [capitalize(key), value])
    );
    const message = { Type: action, ...capitalizedParams };
    const base64message = btoa(JSON.stringify(message));
    await get(`/games/${gameId}/oidcallback/${oid.oid}/${base64message}`);
}

export default function useDoAction<T extends {} | undefined>(oid: ObjectIdContainer) {
    const { gameId, refresh } = useLoadedGameContext();

    return useCallback(async (action: string, otherParams?: T) => {
        doAction(gameId, oid, action, otherParams);
        refresh();
    }, [oid.oid, gameId, refresh]);
}