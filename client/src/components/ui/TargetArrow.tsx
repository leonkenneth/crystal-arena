import { useState, useEffect, useCallback } from "react";
import { createPortal } from "react-dom";
import { getCardZone } from "@/utils/gameStateQueries";
import { useLoadedGameContext } from "@/utils/LoadedGameContext";
type Props = {
  fromCardId: number;
  toCardId: number;
};

export const ArrowPortal = ({
  x1,
  y1,
  x2,
  y2,
  color = "red",
  width = 2,
  curveHeight = 50,
}: {
  x1: number;
  y1: number;
  x2: number;
  y2: number;
  color: string;
  width: number;
  curveHeight: number;
}) => {
  const [portalRoot, setPortalRoot] = useState<HTMLDivElement | null>(null);

  //return JSON.stringify({ x1, x2, y1, y2 });

  useEffect(() => {
    const div = document.createElement("div");
    div.style.position = "fixed";
    div.style.top = "0";
    div.style.left = "0";
    div.style.width = "100%";
    div.style.height = "100%";
    div.style.pointerEvents = "none";
    div.style.zIndex = "1000";
    document.body.appendChild(div);
    setPortalRoot(div);

    return () => {
      document.body.removeChild(div);
    };
  }, []);

  // Calculate the control point for the quadratic curve
  const midX = (x1 + x2) / 2;
  const midY = (y1 + y2) / 2;
  const controlX = midX;
  const controlY = midY - curveHeight; // Adjust curveHeight for more/less curve

  const arrowPath = `
    M${x1},${y1}
    Q${controlX},${controlY} ${x2},${y2}
  `;

  // Calculate arrowhead angle
  const angle = Math.atan2(y2 - y1, x2 - x1);
  const arrowHeadX1 = x2 - 10 * Math.cos(angle) - 5 * Math.sin(angle);
  const arrowHeadY1 = y2 - 10 * Math.sin(angle) + 5 * Math.cos(angle);
  const arrowHeadX2 = x2 - 10 * Math.cos(angle) + 5 * Math.sin(angle);
  const arrowHeadY2 = y2 - 10 * Math.sin(angle) - 5 * Math.cos(angle);

  const arrowHead = `
    M${x2},${y2}
    L${arrowHeadX1},${arrowHeadY1}
    L${arrowHeadX2},${arrowHeadY2}
    Z
  `;

  return portalRoot
    ? createPortal(
        <svg
          style={{
            position: "fixed",
            top: 0,
            left: 0,
            width: "100%",
            height: "100%",
            pointerEvents: "none",
          }}
        >
          <path d={arrowPath} stroke={color} strokeWidth={width} fill="none" />
          <path d={arrowHead} fill={color} />
        </svg>,
        portalRoot
      )
    : null;
};

export default function TargetArrow({ fromCardId, toCardId }: Props) {
  const { gameState } = useLoadedGameContext();
  const [fromLocation, setFromLocation] = useState<DOMRectReadOnly | null>(null);
  const [toLocation, setToLocation] = useState<DOMRectReadOnly | null>(null);
  const attr = "data-cardid";
  const from = `stackeffect-${fromCardId}`;

  const targetZone = getCardZone(gameState, toCardId);

  const to = `card-${toCardId}`;

  const updateArrow = useCallback(
    function () {
      const fromEl = document.querySelector(`[${attr}="${from}"]`);
      const toEl = document.querySelector(`[${attr}="${to}"]`);
      const fromBbox = fromEl?.getBoundingClientRect();
      const toBbox = toEl?.getBoundingClientRect();

      if (fromBbox) {
        setFromLocation(fromBbox);
      }

      if (toBbox) {
        setToLocation(toBbox);
      }

      requestAnimationFrame(updateArrow);
    },
    [attr, from, to]
  );

  useEffect(() => {
    requestAnimationFrame(updateArrow);
  }, [attr, from, to, updateArrow]);

  if (!fromLocation || !toLocation) return null;
  if (targetZone === null || targetZone === "BreakZone" || targetZone === "MainDeck") {
    return null;
  }


  const top = fromLocation.y + fromLocation.height / 2;
  const bottom = toLocation.y + toLocation.height / 2;
  const left = fromLocation.x + fromLocation.width + 10;
  const right = toLocation.x - 10;

  return (
    <ArrowPortal
      x1={left}
      y1={top}
      x2={right}
      y2={bottom}
      color="var(--chakra-colors-blue-500)"
      width={2}
      curveHeight={50}
    />
  );
}
