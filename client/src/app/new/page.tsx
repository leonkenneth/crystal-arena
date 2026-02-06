"use client";

import { useEffect, useRef, useState } from "react";
import { useRouter } from "next/navigation";
import { Box, Heading, HStack, Spinner, Text, Textarea, VStack } from "@chakra-ui/react";
import Image from "next/image";
import Button from "@/components/ui/Button";
import { get, post } from "@/utils/api";
import { groupCards, type DeckEntry } from "@/data/decks";
import humanAvatar from "@/assets/human.png";
import computerAvatar from "@/assets/computer.png";

const imageProxyBaseUrl = process.env.NEXT_PUBLIC_IMAGE_PROXY_BASE_URL || "http://localhost:4000";

function cardImageUrl(serial: string) {
  return `${imageProxyBaseUrl}/images/cards/full/${serial}_eg.jpg`;
}

type TestDeck = { name: string; cards: string[] };

const CUSTOM_TAB = "__custom__";

function CardRow({ entry }: { entry: DeckEntry }) {
  const [hovered, setHovered] = useState(false);
  const rowRef = useRef<HTMLDivElement>(null);
  const [previewTop, setPreviewTop] = useState(0);

  const onMouseEnter = () => {
    setHovered(true);
    if (rowRef.current) {
      const rect = rowRef.current.getBoundingClientRect();
      setPreviewTop(rect.top);
    }
  };

  return (
    <HStack
      ref={rowRef}
      justify="space-between"
      py={1}
      px={1}
      borderRadius="sm"
      _hover={{ bg: "whiteAlpha.100" }}
      cursor="pointer"
      position="relative"
      onMouseEnter={onMouseEnter}
      onMouseLeave={() => setHovered(false)}
    >
      <HStack gap={2}>
        {/* eslint-disable-next-line @next/next/no-img-element */}
        <img
          src={cardImageUrl(entry.code)}
          alt={entry.code}
          width={32}
          height={44}
          style={{ borderRadius: 3, objectFit: "cover" }}
        />
        <Text color="white" fontFamily="mono" fontSize="sm">
          {entry.code}
        </Text>
      </HStack>
      <Text color="whiteAlpha.600" fontSize="sm">
        x{entry.count}
      </Text>

      {hovered && (
        <Box
          position="fixed"
          top={`${previewTop}px`}
          left="50%"
          transform="translate(-50%, -30%)"
          zIndex={50}
          pointerEvents="none"
          boxShadow="dark-lg"
          borderRadius="md"
          overflow="hidden"
        >
          {/* eslint-disable-next-line @next/next/no-img-element */}
          <img
            src={cardImageUrl(entry.code)}
            alt={entry.code}
            width={250}
            style={{ display: "block", borderRadius: 6 }}
          />
        </Box>
      )}
    </HStack>
  );
}

function DeckSection({
  label,
  avatarSrc,
  activeTab,
  onTabChange,
  customText,
  onCustomTextChange,
  entries,
  testDecks,
}: {
  label: string;
  avatarSrc: string;
  activeTab: string;
  onTabChange: (tab: string) => void;
  customText: string;
  onCustomTextChange: (text: string) => void;
  entries: DeckEntry[];
  testDecks: TestDeck[];
}) {
  return (
    <VStack gap={3} align="stretch" flex="1" minH={0}>
      <HStack gap={2}>
        <Image src={avatarSrc} alt={label} width={36} height={36} />
        <Heading size="md" color="white">
          {label}
        </Heading>
      </HStack>

      <HStack gap={2} flexWrap="wrap">
        {testDecks.map((deck) => (
          <Button
            key={deck.name}
            variant={activeTab === deck.name ? "primary" : "secondary"}
            onClick={() => onTabChange(deck.name)}
            h={9}
            px={4}
            fontSize="sm"
          >
            {deck.name}
          </Button>
        ))}
        <Button
          variant={activeTab === CUSTOM_TAB ? "primary" : "secondary"}
          onClick={() => onTabChange(CUSTOM_TAB)}
          h={9}
          px={4}
          fontSize="sm"
        >
          Customize
        </Button>
      </HStack>

      {activeTab === CUSTOM_TAB ? (
        <Textarea
          flex="1"
          minH="120px"
          value={customText}
          onChange={(e) => onCustomTextChange(e.target.value)}
          placeholder={"One card code per line, e.g.:\n1-001\n1-001\n1-003"}
          bg="whiteAlpha.100"
          border="1px solid"
          borderColor="whiteAlpha.200"
          color="white"
          fontFamily="mono"
          fontSize="sm"
          resize="none"
          _placeholder={{ color: "whiteAlpha.400" }}
        />
      ) : (
        <Box
          flex="1"
          minH={0}
          overflowY="auto"
          bg="whiteAlpha.50"
          borderRadius="md"
          border="1px solid"
          borderColor="whiteAlpha.100"
          px={4}
          py={2}
        >
          {entries.map((entry) => (
            <CardRow key={entry.code} entry={entry} />
          ))}
          <Text color="whiteAlpha.500" fontSize="xs" pt={2}>
            {entries.reduce((sum, e) => sum + e.count, 0)} cards total
          </Text>
        </Box>
      )}
    </VStack>
  );
}

function parseCodes(text: string): string[] {
  return text
    .split("\n")
    .map((l) => l.trim())
    .filter(Boolean);
}

function codesForTab(tab: string, testDecks: TestDeck[], customText: string): string[] {
  if (tab === CUSTOM_TAB) return parseCodes(customText);
  const deck = testDecks.find((d) => d.name === tab);
  return deck?.cards ?? [];
}

export default function NewGamePage() {
  const router = useRouter();

  const [testDecks, setTestDecks] = useState<TestDeck[]>([]);
  const [loading, setLoading] = useState(true);

  const [playerTab, setPlayerTab] = useState<string>("");
  const [opponentTab, setOpponentTab] = useState<string>("");
  const [playerCustom, setPlayerCustom] = useState("");
  const [opponentCustom, setOpponentCustom] = useState("");
  const [isCreating, setIsCreating] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;
    (async () => {
      try {
        const res = await get("/decks/test");
        const decks: TestDeck[] = await res.json();
        if (cancelled) return;
        setTestDecks(decks);
        if (decks.length > 0) {
          setPlayerTab(decks[0].name);
          if (decks.length > 1) {
            setOpponentTab(decks[1].name);
          } else {
            setOpponentTab(decks[0].name);
          }
        }
        setLoading(false);
      } catch {
        if (!cancelled) {
          setError("Failed to load test decks.");
          setLoading(false);
        }
      }
    })();
    return () => {
      cancelled = true;
    };
  }, []);

  const seedCustomText = (currentTab: string) => {
    const deck = testDecks.find((d) => d.name === currentTab);
    return deck ? [...deck.cards].sort().join("\n") : "";
  };

  const handlePlayerTabChange = (tab: string) => {
    if (tab === CUSTOM_TAB && playerTab !== CUSTOM_TAB) {
      setPlayerCustom(seedCustomText(playerTab));
    }
    setPlayerTab(tab);
  };

  const handleOpponentTabChange = (tab: string) => {
    if (tab === CUSTOM_TAB && opponentTab !== CUSTOM_TAB) {
      setOpponentCustom(seedCustomText(opponentTab));
    }
    setOpponentTab(tab);
  };

  const playerCodes = codesForTab(playerTab, testDecks, playerCustom);
  const opponentCodes = codesForTab(opponentTab, testDecks, opponentCustom);

  const playerEntries = groupCards(playerCodes);
  const opponentEntries = groupCards(opponentCodes);

  const startGame = async () => {
    try {
      setError(null);
      setIsCreating(true);
      const response = await post("/games", {
        playerDeck: playerCodes,
        computerDeck: opponentCodes,
      });
      const data = await response.json();
      router.push(`/games/${data.uuid}`);
      setIsCreating(false);
    } catch {
      setError("Failed to create game.");
      setIsCreating(false);
    }
  };

  if (loading) {
    return (
      <VStack minH="100dvh" bg="cyan.950" justify="center" align="center" gap={4}>
        <Spinner color="white" size="lg" />
        <Text color="whiteAlpha.700" fontSize="sm">
          Loading decks...
        </Text>
      </VStack>
    );
  }

  return (
    <VStack minH="100dvh" maxH="100dvh" bg="cyan.950" p={{ base: 4, sm: 8 }} pb={0} gap={0}>
      <Heading size="xl" color="white" textAlign="center" pt={2} flexShrink={0}>
        New Game
      </Heading>

      {/* Two-column content area */}
      <HStack
        flex="1"
        minH={0}
        w="full"
        maxW="4xl"
        mx="auto"
        gap={{ base: 4, sm: 8 }}
        align="stretch"
        flexDirection={{ base: "column", md: "row" }}
        overflow={{ base: "auto", md: "hidden" }}
        pt={4}
      >
        <DeckSection
          label="Your deck"
          avatarSrc={humanAvatar.src}
          activeTab={playerTab}
          onTabChange={handlePlayerTabChange}
          customText={playerCustom}
          onCustomTextChange={setPlayerCustom}
          entries={playerEntries}
          testDecks={testDecks}
        />

        <DeckSection
          label="Computer's deck"
          avatarSrc={computerAvatar.src}
          activeTab={opponentTab}
          onTabChange={handleOpponentTabChange}
          customText={opponentCustom}
          onCustomTextChange={setOpponentCustom}
          entries={opponentEntries}
          testDecks={testDecks}
        />
      </HStack>

      {/* Sticky bottom bar */}
      <HStack
        w="full"
        maxW="4xl"
        mx="auto"
        justify="space-between"
        py={4}
        borderTop="1px solid"
        borderColor="whiteAlpha.200"
        flexShrink={0}
      >
        <Button variant="secondary" onClick={() => router.push("/")}>
          Back
        </Button>

        <VStack gap={1} align="end">
          {error && (
            <Text color="red.300" fontSize="xs">
              {error}
            </Text>
          )}
          <Button
            variant="primary"
            onClick={startGame}
            loading={isCreating}
            disabled={playerCodes.length === 0 || opponentCodes.length === 0}
          >
            {isCreating ? "Creating game..." : "Start game"}
          </Button>
        </VStack>
      </HStack>
    </VStack>
  );
}
