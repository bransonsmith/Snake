using ConsoleGame;

namespace Snake
{

    public interface ISnake
    {
        public List<ICell> Cells { get; set; }
        public void Enqueue(ICell cell);
        public ICell Dequeue();
        public ICell GetHead();
        public Direction Direction { get; set; }
    }

    public class Snake : ISnake
    {
        public List<ICell> Cells { get; set; }
        public Direction Direction { get; set; }


        public Snake(ICell HeadCell)
        {
            Cells = new List<ICell>();
            Enqueue(HeadCell);
            Enqueue(new Cell { X = HeadCell.X, Y = HeadCell.Y + 1 });
            Enqueue(new Cell { X = HeadCell.X, Y = HeadCell.Y + 2 });
            Direction = Direction.Down;
        }

        public void Enqueue(ICell cell)
        {
            Cells.Insert(0, cell);
        }

        public ICell Dequeue()
        {
            var lastCell = Cells[Cells.Count - 1];
            Cells.Remove(lastCell);
            return lastCell;
        }

        public ICell GetHead()
        {
            return Cells[0];
        }

        public override string ToString()
        {
            return Direction.ToString() + " | " + string.Join(' ', Cells.Select(c => c.ToString()));
        }
    }
}
