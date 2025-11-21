import { Dialog as ChakraDialog, Portal } from "@chakra-ui/react";

type Props = {
  title?: string | React.ReactNode;
  footer?: React.ReactNode;
  children?: React.ReactNode;
  isModal?: boolean;
  transparent?: boolean;
  compact?: boolean;
} & ChakraDialog.RootProps;

export default function Dialog({
  title,
  children,
  footer,
  isModal = true,
  transparent = true,
  compact = true,
  ...props
}: Props) {
  return (
    <ChakraDialog.Root
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
          <ChakraDialog.Content
            backgroundColor={transparent ? "transparent" : "cyan.950"}
            boxShadow="none"
            borderColor="transparent"
            pointerEvents={transparent ? "none" : "auto"}
          >
            <ChakraDialog.Header
              justifyContent="center"
              p={compact ? 0 : 4}
              pointerEvents={transparent ? "none" : "auto"}
            >
              {title && (
                <ChakraDialog.Title textStyle={transparent ? "outline" : undefined} color="fg">
                  {title}
                </ChakraDialog.Title>
              )}
            </ChakraDialog.Header>
            {children && (
              <ChakraDialog.Body
                textStyle={transparent ? "outline" : undefined}
                justifyContent="center"
                p={compact ? 0 : 4}
                pointerEvents={transparent ? "none" : "auto"}
              >
                {children}
              </ChakraDialog.Body>
            )}
            <ChakraDialog.Footer justifyContent="center" p={compact ? 2 : 4} pointerEvents="auto">
              {footer}
            </ChakraDialog.Footer>
          </ChakraDialog.Content>
        </ChakraDialog.Positioner>
      </Portal>
    </ChakraDialog.Root>
  );
}
