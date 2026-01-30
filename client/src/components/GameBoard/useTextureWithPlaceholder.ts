import { useState, useEffect } from "react";
import { TextureLoader, Texture, CanvasTexture } from "three";

// Create a placeholder texture (solid gray)
function createPlaceholderTexture(): CanvasTexture {
  const canvas = document.createElement("canvas");
  canvas.width = 64;
  canvas.height = 64;
  const ctx = canvas.getContext("2d");
  if (ctx) {
    ctx.fillStyle = "#2a4858";
    ctx.fillRect(0, 0, 64, 64);
  }
  const texture = new CanvasTexture(canvas);
  texture.needsUpdate = true;
  return texture;
}

// Lazy initialization of placeholder (for SSR compatibility)
let placeholderTexture: CanvasTexture | null = null;
function getPlaceholderTexture(): CanvasTexture {
  if (!placeholderTexture && typeof document !== "undefined") {
    placeholderTexture = createPlaceholderTexture();
  }
  return placeholderTexture!;
}

// Global texture cache to avoid reloading the same texture
const textureCache = new Map<string, Texture>();
const textureLoader = typeof window !== "undefined" ? new TextureLoader() : null;

/**
 * Non-suspending texture hook - shows placeholder while loading.
 * Unlike drei's useTexture, this doesn't suspend React rendering.
 */
export function useTextureWithPlaceholder(url: string): Texture {
  const [texture, setTexture] = useState<Texture>(() => {
    // Check cache first
    const cached = textureCache.get(url);
    if (cached) return cached;
    return getPlaceholderTexture();
  });

  useEffect(() => {
    // Already cached
    if (textureCache.has(url)) {
      setTexture(textureCache.get(url)!);
      return;
    }

    // Load texture
    if (textureLoader) {
      textureLoader.load(url, (loadedTexture) => {
        textureCache.set(url, loadedTexture);
        setTexture(loadedTexture);
      });
    }
  }, [url]);

  return texture;
}
