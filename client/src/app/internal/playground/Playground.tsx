"use client";

import { useState } from "react";
import useHover from "@/utils/useHover";

function HoverableDiv({ onHoverIn, onHoverOut, children }: { onHoverIn: () => void, onHoverOut: () => void, children: React.ReactNode }) {
    const { ref } = useHover({
        onHoverIn,
        onHoverOut,
    });
    return <div ref={ref}>
        {children}
    </div>
}

export default function Playground() {
  const [isDiv1Hovered, setIsDiv1Hovered] = useState(false);
  const [isDiv2Hovered, setIsDiv2Hovered] = useState(false);
  return <div>
    <p>
        {isDiv1Hovered ? "Div 1 is hovered" : "Div 1 is not hovered"}
    </p>
    <p>
        {isDiv2Hovered ? "Div 2 is hovered" : "Div 2 is not hovered"}
    </p>
    <HoverableDiv onHoverIn={() => setIsDiv1Hovered(true)} onHoverOut={() => setIsDiv1Hovered(false)}>
      <div>Div 1</div>
    </HoverableDiv>
    <HoverableDiv onHoverIn={() => setIsDiv2Hovered(true)} onHoverOut={() => setIsDiv2Hovered(false)}>
      <div>Div 2</div>
    </HoverableDiv>
  </div>
}