import { Card as ChakraCard } from "@chakra-ui/react";
import { useState } from "react";

type CardContainerProps = {
    children: React.ReactNode,
    isPlayable?: boolean,
    isTapped?: boolean,
    isSelected?: boolean,
    size?: "small" | "large",
} & ChakraCard.RootProps;

export default function CardContainer({ children, isPlayable, isTapped, isSelected, size, ...props }: CardContainerProps) {
    const aspectRatio = 1.4;
    const actualSize = size || "small";
    const baseWidth = actualSize === "small" ? 80 : 120;

    const [isHovered, setIsHovered] = useState(false);
    let borderColor = "gray.400";
    if (isSelected) {
        borderColor = "blue.500";
    } else if (isHovered && isPlayable) {
        borderColor = "green.300";
    } else if (isPlayable) {
        borderColor = "green.500";
    }

    return <ChakraCard.Root
        transform={isTapped ? "rotate(15deg) scale(0.85)" : "rotate(0deg) scale(1)"}
        size="sm"
        w={`${baseWidth}px`}
        h={`${baseWidth * aspectRatio}px`} borderRadius="md"
        overflow="hidden" flexShrink={0} flexGrow={0}
        bg="transparent"
        borderWidth="2px"
        borderColor={borderColor}
        onPointerEnter={() => setIsHovered(true)}
        onPointerLeave={() => setIsHovered(false)}
        {...props}>
        {children}
    </ChakraCard.Root>
}