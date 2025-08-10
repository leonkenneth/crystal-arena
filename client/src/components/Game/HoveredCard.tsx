import { ClientContext } from "@/utils/ClientContext";
import { useContext } from "react";
import Dialog from "@/components/ui/Dialog";
import Card from "./Card";
import { Button } from "@chakra-ui/react";

export default function HoveredCard() {
    const { hoveredCard } = useContext(ClientContext);
    if (!hoveredCard) return null;
    return <Dialog isModal={false}>
        <Card card={hoveredCard} />
    </Dialog>;
}