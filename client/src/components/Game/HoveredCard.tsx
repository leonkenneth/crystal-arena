import { ClientContext } from "@/utils/ClientContext";
import { useContext } from "react";
import Card from "./Card";
import { Portal, Drawer as ChakraDialog, Flex } from "@chakra-ui/react";

export default function HoveredCard() {
    const { hoveredCard } = useContext(ClientContext);
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
        <ChakraDialog.Content boxShadow="none" borderColor="transparent" backgroundColor="transparent" pointerEvents="none" >
            <ChakraDialog.Body pointerEvents="none">
                <Flex justifyContent="center" alignItems="center" height="100%" width="100%">
                    <Card card={{
                        ...hoveredCard,
                        isTapped: false,
                        isFrozen: false,
                        isSelected: false,
                        isSelectedForCombat: false,
                        isTargetOfSpell: false,
                        isPlayable: false,
                    }} size="xl" displayHoverCard={false}/>
                </Flex>
            </ChakraDialog.Body>
            </ChakraDialog.Content>
        </ChakraDialog.Positioner>
    </Portal>
</ChakraDialog.Root>
}