import { useCallback, useState } from "react";

type UseHoverProps = {
  onHoverIn: () => void;
  onHoverOut: () => void;
  triggerTimeout?: number;
  enabled?: boolean;
};

export default function useHover(props: UseHoverProps) {
  const hoverTriggerTimeout = props.triggerTimeout || 500;
  const onHoverIn = props.onHoverIn;
  const onHoverOut = props.onHoverOut;
  const enabled = typeof props.enabled === "undefined" ? true : props.enabled;
  const [isHovered, setIsHovered] = useState(false);
  const ref = useCallback(
    (node: HTMLDivElement | null) => {
      let mouseEnterListener: () => void;
      let mouseLeaveListener: () => void;
      let hoverTimeout: NodeJS.Timeout | null = null;
      if (node && enabled) {
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
        node.addEventListener("mouseenter", mouseEnterListener);
        node.addEventListener("mouseleave", mouseLeaveListener);
      }

      return () => {
        if (hoverTimeout) {
          clearTimeout(hoverTimeout);
        }
        node?.removeEventListener("mouseenter", mouseEnterListener);
        node?.removeEventListener("mouseleave", mouseLeaveListener);
      };
    },
    [onHoverIn, onHoverOut, enabled, hoverTriggerTimeout]
  );
  return { isHovered, ref };
}
