using ConsoleGame;

namespace Snake
{
    public static class SnakeConsoleDisplayFactory
    {
        public static IConsoleGameDisplay CreateSnakeConsoleDisplay(SnakeGameConfig config) {
            var cellRenderer = new SnakeConsoleCellRenderer();
            return new ConsoleGameDisplay(cellRenderer, config.GameAreaHeight, config.GameAreaWidth);
        }
    }
}