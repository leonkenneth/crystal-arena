import { CardState } from "@/types";
import { createContext } from "react";

export const ClientContext = createContext<ClientContextType>({
    hoveredCard: null,
    setHoveredCard: () => {}
});

export type ClientContextType = {
    hoveredCard: null | CardState;
    setHoveredCard: (card: null | CardState) => void;
}