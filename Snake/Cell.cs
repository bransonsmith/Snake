namespace Snake
{
    public enum CellState
    {
        Empty,
        Snake,
        SnakeHead,
        Food,
        Obstacle
    }

    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }

        public CellState State { get; set; }

        public string ToString()
        {
            return $"({X},{Y},{State.ToString()})";
        }
    }

    public class CellUpdateCommand
    {
        public CellState NewState { get; set; }
        public Cell CellToUpdate { get; set; }
    }
}
