import { ObjectIdContainer } from "./oid";

export type PlayerState = {
    handCount: number;
    mainDeckCount: number;
    breakZoneCount: number;
    life: number;
    isActive: boolean;
    isSearchInProgress: boolean;
    playerName: string;
    oid: ObjectIdContainer;
}

export type ManaPoolState = {
    light: number,
    water: number,
    dark: number,
    fire: number,
    wind: number,
    ice: number,
    earth: number,
    lightning: number,
    crystal: number,
    colorless: number,
    multi: number
}