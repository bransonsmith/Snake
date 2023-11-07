
namespace ConsoleGame
{
    public interface IConsoleGameDisplay {
        public int DisplayWidth { get; set; }
        public int DisplayHeight { get; set; }
        public int GameAreaHeight { get; set; }
        public int MessageAreaHeight { get; set; }
        public ICellRenderer CellRenderer { get; set; }

        public void RenderCells(List<ICell> cells);
        public void RenderEntireGameArea(IGameArea gameArea);
        public void DisplayMessage(string message);
        public void ClearMessage();
    }

    public class ConsoleGameDisplay : IConsoleGameDisplay
    {
        public int DisplayWidth { get; set; }
        public int DisplayHeight { get; set; }
        public int GameAreaHeight { get; set; }
        public int MessageAreaHeight { get; set; }
        public ICellRenderer CellRenderer { get; set; }
        public ConsoleMessageBox MessageBox { get; set; }

        public ConsoleGameDisplay(ICellRenderer consoleCellRenderer, int gameHeight, int gameWidth)
        {
            CellRenderer = consoleCellRenderer;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            MessageAreaHeight = 5; // TODO: magic number
            GameAreaHeight = gameHeight;
            DisplayWidth = gameWidth;
            DisplayHeight = gameHeight + 1 + MessageAreaHeight;

            MessageBox = new ConsoleMessageBox(GameAreaHeight + 1, MessageAreaHeight, DisplayWidth);
        }

        public void RenderCells(List<ICell> cells)
        {
            foreach (var cell in cells)
            {
                CellRenderer.Render(cell);
            }
        }

        public void RenderEntireGameArea(IGameArea snakeGameArea)
        {
            foreach (var cellRow in snakeGameArea.Cells)
            {
                foreach (var cell in cellRow)
                {
                    CellRenderer.Render(cell);
                }
            }
        }

        public void DisplayMessage(string message)
        {
            MessageBox.DisplayMessage(message);
        }
        public void ClearMessage()
        {
            MessageBox.ClearMessage();
        }
    }


    public class ConsoleMessageBox {
        public int StartHeightInConsole { get; set; }
        public int HeightInLines { get; set; }
        public int WidthInCharacters { get; set; }

        public ConsoleMessageBox(int startHeight, int height, int width) {
            HeightInLines = height;
            StartHeightInConsole = startHeight;
            WidthInCharacters = width;
        }

        public void DisplayMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            ClearMessage();
            Console.SetCursorPosition(2, StartHeightInConsole);
            Console.Write(message);
            Console.CursorVisible = false;
        }

        public void ClearMessage()
        {
            for (int i = 0; i < HeightInLines; i++)
            {
                Console.SetCursorPosition(0, StartHeightInConsole + i);
                Console.Write(new string(' ', WidthInCharacters));
            }

            Console.CursorVisible = false;
        }
    }
}