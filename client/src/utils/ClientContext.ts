import { CardState } from "@/types";
import { createContext } from "react";

export const ClientContext = createContext<ClientContextType>({
    hoveredCard: null,
    setHoveredCard: () => {},
    draggedCard: null,
    setDraggedCard: () => {}
});

export type ClientContextType = {
    hoveredCard: null | CardState;
    setHoveredCard: (card: null | CardState) => void;
    draggedCard: null | CardState;
    setDraggedCard: (card: null | CardState) => void;
}