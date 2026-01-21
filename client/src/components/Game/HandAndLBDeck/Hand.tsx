import { CardOutsideFieldState } from "@/types";
import CardInHand from "./CardInHand";
import { Box, VStack, HStack, Text } from "@chakra-ui/react";
import useHover from "@/utils/useHover";
import { useCallback, useState } from "react";

type Props = {
  cards?: CardOutsideFieldState[];
  toggleText?: string;
  showToggleText: boolean;
  onClickToggle?: () => void;
  onZoomOut?: () => void;
  zoomable?: boolean;
};

export default function Hand({
  cards,
  toggleText,
  onClickToggle,
  showToggleText,
  onZoomOut,
  zoomable = true,
}: Props) {
  const [isHovered, setIsHovered] = useState(false);
  const handleHoverIn = useCallback(() => {
    setIsHovered(true);
  }, []);
  const handleHoverOut = useCallback(() => {
    setIsHovered(false);
    onZoomOut?.();
  }, [onZoomOut]);
  const { ref } = useHover({
    triggerTimeout: 200,
    onHoverIn: handleHoverIn,
    onHoverOut: handleHoverOut,
    enabled: zoomable,
  });

  if (!cards || cards.length === 0) {
    return <span>No card in hand</span>;
  }

  const isAnyHovered = isHovered;

  return (
    <VStack w="full" pointerEvents="auto" gap={1} ref={ref}>
      <HStack
        w="full"
        justifyContent="center"
        overflowX="scroll"
        flexShrink={0}
        flexGrow={0}
        gap={2}
        style={{
          pointerEvents: "none",
          scrollSnapType: "x mandatory",
        }}
      >
        {cards.map((card) => {
          return (
            <div key={card.cardId} style={{ pointerEvents: "auto" }}>
              <CardInHand card={card} isZoomed={isHovered} />
            </div>
          );
        })}
      </HStack>
      {showToggleText && isAnyHovered && (
        <Box pointerEvents="auto" w="full">
          <Text
            color="fg.muted"
            fontSize="sm"
            cursor="pointer"
            textAlign="center"
            onClick={onClickToggle}
            _hover={{ textDecoration: "underline" }}
          >
            {toggleText}
          </Text>
        </Box>
      )}
    </VStack>
  );
}
