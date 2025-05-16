import { loadImage, createCanvas, CanvasRenderingContext2D } from 'canvas';
import path from 'path';
import {
    createRadialGradientWithColors,
    decimalHash, 
    drawRect,
    enforceArray,
    stringToDistinctRGB,
    titleCase,
    upperCase,
    withShadow,
    drawText
} from './utils';

interface CpColors {
    [key: string]: string;
  }
  
  const cpColors: CpColors = {
    W: 'light',
    U: 'water',
    B: 'dark',
    R: 'fire',
    G: 'wind',
    I: 'ice',
    Y: 'earth',
    P: 'lightning'
  };
  
  const cpColorsToHTMLColor: { [key: string]: string } = {
    light: '#fbfcfc',
    water: '#2e86c1',
    dark: '#212f3d',
    fire: '#e74c3c',
    wind: '#27ae60',
    ice: '#aed6f1',
    earth: '#f4d03f',
    lightning: '#9b59b6'
  };
  
  type ManaCostResult = {
    cost: number;
    colors: string[];
  }
  
  export type CardInfo = {
    name: string;
    power?: string;
    manaCost: string;
    cardType: "forward" | "backup" | "summon" | "monster";
    jobs: string[];
    categories: string[];
  }
  
  function getCostAndColorsFromManaCost(manaCost: string): ManaCostResult {
    const numbers = manaCost.match(/\d+/g);
    const cost = numbers ? numbers.reduce((acc, curr) => acc + parseInt(curr), 0) : 0;
    const colorMatches = manaCost.match(/[WUBRGIYP]/g);
    const colors = colorMatches ? colorMatches.map(color => cpColors[color]) : [];
    return { cost, colors };
  }
  
  
  async function drawBackground(ctx: CanvasRenderingContext2D, color: string, alpha: number) {
      const blankImage = await loadImage(path.join(__dirname, 'blank.png'));
      ctx.drawImage(blankImage, 0, 0, 100, 140);
      // Whiten background slightly by drawing a transparent white rectangle
      ctx.fillStyle = color;
      ctx.fillRect(0, 0, 100, 140);
      ctx.fillStyle = `rgba(255, 255, 255, ${alpha})`;
      ctx.fillRect(0, 0, 100, 140);
  }
  
  async function drawCrystal(ctx: CanvasRenderingContext2D, colors: string[]) {
      const htmlColors = colors.map((c) => cpColorsToHTMLColor[c]);
      const allColorsGradient = createRadialGradientWithColors(ctx, htmlColors);
      ctx.fillStyle = allColorsGradient;
      const crystalWidth = 16;
      const crystalHeight = 28;
      const crystalX = 12;
      const crystalY = 5;
  
      // Draw crystal shape
      ctx.beginPath();
      ctx.moveTo(crystalX, crystalY);
      ctx.lineTo(crystalX + (crystalWidth / 2), crystalY + (crystalHeight / 4));
      ctx.lineTo(crystalX + (crystalWidth / 2), crystalY + (crystalHeight * 3 / 4));
      ctx.lineTo(crystalX, crystalY + crystalHeight);
      ctx.lineTo(crystalX - (crystalWidth / 2), crystalY + (crystalHeight * 3 / 4));
      ctx.lineTo(crystalX - (crystalWidth / 2), crystalY + (crystalHeight / 4));
      ctx.lineTo(crystalX, crystalY);
      ctx.closePath();
      ctx.fill();
  }
  
  async function drawCost(ctx: CanvasRenderingContext2D, cost: number) {
      const numberX = 6;
      const numberY = 25;
      withShadow(ctx, () => {
          drawText(ctx, cost.toString(), numberX, numberY, {
              fontSize: 18,
              fontWeight: 'bold',
              fontStyle: 'italic'
          });
      });
  }
  
  async function drawName(ctx: CanvasRenderingContext2D, name: string) {
      const nameX = 25;
      const nameY = 23;
      withShadow(ctx, () => {
          drawText(ctx, name, nameX, nameY, {
              fontSize: 15,
          });
      });
  }
  
  async function drawPower(ctx: CanvasRenderingContext2D, power: string | undefined) {
      if (!power) {
          return;
      }
      const powerX = 62;
      const powerY = 135;
      withShadow(ctx, () => {
          drawText(ctx, power, powerX, powerY, {
              fontSize: 15,
          });
      });
  }
  
  async function drawEmptyTextBlock(ctx: CanvasRenderingContext2D, colors: string[]) {
      const htmlColors = colors.map((c) => cpColorsToHTMLColor[c]);
      const allColorsGradient = createRadialGradientWithColors(ctx, htmlColors);
      const emptyTextBlockX = 0;
      const emptyTextBlockY = 70;
      const emptyTextBlockWidth = 100;
      const emptyTextBlockHeight = 50;
      //drawRect(ctx, emptyTextBlockX, emptyTextBlockY, emptyTextBlockWidth, emptyTextBlockHeight, allColorsGradient);
      drawRect(ctx, emptyTextBlockX, emptyTextBlockY, emptyTextBlockWidth, emptyTextBlockHeight, 'rgba(0, 0, 0, 0.5)');
  }
  
  async function drawSerial(ctx: CanvasRenderingContext2D, serial: string) {
      const serialX = 8;
      const serialY = 135;
      drawText(ctx, serial, serialX, serialY, {
          fontSize: 8,
          fontColor: '#222222',
      });
  }
  
  async function drawType(ctx: CanvasRenderingContext2D, type: "forward" | "backup" | "summon" | "monster") {
      const typeMap = {
          forward: "Fwd",
          backup: "Bkp",
          summon: "Sum",
          monster: "Mon"
      }
      const typeX = 0;
      const typeY = 65;
      const typeWidth = 22;
      const typeHeight = 12;
      drawRect(ctx, typeX, typeY, typeWidth, typeHeight, '#222222');
      drawText(ctx, typeMap[type], typeX + 2, typeY + typeHeight - 3, {
          fontSize: 8,
      });
  }
  
  async function drawJobs(ctx: CanvasRenderingContext2D, jobs: string) {
      const jobsX = 22;
      const jobsY = 65;
      const jobsWidth = 80;
      const jobsHeight = 12;
  
      drawRect(ctx, jobsX, jobsY, jobsWidth, jobsHeight, '#cccccc');
      drawText(ctx, jobs, jobsX + 4, jobsY + jobsHeight - 3, {
          fontSize: 8,
          fontColor: '#000000',
      });
  }
  
  async function drawCategories(ctx: CanvasRenderingContext2D, categories: string) {
      const categoriesX = 78;
      const categoriesY = 53;
      const categoriesWidth = 24;
      const categoriesHeight = 12;
  
      drawRect(ctx, categoriesX, categoriesY, categoriesWidth, categoriesHeight, '#222222');
      drawText(ctx, categories, categoriesX + 4, categoriesY + categoriesHeight - 3, {
          fontSize: 8,
      });
  }
  
  export async function generatePlaceholderImage(serial: string, cardInfo: CardInfo): Promise<Buffer> {
      const { name, power, manaCost, cardType, jobs, categories } = cardInfo;
      console.log(cardInfo);
      const { cost, colors } = getCostAndColorsFromManaCost(manaCost);
      const canvas = createCanvas(100, 140);
      const ctx = canvas.getContext('2d');
  
      const alpha = decimalHash(serial);
      const color = stringToDistinctRGB(serial, 0.4);
      await drawBackground(ctx, color, alpha);
      await drawEmptyTextBlock(ctx, colors);
      await drawCrystal(ctx, colors);
      await drawCost(ctx, cost);
      await drawName(ctx, name);
      await drawSerial(ctx, serial);
      await drawJobs(ctx, enforceArray(jobs).map(titleCase).join(' / '));
      await drawCategories(ctx, enforceArray(categories).map(upperCase).join(' · '));
      await drawType(ctx, cardType);
      await drawPower(ctx, power);
    
      return canvas.toBuffer('image/jpeg');
  }