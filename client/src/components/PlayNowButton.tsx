"use client";

import { post } from "@/utils/api";
import { useRouter } from "next/navigation";
import { Button } from "@chakra-ui/react";
import type { ButtonProps } from "@chakra-ui/react";

type Props = ButtonProps & {
  children: React.ReactNode;
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
