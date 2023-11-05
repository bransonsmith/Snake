
namespace Snake
{

    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    public interface ISnake
    {
        public List<Cell> Cells { get; set; }
        public void Enqueue(Cell cell);
        public Cell Dequeue();
        public Cell GetHead();
        public Direction Direction { get; set; }
        public string ToString();
    }

    public class Snake : ISnake
    {
        public List<Cell> Cells { get; set; }
        public Direction Direction { get; set; }


        public Snake(Cell HeadCell)
        {
            Cells = new List<Cell>();
            Enqueue(HeadCell);
            Enqueue(new Cell { X = HeadCell.X, Y = HeadCell.Y - 1});
            Enqueue(new Cell { X = HeadCell.X, Y = HeadCell.Y - 2});
            Direction = Direction.Down;
        }

        public void Enqueue(Cell cell)
        {
            Cells.Insert(0, cell);
        }

        public Cell Dequeue() { 
            var lastCell = Cells[Cells.Count - 1];
            Cells.Remove(lastCell);
            return lastCell;
        }

        public Cell GetHead() {
            return Cells[0];
        }

        public string ToString() {
            return Direction.ToString() + " | " + String.Join(' ', Cells.Select(c => c.ToString()));
        }

    }
}
