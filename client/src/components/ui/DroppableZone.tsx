import { useDroppable } from "@dnd-kit/core";

type Props = {
  id: string;
  children: React.ReactNode;
  style?: React.CSSProperties;
};

export function DroppableZone({ id, children, style }: Props) {
  const { isOver, setNodeRef } = useDroppable({
    id: id,
  });
  const combinedStyle = {
    ...style,
    border: isOver ? "2px dotted var(--chakra-colors-green-500)" : "2px solid transparent",
    borderRadius: "var(--chakra-radii-md)",
    boxShadow: isOver ? "inset 0 0 10px 0 var(--chakra-colors-green-500)" : "none",
  };

  return (
    <div ref={setNodeRef} style={combinedStyle}>
      {children}
    </div>
  );
}
