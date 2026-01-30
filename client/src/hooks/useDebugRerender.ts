import { useRef, useEffect } from "react";

export function useDebugRerender(componentName: string, props: Record<string, unknown>) {
  const renderCount = useRef(0);
  const prevProps = useRef<Record<string, unknown>>({});

  renderCount.current += 1;

  useEffect(() => {
    const changedProps: Record<string, { from: unknown; to: unknown }> = {};

    Object.keys(props).forEach((key) => {
      if (prevProps.current[key] !== props[key]) {
        changedProps[key] = {
          from: prevProps.current[key],
          to: props[key],
        };
      }
    });

    // Check for removed props
    Object.keys(prevProps.current).forEach((key) => {
      if (!(key in props)) {
        changedProps[key] = {
          from: prevProps.current[key],
          to: undefined,
        };
      }
    });

    if (Object.keys(changedProps).length > 0) {
      console.log(
        `[${componentName}] Render #${renderCount.current} - Changed props:`,
        changedProps
      );
    } else if (renderCount.current > 1) {
      console.log(
        `[${componentName}] Render #${renderCount.current} - No prop changes (parent re-rendered)`
      );
    } else {
      console.log(`[${componentName}] Initial render`);
    }

    prevProps.current = { ...props };
  });
}
