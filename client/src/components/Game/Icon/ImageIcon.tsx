import Image from "next/image";
import crystalIcon from "@/assets/icons/crystal.png";
import darkIcon from "@/assets/icons/dark.png";
import dullIcon from "@/assets/icons/dull.png";
import earthIcon from "@/assets/icons/earth.png";
import fireIcon from "@/assets/icons/fire.png";
import iceIcon from "@/assets/icons/ice.png";
import lightIcon from "@/assets/icons/light.png";
import lightningIcon from "@/assets/icons/lightning.png";
import windIcon from "@/assets/icons/wind.png";
import waterIcon from "@/assets/icons/water.png";
export type ImageIconType =
  | "crystal"
  | "dark"
  | "dull"
  | "earth"
  | "fire"
  | "ice"
  | "light"
  | "lightning"
  | "wind"
  | "water";

const icons = {
  crystal: crystalIcon,
  dark: darkIcon,
  dull: dullIcon,
  earth: earthIcon,
  fire: fireIcon,
  ice: iceIcon,
  light: lightIcon,
  lightning: lightningIcon,
  wind: windIcon,
  water: waterIcon,
};
type Props = {
  icon: ImageIconType;
};
export default function ImageIcon({ icon }: Props) {
  return (
    <Image
      src={icons[icon]}
      alt={icon}
      style={{
        objectFit: "contain",
        objectPosition: "center",
        display: "inline-block",
        verticalAlign: "middle",
      }}
    />
  );
}
