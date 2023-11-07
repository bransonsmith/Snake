using ConsoleGame;

namespace Snake
{
    public static class SnakeGameAreaModifier
    {
        public static List<CellUpdateCommand> PutSnakeInTheGameArea(IGameArea snakeGameArea, ISnake snake)
        {
            var snakeBodyCellUpdates = snake.Cells.Select(snakeBodyCell => new CellUpdateCommand
            {
                CellToUpdate = snakeBodyCell,
                NewState = (int)SnakeCellState.Snake
            }).ToList();
            snakeBodyCellUpdates.First().NewState = (int)SnakeCellState.SnakeHead;

            return snakeBodyCellUpdates;
        }

        public static List<CellUpdateCommand> AddAFoodToARandomEmptyCell(IGameArea snakeGameArea)
        {
            var cellUpdateCommands = new List<CellUpdateCommand>();

            var FoodCell = snakeGameArea.GetEmptyCell();
            cellUpdateCommands.Add(new CellUpdateCommand { CellToUpdate = FoodCell, NewState = (int)SnakeCellState.Food });

            return cellUpdateCommands;
        }

        public static List<CellUpdateCommand> AddObstaclesToGameArea(IGameArea snakeGameArea)
        {
            var cellUpdateCommands = new List<CellUpdateCommand>();

            var obstacleCells = new List<ICell>();

            obstacleCells.Add(snakeGameArea.Cells[snakeGameArea.Cells.Length / 4][snakeGameArea.Cells[0].Length / 3]);
            obstacleCells.Add(snakeGameArea.Cells[snakeGameArea.Cells.Length / 4 * 2][snakeGameArea.Cells[0].Length / 3]);
            obstacleCells.Add(snakeGameArea.Cells[snakeGameArea.Cells.Length / 4 * 3][snakeGameArea.Cells[0].Length / 3]);

            obstacleCells.Add(snakeGameArea.Cells[snakeGameArea.Cells.Length / 4][snakeGameArea.Cells[0].Length / 3 * 2]);
            obstacleCells.Add(snakeGameArea.Cells[snakeGameArea.Cells.Length / 4 * 2][snakeGameArea.Cells[0].Length / 3 * 2]);
            obstacleCells.Add(snakeGameArea.Cells[snakeGameArea.Cells.Length / 4 * 3][snakeGameArea.Cells[0].Length / 3 * 2]);

            foreach (var obstacleCell in obstacleCells)
            {
                cellUpdateCommands.Add(new CellUpdateCommand { CellToUpdate = obstacleCell, NewState = (int)SnakeCellState.Obstacle });
            }

            return cellUpdateCommands;
        }
    }
}
