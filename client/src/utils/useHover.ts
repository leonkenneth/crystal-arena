import { useCallback, useState } from "react";

function divid() {
  return performance.now().toString();
}

type UseHoverProps = {
  onHoverIn: () => void;
  onHoverOut: () => void;
};

export default function useHover({ onHoverIn, onHoverOut }: UseHoverProps) {
  const hoverTriggerTimeout = 500;
  const [isHovered, setIsHovered] = useState(false);
  const ref = useCallback(
    (node: HTMLDivElement | null) => {
      let mouseEnterListener: () => void;
      let mouseLeaveListener: () => void;
      let hoverTimeout: NodeJS.Timeout | null = null;
      if (node) {
        if (!node.getAttribute("data-hover-id")) {
          node.setAttribute("data-hover-id", divid());
        }
        mouseEnterListener = () => {
          if (hoverTimeout) {
            clearTimeout(hoverTimeout);
          }
          hoverTimeout = setTimeout(() => {
            onHoverIn();
            setIsHovered(true);
          }, hoverTriggerTimeout);
        };
        mouseLeaveListener = () => {
          if (hoverTimeout) {
            clearTimeout(hoverTimeout);
          }
          onHoverOut();
          setIsHovered(false);
        };
        node.addEventListener("mouseenter", mouseEnterListener, true);
        node.addEventListener("mouseleave", mouseLeaveListener, true);
      }

      return () => {
        if (hoverTimeout) {
          clearTimeout(hoverTimeout);
        }
        node?.removeEventListener("mouseenter", mouseEnterListener, true);
        node?.removeEventListener("mouseleave", mouseLeaveListener, true);
      };
    },
    [onHoverIn, onHoverOut]
  );
  return { isHovered, ref };
}
