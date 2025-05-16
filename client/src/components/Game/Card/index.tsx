import { CardState } from "@/types";
import { Card as ChakraCard } from "@chakra-ui/react";
import back from "@/assets/back.jpeg";
import { Image } from "@chakra-ui/react";
import { useState } from "react";
import useDoAction from "@/utils/useDoAction";
const imageProxyBaseUrl = process.env.IMAGE_PROXY_BASE_URL || 'http://localhost:4000'

function addArrayToQueryParams(searchParams: URLSearchParams, key: string, array: string[]) {
    array.forEach(item => {
        searchParams.append(key, item);
    });
}

function buildUrl(card: CardState) {
    const { name, serial, power, manaCost } = card;
    const queryParams : Record<string, string> = {
        name,
        power: power ? power.toString() : "",
        manaCost,
        cardType: card.cardTypes[0]
    }
    const query = new URLSearchParams(queryParams);
    addArrayToQueryParams(query, "jobs", card.jobs);
    addArrayToQueryParams(query, "categories", card.categories);
    return `${imageProxyBaseUrl}/images/cards/full/${serial}_eg.jpg?${query}`
}

function CardBack() {
    return <CardContainer isPlayable={false}>
        <Image src={back.src} alt="Card Back" />
    </CardContainer>
}

type CardContainerProps = {
    children: React.ReactNode,
    isPlayable?: boolean,
    isTapped?: boolean,
    isSelected?: boolean,
} & ChakraCard.RootProps;

export function CardContainer({ children, isPlayable, isTapped, isSelected, ...props }: CardContainerProps) {
    const aspectRatio = 1.4;
    const baseWidth = 80;

    const [isHovered, setIsHovered] = useState(false);
    let borderColor = "gray.400";
    if (isSelected) {
        borderColor = "blue.500";
    } else if (isHovered && isPlayable) {
        borderColor = "green.300";
    } else if (isPlayable) {
        borderColor = "green.500";
    }

    return <ChakraCard.Root
        transform={isTapped ? "rotate(15deg) scale(0.85)" : "rotate(0deg) scale(1)"}
        size="sm"
        w={`${baseWidth}px`}
        h={`${baseWidth * aspectRatio}px`} borderRadius="md"
        overflow="hidden" flexShrink={0} flexGrow={0}
        bg="transparent"
        borderWidth="2px"
        borderColor={borderColor}
        onPointerEnter={() => setIsHovered(true)}
        onPointerLeave={() => setIsHovered(false)}
        {...props}>
        {children}
    </ChakraCard.Root>
}

type Props = {
    card: CardState;
    text?: string;
}

export default function Card({ card, text }: Props) {
    const doAction = useDoAction(card.oid);
    
    if (!card.isVisibleInUi) {
        return <CardBack />
    }
    
    const imageUrl = buildUrl(card);


    const onClick = () => {
        return doAction("Select");
    }

    // @ts-ignore
    const isSelected = card.isSelected || card.isSelectedForCombat;

    return <CardContainer
            isTapped={card.isTapped}
            isPlayable={card.isPlayable}
            isSelected={isSelected}
            onClick={onClick}>
        <Image src={imageUrl} alt={card.serial} />
    </CardContainer>
}