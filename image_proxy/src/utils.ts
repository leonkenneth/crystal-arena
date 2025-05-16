import { CanvasRenderingContext2D } from 'canvas';

type TextStyle = {
    fontFamily: string;
    fontSize: number;
    fontColor: string;
    fontWeight: string;
    fontStyle: string;
    fontVariant: string;
    fontStretch: string;
}

export function drawText(ctx: CanvasRenderingContext2D, text: string, x: number, y: number, style: Partial<TextStyle>) {
    const defaultStyle: TextStyle = {
        fontFamily: 'Arial',
        fontSize: 12,
        fontColor: '#ffffff',
        fontWeight: 'normal',
        fontStyle: 'normal',
        fontVariant: 'normal',
        fontStretch: 'normal'
    }
    const finalStyle = { ...defaultStyle, ...style };
    ctx.font = `${finalStyle.fontWeight} ${finalStyle.fontStyle} ${finalStyle.fontVariant} ${finalStyle.fontStretch} ${finalStyle.fontSize}px ${finalStyle.fontFamily}`;
    ctx.fillStyle = finalStyle.fontColor;
    ctx.fillText(text, x, y);
}

export function withShadow(ctx: CanvasRenderingContext2D, fn: () => void) {
    const shadowColor = 'rgba(0, 0, 0, 0.8)';
    const shadowOffsetX = 1;
    const shadowOffsetY = 2;
    const shadowBlur = 1;

    ctx.save();
    ctx.shadowColor = shadowColor;
    ctx.shadowOffsetX = shadowOffsetX;
    ctx.shadowOffsetY = shadowOffsetY;
    ctx.shadowBlur = shadowBlur;

    fn();
    ctx.restore();
}

export function stringToDistinctRGB(str: string, alpha: number) {
    // Hash the string to an integer using DJB2
    let hash = 5381;
    for (let i = 0; i < str.length; i++) {
      hash = ((hash << 5) + hash) + str.charCodeAt(i); // hash * 33 + c
    }
  
    // Ensure the hash is non-negative
    hash = hash >>> 0;
  
    // Use the hash to get a hue value between 0 and 360
    const hue = hash % 360;
  
    // Use fixed saturation and lightness for high contrast and vibrant colors
    const saturation = 90; // percent
    const lightness = 60;  // percent
  
    // Convert HSL to RGB
    function hslToRgb(h: number, s: number, l: number) {
      s /= 100;
      l /= 100;
  
      const c = (1 - Math.abs(2 * l - 1)) * s;
      const x = c * (1 - Math.abs((h / 60) % 2 - 1));
      const m = l - c/2;
      let r = 0, g = 0, b = 0;
  
      if (h < 60) [r, g, b] = [c, x, 0];
      else if (h < 120) [r, g, b] = [x, c, 0];
      else if (h < 180) [r, g, b] = [0, c, x];
      else if (h < 240) [r, g, b] = [0, x, c];
      else if (h < 300) [r, g, b] = [x, 0, c];
      else [r, g, b] = [c, 0, x];
  
      return {
        r: Math.round((r + m) * 255),
        g: Math.round((g + m) * 255),
        b: Math.round((b + m) * 255)
      };
    }
  
    const { r, g, b } = hslToRgb(hue, saturation, lightness);
    return `rgba(${r}, ${g}, ${b}, ${alpha})`;
  }
  

export const decimalHash = (s: string) => {
    let sum = 0;
    for (let i = 0; i < s.length; i++)
        sum += (i + 1) * (s.codePointAt(i) ?? 0) / (1 << 8)
    return sum % 1;
}

export function createRadialGradientWithColors(ctx: CanvasRenderingContext2D, htmlColors: string[]) {
    const allColorsGradient = ctx.createRadialGradient(50, 50, 0, 50, 50, 100);
    const step = 1 / (htmlColors.length - 1);
    for (let i = 0; i < htmlColors.length; i++) {
        allColorsGradient.addColorStop(i * step, htmlColors[i]);
    }
    return allColorsGradient;
}

export function drawRect(ctx: CanvasRenderingContext2D, x: number, y: number, width: number, height: number, color: string | CanvasGradient) {
    ctx.fillStyle = color;
    ctx.fillRect(x, y, width, height);
}

export function enforceArray(value: undefined | string | string[]): string[] {
    if (value === undefined) {
        return [];
    }
    return Array.isArray(value) ? value : [value];
}

export function titleCase(str: string) {
    const words = str.split(' ');
    const titleCasedWords = words.map(word => word.charAt(0).toUpperCase() + word.slice(1));
    return titleCasedWords.join(' ');
}

export function upperCase(str: string) {
    return str.toUpperCase();
}