namespace Snake
{
    public interface ISnakeGameConfig {
        public string Name { get; set; }
        public int Order { get; set; }
        public int GameAreaWidth { get; set; }
        public int GameAreaHeight { get; set; }
        public int Fps { get; set; }
    }
    
    public class SnakeGameConfig : ISnakeGameConfig
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public int GameAreaWidth { get; set; }
        public int GameAreaHeight { get; set; }
        public int Fps { get; set; }
    }
}
