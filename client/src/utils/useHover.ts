import { useCallback, useState } from "react";

function divid() {
    return performance.now().toString();
}

type UseHoverProps = {
    onHoverIn: () => void,
    onHoverOut: () => void
}

export default function useHover({ onHoverIn,  onHoverOut} : UseHoverProps) {
    const hoverTriggerTimeout = 500;
    const [isHovered, setIsHovered] = useState(false);
    const ref = useCallback((node: HTMLDivElement | null) => {
        let mouseEnterListener: () => void;
        let mouseLeaveListener: () => void;
        let hoverTimeout: NodeJS.Timeout | null = null;
        if (node) {
            if (!node.getAttribute("data-hover-id")) {
                node.setAttribute("data-hover-id", divid());
            }
            const hoverId = node.getAttribute("data-hover-id");
            mouseEnterListener = () => {
                if (hoverTimeout) {
                    clearTimeout(hoverTimeout);
                }
                console.log("hover in", hoverId);
                hoverTimeout = setTimeout(() => {
                    console.log("hover in timeout", hoverId);
                    onHoverIn();
                    setIsHovered(true);
                }, hoverTriggerTimeout);
            };
            mouseLeaveListener = () => {
                console.log("hover out", hoverId);
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
        }
    }, []);
    return { isHovered, ref };
}