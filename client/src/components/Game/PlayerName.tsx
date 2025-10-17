import { PlayerState } from "@/types";
import { Text } from "@chakra-ui/react";

export default function PlayerName({ player }: { player: PlayerState }) {
  return <Text color={player.isActive ? "green.500" : "fg"}>{player.playerName}</Text>;
}
