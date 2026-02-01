"use client";

import { useState, useEffect } from "react";
import { CloseButton, Dialog, Portal, VStack } from "@chakra-ui/react";
import Button from "@/components/ui/Button";
import { useRouter } from "next/navigation";

type Props = {
  onClose: () => void;
};

export default function MenuDialog({ onClose }: Props) {
  const router = useRouter();
  const [isFullscreen, setIsFullscreen] = useState(false);

  useEffect(() => {
    setIsFullscreen(!!document.fullscreenElement);
  }, []);

  const handleQuitGame = () => {
    router.push("/");
  };

  const handleToggleFullscreen = () => {
    if (document.fullscreenElement) {
      document.exitFullscreen();
    } else {
      document.documentElement.requestFullscreen();
    }
    onClose();
  };

  return (
    <Dialog.Root size="full" motionPreset="slide-in-bottom" open>
      <Portal>
        <Dialog.Backdrop />
        <Dialog.Positioner>
          <Dialog.Content>
            <Dialog.Header>
              <Dialog.Title color="white">Menu</Dialog.Title>
            </Dialog.Header>
            <Dialog.Body display="flex" justifyContent="center" alignItems="center" h="full">
              <VStack gap={4} w="full" maxW="300px">
                <Button variant="secondary" w="full" onClick={handleToggleFullscreen}>
                  {isFullscreen ? "Quit fullscreen" : "Go fullscreen"}
                </Button>
                <Button variant="secondary" w="full" onClick={handleQuitGame}>
                  Quit game
                </Button>
              </VStack>
            </Dialog.Body>
            <Dialog.CloseTrigger asChild onClick={onClose}>
              <CloseButton size="sm" />
            </Dialog.CloseTrigger>
          </Dialog.Content>
        </Dialog.Positioner>
      </Portal>
    </Dialog.Root>
  );
}
