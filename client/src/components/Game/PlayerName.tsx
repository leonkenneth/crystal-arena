import { PlayerState } from "@/types";
import { HStack, Text } from "@chakra-ui/react";

export default function PlayerName({ player }: { player: PlayerState }) {
    return <HStack gap={1}>
        <Text color={player.isActive ? "green.500" : "fg"}>
            {player.playerName}
        </Text>
        {player.isSearchInProgress && (
            <Text color="gray.500" fontSize="sm">
                ⟳
            </Text>
        )}
    </HStack>;
}