import { EffectState } from "@/types";
import Card from "../Card";

type StackEffectProps = {
    effect: EffectState;
}

export default function StackEffect({ effect }: StackEffectProps) {
    const card = effect.card;
    const text = effect.text;
    return <>
        <Card card={card} text={text} size="md" />
    </>;
}