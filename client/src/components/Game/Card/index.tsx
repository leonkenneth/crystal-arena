import { CardState } from "@/types";
import { CardBody } from "@chakra-ui/react";
import { Image } from "@chakra-ui/react";
import useDoAction from "@/utils/useDoAction";
import CardBack, { CardBackImage } from "./CardBack";
import CardContainer, { type Props as CardContainerProps } from "./CardContainer";
import { CardSize } from "./size";
import useHover from "@/utils/useHover";
import { ClientContext } from "@/utils/ClientContext";
import { useContext } from "react";
import { isTargeted } from "@/utils/gameStateQueries";
import { useLoadedGameContext } from "@/utils/LoadedGameContext";
import CardText from "./CardText";

const imageProxyBaseUrl = process.env.NEXT_PUBLIC_IMAGE_PROXY_BASE_URL || "http://localhost:4000";

function addArrayToQueryParams(searchParams: URLSearchParams, key: string, array: string[]) {
  array.forEach((item) => {
    searchParams.append(key, item);
  });
}

function buildUrl(card: CardState) {
  const { name, serial, power, manaCost } = card;
  const queryParams: Record<string, string> = {
    name,
    power: power ? power.toString() : "",
    manaCost,
    cardType: card.cardTypes[0],
  };
  const query = new URLSearchParams(queryParams);
  addArrayToQueryParams(query, "jobs", card.jobs);
  addArrayToQueryParams(query, "categories", card.categories);
  return new URL(`/images/cards/full/${serial}_eg.jpg?${query}`, imageProxyBaseUrl).toString();
}

type Props = {
  card: CardState;
  text?: string;
  size?: CardSize;
  dataCardId?: string;
  containerProps?: Partial<CardContainerProps>;
  displayTextOverlay?: boolean;
  displayHoverCard?: boolean;
};

export default function Card({
  card,
  text,
  size,
  dataCardId,
  containerProps,
  displayTextOverlay = true,
  displayHoverCard = card.isVisibleInUi,
}: Props) {
  const doAction = useDoAction(card.oid);
  const { gameState } = useLoadedGameContext();
  const { setHoveredCard } = useContext(ClientContext);
  const { ref } = useHover({
    onHoverIn: () => {
      if (displayHoverCard) {
        setHoveredCard(card);
      }
    },
    onHoverOut: () => {
      if (displayHoverCard) {
        setHoveredCard(null);
      }
    },
  });
  const displayedText = text || card.text;

  const imageUrl = buildUrl(card);

  const onClick = () => {
    return doAction("Select");
  };

  const cardIsTargeted = isTargeted(gameState, card.cardId);
  // @ts-expect-error - card.isSelected is not defined in the CardState type
  const isSelected = card.isSelected || card.isSelectedForCombat || cardIsTargeted;

  return (
    <CardContainer
      isTapped={card.isTapped}
      isPlayable={card.isPlayable}
      isSelected={isSelected}
      isInteractable={card.isPlayable}
      dataCardId={dataCardId || `card-${card.cardId}`}
      onClick={onClick}
      size={size}
      ref={ref}
      {...containerProps}
    >
      {card.isVisibleInUi ? (
        <>
          <Image src={imageUrl} alt={card.serial} draggable={false} />
          {displayTextOverlay && (
            <CardBody
              style={{
                position: "absolute",
                bottom: "13%",
                height: "22%",
                overflow: "scroll",
                padding: "0.2rem",
                fontSize: "0.5rem",
                lineHeight: "0.6rem",
                color: "white",
              }}
              bg="blackAlpha.950"
            >
              {displayedText && <CardText text={displayedText} />}
            </CardBody>
          )}
        </>
      ) : (
        <CardBackImage />
      )}
    </CardContainer>
  );
}

export { CardBack, CardContainer };
