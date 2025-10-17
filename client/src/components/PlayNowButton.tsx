"use client";

import { post } from "@/utils/api";
import { useRouter } from "next/navigation";
type Props = {
  className: string;
  children: React.ReactNode;
};

export default function PlayNowButton({ className, children }: Props) {
  const router = useRouter();

  const onPlayNowClick = async () => {
    const response = await post("/internal/games");
    const data = await response.json();
    router.push(`/games/${data.uuid}`);
  };

  return (
    <button className={className} onClick={onPlayNowClick}>
      {children}
    </button>
  );
}
