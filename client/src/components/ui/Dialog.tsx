import { Dialog as ChakraDialog, Portal } from "@chakra-ui/react";
import { useEffect } from "react";

type Props = {
    title?: string | React.ReactNode;
    footer?: React.ReactNode;
    children?: React.ReactNode;
    isModal?: boolean;
} & ChakraDialog.RootProps;

export default function Dialog({ title, children, footer, isModal = true, ...props }: Props) {
    return <ChakraDialog.Root 
    open={true}
    modal={isModal}
    closeOnInteractOutside={!isModal}
    preventScroll={isModal}
    size={props.size || "sm"}
    {...props}
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