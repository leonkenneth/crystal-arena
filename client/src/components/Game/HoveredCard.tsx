import { ClientContext } from "@/utils/ClientContext";
import { useContext } from "react";
import Card from "./Card";
import { Button, Portal, Drawer as ChakraDialog, AbsoluteCenter, Flex } from "@chakra-ui/react";
import useHover from "@/utils/useHover";

export default function HoveredCard() {
    const { hoveredCard, setHoveredCard } = useContext(ClientContext);
    const { ref } = useHover({
        onHoverIn: () => {
            setHoveredCard(hoveredCard);
        },
        onHoverOut: () => {
            setHoveredCard(null);
        }
    });
    if (!hoveredCard) return null;

    return <ChakraDialog.Root 
    open={true}
    modal={false}
    closeOnInteractOutside={false}
    preventScroll={false}
    placement="end"
    size="xs"
    >
    <Portal>
    <ChakraDialog.Positioner pointerEvents="none">
        <ChakraDialog.Content ref={ref} boxShadow="none" borderColor="transparent" backgroundColor="transparent" pointerEvents="none" >
            <ChakraDialog.Body>
                <Flex justifyContent="center" alignItems="center" height="100%" width="100%">
                    <Card card={{
                        ...hoveredCard,
                        isTapped: false,
                        isFrozen: false,
                        isSelected: false,
                        isSelectedForCombat: false,
                        isTargetOfSpell: false,
                        isPlayable: false,
                    }} size="xl" />
                </Flex>
            </ChakraDialog.Body>
            </ChakraDialog.Content>
        </ChakraDialog.Positioner>
    </Portal>
</ChakraDialog.Root>
}