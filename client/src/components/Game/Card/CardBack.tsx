import CardContainer from "./CardContainer";
import back from "@/assets/back.jpeg";
import { Image } from "@chakra-ui/react";

export function CardBackImage() {
  return <Image src={back.src} alt="Card Back" />;
}

export default function CardBack() {
  return (
    <CardContainer isPlayable={false}>
      <CardBackImage />
    </CardContainer>
  );
}
