import { Dialog as ChakraDialog, Portal } from "@chakra-ui/react";
import { useEffect } from "react";

type Props = {
    title?: string | React.ReactNode;
    footer?: React.ReactNode;
    children?: React.ReactNode;
    isModal?: boolean;
    size?: "xs" | "sm" | "md" | "lg" | "xl";
}

export default function Dialog({ title, children, footer, isModal = true, size = "xs" }: Props) {
    return <ChakraDialog.Root 
    open={true}
    modal={isModal}
    closeOnInteractOutside={!isModal}
    preventScroll={isModal}
    size={size}
    >
    <Portal>
    {isModal && <ChakraDialog.Backdrop />}
    <ChakraDialog.Positioner pointerEvents={isModal ? "auto" : "none"}>
        <ChakraDialog.Content backgroundColor="cyan.950">
            <ChakraDialog.Header justifyContent="center">
                {title && <ChakraDialog.Title color="fg">{title}</ChakraDialog.Title>}
            </ChakraDialog.Header>
            {children && <ChakraDialog.Body>
                {children}
            </ChakraDialog.Body>}
        <ChakraDialog.Footer justifyContent="center">
            {footer}
        </ChakraDialog.Footer>
            </ChakraDialog.Content>
        </ChakraDialog.Positioner>
    </Portal>
</ChakraDialog.Root>;
}