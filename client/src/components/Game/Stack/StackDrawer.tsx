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
        <ChakraDialog.Content boxShadow="none" borderColor="transparent" backgroundColor="transparent" transform="translateX(-30vw)">
        <ChakraDialog.Header justifyContent="center">
            <ChakraDialog.Title color="fg">Stack</ChakraDialog.Title>
        </ChakraDialog.Header>
            <ChakraDialog.Body>
                {children}
            </ChakraDialog.Body>
        <ChakraDialog.Footer justifyContent="center" pointerEvents="auto">
            {footer}
        </ChakraDialog.Footer>
            </ChakraDialog.Content>
        </ChakraDialog.Positioner>
    </Portal>
</ChakraDialog.Root>
}