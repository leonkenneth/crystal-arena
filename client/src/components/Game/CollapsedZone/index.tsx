import { ZoneState } from "@/types";
import StackOfCards from "./StackOfCards";
import ExpandedZoneDialog from "./ExpandedZoneDialog";
import { useState } from "react";
import { Text } from "@chakra-ui/react";

type Props = {
  zone: ZoneState;
  name: string;
  linkOnly?: boolean;
  children?: React.ReactNode;
};

export default function CollapsedZone({ zone, linkOnly, children, name }: Props) {
  const [isOpened, setIsOpened] = useState(false);
  const cards = zone.cards;

  const onClick = () => setIsOpened(true);
  const onClose = () => setIsOpened(false);

  if (linkOnly && !children) {
    throw new Error("CollapsedZone must have children if linkOnly is true");
  }

  return (
    <div>
      {linkOnly && children ? (
        <Text
          as="div"
          textDecoration="underline"
          color="fg.muted"
          onClick={onClick}
          cursor="pointer"
        >
          {children}
        </Text>
      ) : (
        <StackOfCards cards={cards} onClick={onClick} />
      )}
      {isOpened && <ExpandedZoneDialog cards={cards} title={name} onClose={onClose} />}
    </div>
  );
}
