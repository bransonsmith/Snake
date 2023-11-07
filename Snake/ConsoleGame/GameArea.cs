
namespace ConsoleGame
{
    public interface IGameArea
    {
        public ICell[][] Cells { get; set; }
        public ICell GetEmptyCell();
    }
}
