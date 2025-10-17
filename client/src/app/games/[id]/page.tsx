import Game from "@/components/Game";

export default async function GamePage({ params }: { params: { id: string } }) {
  const { id } = await params;
  return <Game id={id} />;
}
