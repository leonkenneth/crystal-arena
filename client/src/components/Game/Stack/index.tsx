import { useLoadedGameContext } from "@/utils/GameContext";
import { Drawer } from "@chakra-ui/react";
import PassPriorityButton from "../PassPriorityButton";
import StackEffect from "./StackEffect";



export default function Stack() {
    const { gameState } = useLoadedGameContext();
    const stack = gameState.screen.stack;

    if (stack.effects.length === 0) {
        return null;
    }

    return  <Drawer.Root open={true} placement="start">
    <Drawer.Backdrop />
    <Drawer.Positioner>
        <Drawer.Content>
            <Drawer.Header>
                <Drawer.Title>Stack</Drawer.Title>
            </Drawer.Header>
            <Drawer.Body>
                {stack.effects.map((effect) => (
                    <StackEffect key={`effect-${effect.card.cardId}-${effect.text}`} effect={effect} />
                ))}
            </Drawer.Body>
            <Drawer.Footer>
                <PassPriorityButton />
            </Drawer.Footer>
        </Drawer.Content>
    </Drawer.Positioner>
</Drawer.Root>
}