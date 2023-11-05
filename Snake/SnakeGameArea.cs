
namespace Snake
{
    public interface ISnakeGameArea {
        public Cell[][] Cells { get; set; }
    }

    public class SnakeGameArea : ISnakeGameArea {

        public Cell[][] Cells { get; set; }

        public SnakeGameArea(int height, int width) {
            Cells = new Cell[height][]; 
            for (int y = 0; y < height; y++)
            {
                Cells[y] = new Cell[width];
                for (int x = 0; x < width; x++)
                {
                    Cells[y][x] = new Cell { X = x, Y = y, State = CellState.Empty };
                }
            }
        }
    } 
}