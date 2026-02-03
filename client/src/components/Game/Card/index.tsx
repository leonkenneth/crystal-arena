import { CardState } from "@/types";
import { CardBody } from "@chakra-ui/react";
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
import CardImage from "./CardImage";
import CardPower from "./CardPower";

type Props = {
  card: CardState;
  text?: string;
  size?: CardSize;
  dataCardId?: string;
  containerProps?: Partial<CardContainerProps>;
  displayTextOverlay?: boolean;
  displayHoverCard?: boolean;
  isInteractable?: boolean;
  overrideIsTapped?: boolean;
};

// eslint-disable-next-line react-compiler/react-compiler
export default function Card({
  card,
  text,
  size,
  dataCardId,
  containerProps,
  displayTextOverlay = true,
  displayHoverCard = card.isVisibleInUi,
  isInteractable = card.isPlayable,
  overrideIsTapped = card.isTapped,
}: Props) {
  const doAction = useDoAction(card.oid);
  const { gameState } = useLoadedGameContext();
  const { setHoveredCard } = useContext(ClientContext);
  const { ref } = useHover({
    onHoverIn: () => {
      setHoveredCard(card);
    },
    onHoverOut: () => {
      setHoveredCard(null);
    },
    enabled: displayHoverCard,
  });
  const displayedText = text;

  const onClick = () => {
    return doAction("Select");
  };

  const cardIsTargeted = isTargeted(gameState, card.cardId);
  const isSelected = card.isSelected || cardIsTargeted;
  // @ts-expect-error - card.isSelectedForCombat is not defined in the CardState type
  const isSelectedForCombat = card.isSelectedForCombat;

  return (
    <CardContainer
      isTapped={typeof overrideIsTapped === "boolean" ? overrideIsTapped : card.isTapped}
      isPlayable={card.isPlayable}
      isSelected={isSelected}
      isSelectedForCombat={isSelectedForCombat}
      isInteractable={isInteractable}
      dataCardId={dataCardId || `card-${card.cardId}`}
      onClick={isInteractable ? onClick : undefined}
      size={size}
      ref={ref}
      {...containerProps}
    >
      {card.isVisibleInUi ? (
        <>
          <CardImage card={card} />
          {card.toughness > 0 && card.type === "Permanent" && <CardPower card={card} />}
          {displayTextOverlay && displayedText && (
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
