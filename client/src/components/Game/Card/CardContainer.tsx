import { Card as ChakraCard } from "@chakra-ui/react";
import { forwardRef, useState } from "react";
import { getCardSize, CardSize } from "./size";

export type Props = {
  children: React.ReactNode;
  isPlayable?: boolean;
  isTapped?: boolean;
  isSelected?: boolean;
  isSelectedForCombat?: boolean;
  size?: CardSize;
  isInteractable?: boolean;
  dataCardId?: string;
} & ChakraCard.RootProps;

export default forwardRef(function CardContainer(
  {
    dataCardId,
    children,
    isInteractable,
    isPlayable,
    isTapped,
    isSelected,
    isSelectedForCombat,
    size,
    ...props
  }: Props,
  ref: React.Ref<HTMLDivElement>
) {
  const actualSize = size || "sm";
  const { width, height } = getCardSize(actualSize);

  const [isHovered, setIsHovered] = useState(false);
  const isDisplayedHovered = isInteractable && isHovered;

  const getBorderColor = function () {
    if (isSelectedForCombat) {
      return "red.500";
    }

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
  };

  const getTransform = function () {
    if (isTapped) {
      return "rotate(15deg) scale(0.85)";
    }

    return "rotate(0deg) scale(1)";
  };

  return (
    <ChakraCard.Root
      transform={getTransform()}
      size="sm"
      w={`${width}px`}
      h={`${height}px`}
      transition="width 200ms ease, height 200ms ease"
      borderRadius="md"
      overflow="hidden"
      flexShrink={0}
      flexGrow={0}
      bg="transparent"
      borderWidth="2px"
      boxShadow={isDisplayedHovered ? "lg" : "none"}
      boxShadowColor={getBorderColor()}
      bgColor="gray.500"
      borderColor={getBorderColor()}
      onPointerEnter={() => setIsHovered(true)}
      onPointerLeave={() => setIsHovered(false)}
      cursor={isInteractable ? "pointer" : "default"}
      data-cardid={dataCardId}
      ref={ref}
      {...props}
    >
      {children}
    </ChakraCard.Root>
  );
});
