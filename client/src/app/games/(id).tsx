export default function GamePage({ params }: { params: { id: string } }) {
  return (
    <div>
      <h1>GamePage</h1>
      <p>{params.id}</p>
    </div>
  );
}
