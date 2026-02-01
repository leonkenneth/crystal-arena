"use client";

import { useState } from "react";
import { ChakraProvider } from "@chakra-ui/react";
import { system } from "@/components/ui/system";
import MenuDialog from "./MenuDialog";

export default function MenuButton() {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <>
      <button
        onClick={() => setIsOpen(true)}
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
        Menu
      </button>
      {isOpen && (
        <ChakraProvider value={system}>
          <MenuDialog onClose={() => setIsOpen(false)} />
        </ChakraProvider>
      )}
    </>
  );
}
