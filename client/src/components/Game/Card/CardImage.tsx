import { CardState } from "@/types";
import { Image } from "@chakra-ui/react";

const imageProxyBaseUrl = process.env.NEXT_PUBLIC_IMAGE_PROXY_BASE_URL || "http://localhost:4000";

function addArrayToQueryParams(searchParams: URLSearchParams, key: string, array: string[]) {
  array.forEach((item) => {
    searchParams.append(key, item);
  });
}

export function getImageUrl(card: CardState) {
  const { name, serial, power, manaCost } = card;
  const queryParams: Record<string, string> = {
    name,
    power: power ? power.toString() : "",
    manaCost,
    cardType: card.cardTypes[0],
  };
  const query = new URLSearchParams(queryParams);
  addArrayToQueryParams(query, "jobs", card.jobs);
  addArrayToQueryParams(query, "categories", card.categories);
  return new URL(`/images/cards/full/${serial}_eg.jpg?${query}`, imageProxyBaseUrl).toString();
}

export default function CardImage({ card }: { card: CardState }) {
  const imageUrl = getImageUrl(card);
  return <Image src={imageUrl} alt={card.serial} draggable={false} />;
}