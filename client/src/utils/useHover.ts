import { useCallback, useState } from "react";

type UseHoverProps = {
  onHoverIn: () => void;
  onHoverOut: () => void;
  triggerTimeout?: number;
  enabled?: boolean;
};

type HoverNodeInfo = {
  onHoverIn: () => void;
  onHoverOut: () => void;
  triggerTimeout: number;
  enabled: boolean;
  node: HTMLElement | null;
  lastInteractionStartedAt: number | null;
  hovered: boolean;
};

class HoverManager {
  private nodes: HoverNodeInfo[] = [];
  private isStarted: boolean = false;
  private pointerPosition: { x: number, y: number } | null = null;

  start(node : HTMLElement | null, props : UseHoverProps) {
    this.nodes.push({
      triggerTimeout: props.triggerTimeout || defaultTriggerTimeout,
      node,
      hovered: false,
      enabled: typeof props.enabled === "undefined" ? true : props.enabled,
      lastInteractionStartedAt: null,
      ...props,
    });
    if (!this.isStarted) {
      this.isStarted = true;
      this.setupListeners();
    }
  }

  stop(node : HTMLElement | null) {
    this.nodes = this.nodes.filter(n => n.node !== node);
  }

  private setupListeners() {
    this.runNextTick();
    window.addEventListener("pointermove", this.handlePointerMove);
    window.addEventListener("mousemove", this.handlePointerMove);
  }

  private handlePointerMove = (event: PointerEvent | MouseEvent) => {
    this.pointerPosition = { x: event.clientX, y: event.clientY };
  }

  private runNextTick() {
    window.requestAnimationFrame(() => {
      for (const nodeInfo of this.nodes) {
        const { node, enabled, onHoverIn, onHoverOut } = nodeInfo;
        if (!node) continue;

        if (!enabled) {
          onHoverOut();
          nodeInfo.hovered = false;
          nodeInfo.lastInteractionStartedAt = null;
          continue;
        }

        const rect = node.getBoundingClientRect();
        const isInside = this.pointerPosition && this.pointerPosition.x >= rect.left && this.pointerPosition.x <= rect.right && this.pointerPosition.y >= rect.top && this.pointerPosition.y <= rect.bottom;
        if (isInside && !nodeInfo.lastInteractionStartedAt) {
          nodeInfo.lastInteractionStartedAt = Date.now();
        }

        if (isInside && nodeInfo.lastInteractionStartedAt && Date.now() - nodeInfo.lastInteractionStartedAt > nodeInfo.triggerTimeout && !nodeInfo.hovered) {
          onHoverIn();
          nodeInfo.hovered = true;
        }
        
        if (!isInside && nodeInfo.hovered) {
          onHoverOut();
          nodeInfo.hovered = false;
          nodeInfo.lastInteractionStartedAt = null;
        }
      }
      this.runNextTick();
    });
  }

  static singleton: HoverManager | null = null;
  static start(node: HTMLElement | null, props: UseHoverProps) {
    if (!HoverManager.singleton) {
      HoverManager.singleton = new HoverManager();
      // @ts-expect-error - window is not typed
      window.HoverManager = HoverManager.singleton;
      
    }
    HoverManager.singleton.start(node, props);
  }
  static stop(node: HTMLElement | null) {
    HoverManager.singleton?.stop(node);
  }
}

const defaultTriggerTimeout = 500;

export default function useHover(props: UseHoverProps) {
  const hoverTriggerTimeout = props.triggerTimeout || defaultTriggerTimeout;
  const onHoverIn = props.onHoverIn;
  const onHoverOut = props.onHoverOut;
  const enabled = typeof props.enabled === "undefined" ? true : props.enabled;
  const [isHovered, setIsHovered] = useState(false);
  const handleHoverIn = useCallback(() => {
    onHoverIn();
    setIsHovered(true);
  }, [onHoverIn, setIsHovered]);
  const handleHoverOut = useCallback(() => {
    onHoverOut();
    setIsHovered(false);
  }, [onHoverOut, setIsHovered]);
  const ref = useCallback(
    (node: HTMLDivElement | null) => {
        HoverManager.start(node, { onHoverIn: handleHoverIn, onHoverOut: handleHoverOut, triggerTimeout: hoverTriggerTimeout, enabled });

        return () => {
          HoverManager.stop(node);
        };
      },
    [enabled, handleHoverIn, handleHoverOut, hoverTriggerTimeout]
  );
  return { isHovered, ref };
}
