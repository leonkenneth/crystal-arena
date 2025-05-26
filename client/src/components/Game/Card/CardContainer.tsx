import { Card as ChakraCard } from "@chakra-ui/react";
import { useState } from "react";
import { getCardSize, CardSize } from "./size";

export type Props = {
    children: React.ReactNode,
    isPlayable?: boolean,
    isTapped?: boolean,
    isSelected?: boolean,
    size?: CardSize,
    isInteractable?: boolean,
} & ChakraCard.RootProps;

export default function CardContainer({ children, isInteractable, isPlayable, isTapped, isSelected, size, ...props }: Props) {
    const actualSize = size || "sm";
    const { width, height } = getCardSize(actualSize);

    const [isHovered, setIsHovered] = useState(false);
    const isDisplayedHovered = isInteractable && isHovered;

    const getBorderColor = function () {
        if (isSelected) {
            return "blue.500";
        }
        
        if (isDisplayedHovered && isPlayable) {
            return "green.300";
        }

        if (isPlayable) {
            return "green.500";
        }

        return "gray.400";
    }

    const getTransform = function () {
        if (isTapped) {
            return "rotate(15deg) scale(0.85)";
        }

        return "rotate(0deg) scale(1)";
    }

    return <ChakraCard.Root
        transform={getTransform()}
        size="sm"
        w={`${width}px`}
        h={`${height}px`} borderRadius="md"
        overflow="hidden" flexShrink={0} flexGrow={0}
        bg="transparent"
        borderWidth="2px"
        boxShadow={isDisplayedHovered ? "lg" : "none"}
        boxShadowColor={getBorderColor()}
        borderColor={getBorderColor()}
        onPointerEnter={() => setIsHovered(true)}
        onPointerLeave={() => setIsHovered(false)}
        cursor={isInteractable ? "pointer" : "default"}
        {...props}>
        {children}
    </ChakraCard.Root>
}