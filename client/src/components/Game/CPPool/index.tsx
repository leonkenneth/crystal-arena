import { ManaPoolState } from "@/types";
import { Box, Text } from "@chakra-ui/react";
import Icon from "../Icon";

type Props = {
    pool: ManaPoolState;
}

type ManaPoolItem = {
    color: keyof ManaPoolState;
    amount: number;
}

function CPPoolItem({ color, amount }: ManaPoolItem) {
    if (color === "colorless" || color === "multi") {
        return null;
    }

    if (color !== "crystal" && amount === 0) {
        return null;
    }

    return (
        <Box>
            <Text verticalAlign="middle" color="fg.muted"><Icon icon={color} /> {amount}</Text>
        </Box>
    );
}

export default function CPPool({ pool }: Props) {
    return (
        <Box>
            {Object.entries(pool).map(([color, amount]) => (
                <CPPoolItem color={color as keyof ManaPoolState} amount={amount} key={color} />
            ))}
        </Box>
    );
}