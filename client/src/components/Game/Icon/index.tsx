import IconContainer from "./IconContainer";
import ImageIcon, { ImageIconType } from "./ImageIcon";

type Props = {
  icon: ImageIconType | "X";
  x?: number;
};

export default function Icon({ icon, x }: Props) {
  if (icon === "X") {
    if (typeof x !== "number") {
      throw new Error("X icon must have a number value");
    }
    return <IconContainer shape="circle">{x}</IconContainer>;
  }

  if (icon === "crystal") {
    return (
      <IconContainer>
        <ImageIcon icon="crystal" />
      </IconContainer>
    );
  }

  if (icon === "dull") {
    return (
      <IconContainer shape="square">
        <ImageIcon icon="dull" />
      </IconContainer>
    );
  }

  return (
    <IconContainer shape="circle">
      <ImageIcon icon={icon} />
    </IconContainer>
  );
}
