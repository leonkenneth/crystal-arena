"use client";

import PlayNowButton from "@/components/PlayNowButton";
import { VStack, Heading, Text, HStack, Link } from "@chakra-ui/react";

export default function Home() {
  return (
    <VStack
      minH="100vh"
      bg="cyan.950"
      p={{ base: 8, sm: 20 }}
      justify="space-between"
      align="center"
    >
      {/* Spacer */}
      <VStack />

      {/* Main Content - Centered */}
      <VStack gap={8} align="center" maxW="2xl">
        <Heading size="4xl" fontWeight="bold" color="white" textAlign="center">
          💎 Crystal Arena
        </Heading>
        <Text fontSize="sm" textAlign="center" fontFamily="mono" color="gray.300">
          An FFTCG simulation engine for fun and learning.
        </Text>

        <HStack gap={4} align="center" flexDirection={{ base: "column", sm: "row" }} w={{ base: "full", sm: "auto" }}>
          <PlayNowButton
            bg="white"
            color="black"
            borderRadius="full"
            border="none"
            transition="all 0.2s"
            display="flex"
            alignItems="center"
            justifyContent="center"
            gap={2}
            _hover={{ bg: "gray.300" }}
            fontWeight="medium"
            fontSize={{ base: "sm", sm: "md" }}
            h={{ base: 10, sm: 12 }}
            px={{ base: 4, sm: 5 }}
            w={{ base: "full", sm: "auto" }}
            cursor="pointer"
          >
            Play now
          </PlayNowButton>
          <Link
            href="https://github.com/leonkenneth/crystal-arena"
            target="_blank"
            rel="noopener noreferrer"
            borderRadius="full"
            border="1px solid"
            borderColor="whiteAlpha.200"
            transition="all 0.2s"
            display="flex"
            alignItems="center"
            justifyContent="center"
            _hover={{ bg: "whiteAlpha.100", borderColor: "transparent" }}
            fontWeight="medium"
            fontSize={{ base: "sm", sm: "md" }}
            h={{ base: 10, sm: 12 }}
            px={{ base: 4, sm: 5 }}
            w={{ base: "full", sm: "auto", md: "158px" }}
            textDecoration="none"
          >
            See on GitHub
          </Link>
        </HStack>
      </VStack>

      {/* Footer - Bottom */}
      <VStack gap={6} align="center" justify="center">
        <Text fontSize="xs" textAlign="center" fontFamily="mono" color="whiteAlpha.700" maxW="lg">
          FINAL FANTASY, SQUARE ENIX and the SQUARE ENIX logo are trademarks or registered
          trademarks of Square Enix Holdings Co., Ltd.
        </Text>
      </VStack>
    </VStack>
  );
}
