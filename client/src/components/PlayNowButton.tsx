"use client";

import { post } from "@/utils/api";
import { useRouter } from "next/navigation";
import Button from "@/components/ui/Button";
import { Link, Text, VStack, type ButtonProps as ChakraButtonProps } from "@chakra-ui/react";
import { useState } from "react";

type Props = ChakraButtonProps & {
  children: React.ReactNode;
  variant?: "primary" | "secondary";
};

// eslint-disable-next-line react-compiler/react-compiler
export default function PlayNowButton({ children, ...props }: Props) {
  const router = useRouter();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const onPlayNowClick = async () => {
    try {
      setIsLoading(true);
      const response = await post("/internal/games");
      const data = await response.json();
      router.push(`/games/${data.uuid}`);
      // We don't reset isLoading here.
      // It will give a user feedback waiting for the page change
    } catch {
      setError("Failed to create game.");
      setIsLoading(false);
    }
  };

  const buttonContent = isLoading ? "Creating game..." : children;

  return (
    <VStack gap={2} w={{ base: "full", sm: "auto" }}>
      {!error && (
        <Button onClick={onPlayNowClick} loading={isLoading} {...props}>
          {buttonContent}
        </Button>
      )}
      {error && (
        <>
          <Text fontSize="sm">
            {error} <Link onClick={onPlayNowClick}>Try again</Link>
          </Text>
        </>
      )}
    </VStack>
  );
}
