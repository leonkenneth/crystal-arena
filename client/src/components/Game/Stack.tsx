import { useLoadedGameContext } from "@/utils/GameContext";
import { Drawer } from "@chakra-ui/react";
import PassPriorityButton from "./PassPriorityButton";
import { EffectState } from "@/types";
import Card from "./Card";

type StackEffectProps = {
    effect: EffectState;
}

function StackEffect({ effect }: StackEffectProps) {
    const card = effect.card;
    const text = effect.text;
    return <>
        <Card card={card} text={text} />
    </>;
}

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