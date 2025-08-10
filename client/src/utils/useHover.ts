import { useCallback, useState } from "react";

type UseHoverProps = {
    onHoverIn: () => void,
    onHoverOut: () => void
}

export default function useHover({ onHoverIn,  onHoverOut} : UseHoverProps) {
    const [isHovered, setIsHovered] = useState(false);
    const ref = useCallback((node: HTMLDivElement | null) => {
        let mouseEnterListener: () => void;
        let mouseLeaveListener: () => void;
        if (node) {
            mouseEnterListener = () => {
                onHoverIn();
                setIsHovered(true);
            };
            mouseLeaveListener = () => {
                onHoverOut();
                setIsHovered(false);
            };
            node.addEventListener("mouseenter", mouseEnterListener);
            node.addEventListener("mouseleave", mouseLeaveListener);
        }

        return () => {
            node?.removeEventListener("mouseenter", mouseEnterListener);
            node?.removeEventListener("mouseleave", mouseLeaveListener);
        }
    }, []);
    return { isHovered, ref };
}