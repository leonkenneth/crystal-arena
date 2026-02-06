export type DeckEntry = { code: string; count: number };

export function groupCards(codes: string[]): DeckEntry[] {
  const counts = new Map<string, number>();
  for (const code of codes) {
    counts.set(code, (counts.get(code) || 0) + 1);
  }
  return Array.from(counts, ([code, count]) => ({ code, count }));
}
