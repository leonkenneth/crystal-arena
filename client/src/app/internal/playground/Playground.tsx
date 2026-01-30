"use client";

import { useState } from "react";
import useHover from "@/utils/useHover";
import Button from "@/components/ui/Button";

function HoverableDiv({
  onHoverIn,
  onHoverOut,
  children,
}: {
  onHoverIn: () => void;
  onHoverOut: () => void;
  children: React.ReactNode;
}) {
  const { ref } = useHover({
    onHoverIn,
    onHoverOut,
  });
  return <div ref={ref}>{children}</div>;
}

// eslint-disable-next-line react-compiler/react-compiler
export default function Playground() {
  const [divsState, setDivsState] = useState<{ [key: string]: boolean }>({});
  return (
    <div>
      {Object.entries(divsState).map(([key, value]) => (
        <p key={key}>{value ? "Div is hovered" : "Div is not hovered"}</p>
      ))}
      {Object.keys(divsState).map((key) => (
        <HoverableDiv
          key={key}
          onHoverIn={() => setDivsState({ ...divsState, [key]: true })}
          onHoverOut={() => setDivsState({ ...divsState, [key]: false })}
        >
          <div>Div {key}</div>
        </HoverableDiv>
      ))}
      <Button onClick={() => setDivsState({ ...divsState, [Math.random().toString()]: false })}>
        Add Div
      </Button>
    </div>
  );
}
