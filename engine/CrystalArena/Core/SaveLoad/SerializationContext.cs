namespace CrystalArena
{
    public class SerializationContext
    {
        public GameRecorder Recorder
        {
            get { return Game.Recorder; }
        }
        public Game Game { get; set; }
    }
}
