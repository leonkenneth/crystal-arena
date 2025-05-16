import CardContainer from "./CardContainer";
import back from "@/assets/back.jpeg";
import { Image } from "@chakra-ui/react";

export default function CardBack() {
    return <CardContainer isPlayable={false}>
        <Image src={back.src} alt="Card Back" />
    </CardContainer>
}