import TextIcon from "../TextIcon";


// R: Fire, I: Ice, G: Wind, Y: Earth, P: Lightning, U: Water
// W: Light, B: Dark, Z: Crystal, T: Dull, S: Special
const replacementMap: Record<string, (key: string) => React.ReactNode> = {
    "{R}": (key) => <TextIcon key={key} icon="fire" />,
    "{I}": (key) => <TextIcon key={key} icon="ice" />,
    "{G}": (key) => <TextIcon key={key} icon="wind" />,
    "{Y}": (key) => <TextIcon key={key} icon="earth" />,
    "{P}": (key) => <TextIcon key={key} icon="lightning" />,
    "{U}": (key) => <TextIcon key={key} icon="water" />,
    "{W}": (key) => <TextIcon key={key} icon="light" />,
    "{B}": (key) => <TextIcon key={key} icon="dark" />,
    "{Z}": (key) => <TextIcon key={key} icon="crystal" />,
    "{T}": (key) => <TextIcon key={key} icon="dull" />,
    "{EX BURST}": (key) => <i key={key}><small style={{ fontWeight: "bold", color: "blue", textShadow: "0 0 2px white", textTransform: "capitalize" }}>EX BURST</small></i>,
    "\n": (key) => <br key={key} />,
}

for (let i = 0; i < 10; i++) {
    replacementMap[i.toString()] = (key) => <span key={key} style={{ color: "black" }}>{i}</span>;
}

type Props = {
    text: string;
}

export default function CardText({ text } : Props) {
    const parts = text.split(/({[^}]+})|(\n)/);
    return parts.map((part, index) => {
        const replacement = replacementMap[part];
        return replacement ? replacement(index.toString()) : part;
    });
}
