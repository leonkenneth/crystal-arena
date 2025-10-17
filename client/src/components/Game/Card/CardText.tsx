import Icon from "../Icon";

// R: Fire, I: Ice, G: Wind, Y: Earth, P: Lightning, U: Water
// W: Light, B: Dark, Z: Crystal, T: Dull, S: Special
const iconsMap: Record<string, (key: string) => React.ReactNode> = {
  "{R}": (key) => <Icon key={key} icon="fire" />,
  "{I}": (key) => <Icon key={key} icon="ice" />,
  "{G}": (key) => <Icon key={key} icon="wind" />,
  "{Y}": (key) => <Icon key={key} icon="earth" />,
  "{P}": (key) => <Icon key={key} icon="lightning" />,
  "{U}": (key) => <Icon key={key} icon="water" />,
  "{W}": (key) => <Icon key={key} icon="light" />,
  "{B}": (key) => <Icon key={key} icon="dark" />,
  "{Z}": (key) => <Icon key={key} icon="crystal" />,
  "{T}": (key) => <Icon key={key} icon="dull" />,
  "{EX BURST}": (key) => (
    <i key={key}>
      <small
        style={{
          fontWeight: "bold",
          color: "blue",
          textShadow: "0 0 2px white",
          textTransform: "capitalize",
        }}
      >
        EX BURST
      </small>
    </i>
  ),
};

for (let i = 0; i < 10; i++) {
  iconsMap[`{${i}}`] = (key) => <Icon key={key} icon="X" x={i} />;
}

type Props = {
  text: string;
};

function applyStringTransform(
  textParts: (string | React.ReactNode)[],
  transform: (text: string, index: number) => string | React.ReactNode
): React.ReactNode[] {
  return textParts
    .map((part, index) => {
      if (typeof part === "string") {
        return transform(part, index);
      }
      return part;
    })
    .flat();
}

function replaceIcons(textParts: (string | React.ReactNode)[]): React.ReactNode[] {
  return applyStringTransform(textParts, (text, i) => {
    const parts = text.split(/({[^}]+})/g);
    return parts.map((part, index) => {
      const replacement = iconsMap[part];
      return replacement ? replacement(`icon-${i}-${index}`) : part;
    });
  });
}

function replaceDamageX(textParts: (string | React.ReactNode)[]): React.ReactNode[] {
  return applyStringTransform(textParts, (text, i) => {
    const parts = text.split(/Damage (\d+) -- /g);
    return parts.map((part, index) => {
      if (index % 2 === 1) {
        return (
          <span style={{ fontStyle: "italic" }} key={`damage-x-${i}-${index}`}>
            Damage {part} ᠆{" "}
          </span>
        );
      }
      return part;
    });
  });
}

function replaceNewLine(textParts: (string | React.ReactNode)[]): React.ReactNode[] {
  return applyStringTransform(textParts, (text, i) => {
    const parts = text.split(/\n+/g);
    return parts.map((part, index) => {
      if (index === 0) {
        return part;
      }
      return (
        <span key={`newline-${i}-${index}`}>
          <br />
          {part}
        </span>
      );
    });
  });
}

export default function CardText({ text }: Props) {
  let processedText: (string | React.ReactNode)[] = [text.trim()];
  processedText = replaceIcons(processedText);
  processedText = replaceDamageX(processedText);
  processedText = replaceNewLine(processedText);
  return <div>{processedText}</div>;
}
