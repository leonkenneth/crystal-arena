import { useLoadedGameContext } from "@/utils/GameContext";
import { Drawer } from "@chakra-ui/react";
import PassPriorityButton from "../PassPriorityButton";
import StackEffect from "./StackEffect";
import StackDrawer from "./StackDrawer";



export default function Stack() {
    const { gameState } = useLoadedGameContext();
    const stack = gameState.screen.stack;

    if (stack.effects.length === 0) {
        return null;
    }

    return <StackDrawer footer={<PassPriorityButton />}>
        {stack.effects.map((effect) => (
                <StackEffect key={`effect-${effect.card.cardId}-${effect.text}`} effect={effect} />
            ))}
    </StackDrawer>
}