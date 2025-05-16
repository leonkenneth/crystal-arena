
type Props = {
    children: React.ReactNode;
    shape?: "circle" | "square";
}

const shapeStyles = {
    circle: {
        borderRadius: "50%",
        border: "1px solid #333333",
        backgroundColor: "white",
    },
    square: {
        borderRadius: "30%",
        border: "1px solid #333333",
        backgroundColor: "white",
    },
}

export default function IconContainer({ children, shape }: Props) {
    return <span style={{
        ...(shape ? shapeStyles[shape] : {}),
        color: "black",
        width: "1.2em",
        height: "1.2em",
        lineHeight: "1.2em",
        textAlign: "center",
        display: "inline-block",
        verticalAlign: "middle",
    }}>{children}</span>;
}