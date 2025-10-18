import { Button as ChakraButton } from "@chakra-ui/react";
import type { ButtonProps as ChakraButtonProps } from "@chakra-ui/react";

type ButtonVariant = "primary" | "secondary";

type Props = ChakraButtonProps & {
  children: React.ReactNode;
  variant?: ButtonVariant;
};

export default function Button({ children, variant = "primary", ...props }: Props) {
  const variantStyles = getVariantStyles(variant);

  return (
    <ChakraButton {...variantStyles} {...props}>
      {children}
    </ChakraButton>
  );
}

function getVariantStyles(variant: ButtonVariant): ChakraButtonProps {
  const baseStyles: ChakraButtonProps = {
    borderRadius: "full",
    transition: "all 0.2s",
    display: "flex",
    alignItems: "center",
    justifyContent: "center",
    fontWeight: "medium",
    fontSize: { base: "sm", sm: "md" },
    h: { base: 10, sm: 12 },
    px: { base: 4, sm: 5 },
    w: { base: "full", sm: "auto" },
    cursor: "pointer",
  };

  switch (variant) {
    case "primary":
      return {
        ...baseStyles,
        bg: "white",
        color: "black",
        border: "none",
        _hover: { bg: "gray.300" },
      };
    case "secondary":
      return {
        ...baseStyles,
        bg: "transparent",
        color: "white",
        border: "1px solid",
        borderColor: "whiteAlpha.200",
        _hover: { bg: "whiteAlpha.100", borderColor: "transparent" },
      };
  }
}
