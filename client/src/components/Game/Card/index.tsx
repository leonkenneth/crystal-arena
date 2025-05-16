import { CardState } from "@/types";
import { CardBody, Card as ChakraCard, Text } from "@chakra-ui/react";
import { Image } from "@chakra-ui/react";
import useDoAction from "@/utils/useDoAction";
import CardBack from "./CardBack";
import CardContainer from "./CardContainer";
import CardText from "./CardText";

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
    size?: "small" | "large";
}

export default function Card({ card, text, size }: Props) {
    const doAction = useDoAction(card.oid);
    const displayedText = text || card.text;
    
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
            onClick={onClick}
            size={size}>
        <Image src={imageUrl} alt={card.serial} />
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
        </CardBody>
    </CardContainer>
}

export { CardBack, CardContainer };