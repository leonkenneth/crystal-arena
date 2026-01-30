"use client";

import { useRouter } from "next/navigation";

export default function QuitGameButton() {
  const router = useRouter();

  return (
    <button
      onClick={() => router.push("/")}
      style={{
        position: "absolute",
        top: 8,
        left: 8,
        zIndex: 1000,
        padding: "4px 8px",
        fontSize: "11px",
        backgroundColor: "rgba(60, 60, 60, 0.8)",
        color: "#aaa",
        border: "1px solid #555",
        borderRadius: "4px",
        cursor: "pointer",
      }}
    >
      Quit
    </button>
  );
}
