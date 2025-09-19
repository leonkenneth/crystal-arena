import { Dialog as ChakraDialog, Portal } from "@chakra-ui/react"
import { useEffect } from "react";
import Dialog from "@/components/ui/Dialog";
type Props = {
    children: React.ReactNode,
    footer: React.ReactNode,
}

export default function StackDrawer({ children, footer }: Props) {
    return <ChakraDialog.Root 
    open={true}
    modal={false}
    closeOnInteractOutside={false}
    preventScroll={false}
    placement="center"
    size="xs"
    >
    <Portal>
    <ChakraDialog.Positioner pointerEvents="none">
        <ChakraDialog.Content className="no-pointer-events" boxShadow="none" borderColor="transparent" backgroundColor="transparent" transform="translateX(-30vw)" pointerEvents="none">
        <ChakraDialog.Header justifyContent="center" pointerEvents="none">
            <ChakraDialog.Title color="fg" textStyle="outline">Stack</ChakraDialog.Title>
        </ChakraDialog.Header>
            <ChakraDialog.Body pointerEvents="none">
                <div className="pointer-events-auto">
                    {children}
                </div>
            </ChakraDialog.Body>
        <ChakraDialog.Footer justifyContent="center" pointerEvents="none">
            <div className="pointer-events-auto">
                {footer}
            </div>
        </ChakraDialog.Footer>
            </ChakraDialog.Content>
        </ChakraDialog.Positioner>
    </Portal>
</ChakraDialog.Root>
}