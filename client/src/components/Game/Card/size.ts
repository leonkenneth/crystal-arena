const aspectRatio = 1.4;

export type CardSize = "xs" | "sm" | "md" | "lg";

function getBaseWidth(size: CardSize) {
    if (size === "xs") {
        return 60;
    }
    if (size === "sm") {
        return 80;
    }

    if (size === "md") {
        return 100;
    }
    if (size === "lg") {
        return 120;
    }

    return 100;
}

export const getCardSize = (size:   CardSize) => {
    const baseWidth = getBaseWidth(size);
    const width = baseWidth;
    const height = baseWidth * aspectRatio;
    return { width, height };
}

