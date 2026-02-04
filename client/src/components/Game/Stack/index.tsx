import { useLoadedGameContext } from "@/utils/LoadedGameContext";
import { Container } from "@chakra-ui/react";
import PassPriorityButton from "../PassPriorityButton";
import StackEffect from "./StackEffect";
import StackDrawer from "./StackDrawer";
import { EffectState } from "@/types";

export default function Stack() {
  const { gameState } = useLoadedGameContext();
  const stack = gameState.screen.stack;

  if (stack.effects.length === 0) {
    return null;
  }

  return (
    <StackDrawer footer={<PassPriorityButton />}>
      <Container display="flex" alignItems="center" justifyContent="center">
        {stack.effects.map((effect: EffectState, index: number) => (
          <StackEffect
            key={`effect-${effect.card.cardId}-${effect.text}`}
            effect={effect}
            stackOid={stack.oid}
            effectIndex={index}
          />
        ))}
      </Container>
    </StackDrawer>
  );
}
