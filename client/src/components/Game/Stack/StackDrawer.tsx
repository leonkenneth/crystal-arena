import { Drawer } from "@chakra-ui/react"
import { useEffect } from "react";

type Props = {
    children: React.ReactNode,
    footer: React.ReactNode,
}

export default function StackDrawer({ children, footer }: Props) {
    return <Drawer.Root open={true} placement="start" preventScroll={false}>
    <Drawer.Positioner pointerEvents="none">
        <Drawer.Content>
            <Drawer.Header>
                <Drawer.Title>Stack</Drawer.Title>
            </Drawer.Header>
            <Drawer.Body>
                {children}
            </Drawer.Body>
            <Drawer.Footer>
                {footer}
            </Drawer.Footer>
        </Drawer.Content>
    </Drawer.Positioner>
</Drawer.Root>
}