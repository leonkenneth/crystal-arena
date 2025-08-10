import { CardState } from "@/types";
import { CardBody, Card as ChakraCard, Text } from "@chakra-ui/react";
import { Image } from "@chakra-ui/react";
import useDoAction from "@/utils/useDoAction";
import CardBack, { CardBackImage } from "./CardBack";
import CardContainer, { type Props as CardContainerProps } from "./CardContainer";
import CardText from "./CardText";
import { CardSize } from "./size";
import useHover from "@/utils/useHover";
import { ClientContext } from "@/utils/ClientContext";
import { useContext } from "react";

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



type Props = {
    card: CardState;
    text?: string;
    size?: CardSize;
    containerProps?: Partial<CardContainerProps>
}

export default function Card({ card, text, size, containerProps }: Props) {
    const doAction = useDoAction(card.oid);
    const { setHoveredCard } = useContext(ClientContext);
    const { isHovered, ref } = useHover({
        onHoverIn: () => {
            setHoveredCard(card);
        },
        onHoverOut: () => {
            setHoveredCard(null);
        }
    });
    const displayedText = text || card.text;
    
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
            isInteractable={card.isPlayable}
            onClick={onClick}
            size={size}
            ref={ref}
            {...containerProps}>
        {card.isVisibleInUi ? <><Image src={imageUrl} alt={card.serial} />
        <CardBody style={{
            position: "absolute",
            bottom: "12%",
            height: "35%",
            overflow: "scroll",
            padding: "0.2rem",
            fontSize: "0.5rem",
            lineHeight: "0.6rem",
            color: "white",
        }} bg="blackAlpha.800">
            <CardText text={displayedText} />    
        </CardBody></> : <CardBackImage />}
    </CardContainer>
}

export { CardBack, CardContainer };