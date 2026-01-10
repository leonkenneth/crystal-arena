import { useCallback, useRef, useState } from "react";

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

const isTouchDevice = () => {
  return 'ontouchstart' in window || navigator.maxTouchPoints > 0;
}

if (isTouchDevice()) {
  window.oncontextmenu = function(event: MouseEvent) {
    if (event.button != 2 && !(event.clientX === 1 && event.clientY === 1)) {
        event.preventDefault();
    }
  }
}

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
    for (const nodeInfo of this.nodes) {
      if (nodeInfo.node === node) {
        this.checkHovering(nodeInfo);
        break;
      }
    }
    this.nodes = this.nodes.filter(n => n.node !== node);
  }

  private setupListeners() {
    this.runNextTick();
    if (isTouchDevice()) {
      window.addEventListener("pointermove", this.handlePointerMove);
    } else {
      window.addEventListener("mousemove", this.handlePointerMove);
    }
  }

  private handlePointerMove = (event: PointerEvent | MouseEvent) => {
    this.pointerPosition = { x: event.clientX, y: event.clientY };
  }

  private checkHovering(nodeInfo: HoverNodeInfo) {
    const { node, enabled, onHoverIn } = nodeInfo;

    if (!node) return;

    if (!enabled && nodeInfo.hovered) {
      this.stopHovering(nodeInfo);
      return;
    }

    const rect = node.getBoundingClientRect();
    const isInside = this.pointerPosition && this.pointerPosition.x >= rect.left && this.pointerPosition.x <= rect.right && this.pointerPosition.y >= rect.top && this.pointerPosition.y <= rect.bottom;
    if (isInside && !nodeInfo.lastInteractionStartedAt) {
      nodeInfo.lastInteractionStartedAt = performance.now();
    }

    if (isInside && nodeInfo.lastInteractionStartedAt && (performance.now() - nodeInfo.lastInteractionStartedAt > nodeInfo.triggerTimeout) && !nodeInfo.hovered) {
      onHoverIn();
      nodeInfo.hovered = true;
    }
    
    if (!isInside) {
      this.stopHovering(nodeInfo);
    }
  }

  private runNextTick() {
    window.requestAnimationFrame(() => {
      for (const nodeInfo of this.nodes) {
        this.checkHovering(nodeInfo);
      }
      this.runNextTick();
    });
  }

  private stopHovering(nodeInfo: HoverNodeInfo) {
    if (nodeInfo.hovered) {
      nodeInfo.onHoverOut();
    }
    nodeInfo.hovered = false;
    nodeInfo.lastInteractionStartedAt = null;
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
  const onHoverInRef = useRef(props.onHoverIn);
  const onHoverOutRef = useRef(props.onHoverOut);
  onHoverInRef.current = props.onHoverIn;
  onHoverOutRef.current = props.onHoverOut;
  const enabled = typeof props.enabled === "undefined" ? true : props.enabled;
  const [isHovered, setIsHovered] = useState(false);
  const handleHoverIn = useCallback(() => {
    onHoverInRef.current();
    setIsHovered(true);
  }, [setIsHovered]);
  const handleHoverOut = useCallback(() => {
    onHoverOutRef.current();
    setIsHovered(false);
  }, [setIsHovered]);
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
