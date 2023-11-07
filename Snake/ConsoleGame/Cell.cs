
namespace ConsoleGame
{
    public interface ICell {
        public int X { get; set; }
        public int Y { get; set; }
        public int State { get; set; }
    }

    public class Cell : ICell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int State { get; set; }

        public override string ToString()
        {
            return $"({X},{Y},{State})";
        }
    }

    public class CellUpdateCommand
    {
        public int NewState { get; set; }
        public ICell CellToUpdate { get; set; }
    }
}
