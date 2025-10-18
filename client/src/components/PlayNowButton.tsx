"use client";

import { post } from "@/utils/api";
import { useRouter } from "next/navigation";
import Button from "@/components/ui/Button";
import type { ButtonProps as ChakraButtonProps } from "@chakra-ui/react";

type Props = ChakraButtonProps & {
  children: React.ReactNode;
  variant?: "primary" | "secondary";
};

export default function PlayNowButton({ children, ...props }: Props) {
  const router = useRouter();

  const onPlayNowClick = async () => {
    const response = await post("/internal/games");
    const data = await response.json();
    router.push(`/games/${data.uuid}`);
  };

  return (
    <Button onClick={onPlayNowClick} {...props}>
      {children}
    </Button>
  );
}
