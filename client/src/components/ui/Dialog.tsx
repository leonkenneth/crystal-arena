import { Dialog as ChakraDialog, Portal } from "@chakra-ui/react";
import { useEffect } from "react";

type Props = {
    title?: string | React.ReactNode;
    footer?: React.ReactNode;
    children?: React.ReactNode;
    isModal?: boolean;
}

export default function Dialog({ title, children, footer, isModal = true }: Props) {
    useEffect(() => {
        // Ugly hack to be able to scroll the page on non-modal dialogs
        if (!isModal) {
            document.body.style.overflow = "hidden";
        } else {
            document.body.style.overflow = "auto";
        }

        return () => {
            document.body.style.overflow = "auto";
        }
    }, [isModal]);

    return <ChakraDialog.Root 
    open={true}
    modal={isModal}
    closeOnInteractOutside={!isModal}
    >
    <Portal>
    {isModal && <ChakraDialog.Backdrop />}
    <ChakraDialog.Positioner pointerEvents={isModal ? "auto" : "none"}>
        <ChakraDialog.Content>
            <ChakraDialog.Header>
                {title && <ChakraDialog.Title color="fg">{title}</ChakraDialog.Title>}
            </ChakraDialog.Header>
            {children && <ChakraDialog.Body>
                {children}
            </ChakraDialog.Body>}
        <ChakraDialog.Footer>
            {footer}
        </ChakraDialog.Footer>
            </ChakraDialog.Content>
        </ChakraDialog.Positioner>
    </Portal>
</ChakraDialog.Root>;
}