namespace Snake
{
    public interface ISnakeGameRunner
    {
        public ISnakeGameArea SnakeGameArea { get; set; }
        public ISnakeUserInputHandler UserInputHandler { get; set; }
        public ISnakeGameRenderer SnakeGameRenderer { get; set; }
        public ISnake Snake { get; set; }

        public void ResetGame(int gameAreaHeightInSpaces, int gameAreaWidthInSpaces);
        public List<Cell> UpdateCells(List<CellUpdateCommand> cellUpdateCommands);
        public void ExecuteNextFrameOfGame();
        public void CheckUserInput();
    }

    public interface IConsoleSnakeGameRunner : ISnakeGameRunner { }

    public class ConsoleSnakeGameRunner : IConsoleSnakeGameRunner
    {
        public ISnakeGameArea SnakeGameArea { get; set; }
        public ISnakeUserInputHandler UserInputHandler { get; set; }
        public ISnakeGameRenderer SnakeGameRenderer { get; set; }
        public ISnake Snake { get; set; }

        public ConsoleSnakeGameRunner(ISnakeUserInputHandler userInputHandler, IConsoleSnakeGameRenderer gameRenderer)
        {
            UserInputHandler = userInputHandler;
            SnakeGameRenderer = gameRenderer;
        }

        public void ResetGame(int gameAreaHeightInSpaces, int gameAreaWidthInSpaces)
        {
            SnakeGameArea = new SnakeGameArea(gameAreaHeightInSpaces, gameAreaWidthInSpaces);
            CreateNewSnakeInCenterOfTheGameArea();
            AddObstaclesToGameArea();
            AddAFoodToARandomEmptyCell();
            SnakeGameRenderer.RenderEntireSnakeGameArea(SnakeGameArea);
        }

        private void CreateNewSnakeInCenterOfTheGameArea()
        {
            Snake = new Snake(SnakeGameArea.Cells[SnakeGameArea.Cells.Length / 2][SnakeGameArea.Cells[0].Length / 2]);

            var snakeBodyCellUpdates = Snake.Cells.Select(snakeBodyCell => new CellUpdateCommand {
                CellToUpdate = snakeBodyCell,
                NewState = CellState.Snake
            }).ToList();
            snakeBodyCellUpdates.First().NewState = CellState.SnakeHead;

            UpdateCells(snakeBodyCellUpdates);
        }

        private void AddAFoodToARandomEmptyCell() {
            var cellUpdateCommands = new List<CellUpdateCommand>();
            
            var FoodCell = GetEmptyCell();
            cellUpdateCommands.Add(new CellUpdateCommand { CellToUpdate = FoodCell, NewState = CellState.Food });
            UpdateCells(cellUpdateCommands);
        }
        
        private void AddObstaclesToGameArea() {
            var cellUpdateCommands = new List<CellUpdateCommand>();

            var obstacleCells = new List<Cell>();

            obstacleCells.Add(SnakeGameArea.Cells[5][10]);
            obstacleCells.Add(SnakeGameArea.Cells[5][12]);
            obstacleCells.Add(SnakeGameArea.Cells[5][14]);

            obstacleCells.Add(SnakeGameArea.Cells[12][10]);
            obstacleCells.Add(SnakeGameArea.Cells[12][12]);
            obstacleCells.Add(SnakeGameArea.Cells[12][14]);

            foreach(var obstacleCell in obstacleCells) {
                cellUpdateCommands.Add(new CellUpdateCommand { CellToUpdate = obstacleCell, NewState = CellState.Obstacle });
            }
            UpdateCells(cellUpdateCommands);
        }

        public List<Cell> UpdateCells(List<CellUpdateCommand> cellUpdateCommands)
        {
            var updatedCells = new List<Cell>();
            foreach (var cellUpdateCommand in cellUpdateCommands)
            {
                SnakeGameArea.Cells[cellUpdateCommand.CellToUpdate.Y][cellUpdateCommand.CellToUpdate.X].State = cellUpdateCommand.NewState;
                updatedCells.Add(cellUpdateCommand.CellToUpdate);
            }
            // Console.WriteLine($"UpdateCells, {updatedCells.Count()} updatedCells");
            return updatedCells;
        }

        public void CheckUserInput() {
            UserInputHandler.CheckUserInput();

            if (UserInputHandler.CurrentDirection != Snake.Direction) {
                switch (UserInputHandler.CurrentDirection)
                {
                    case Direction.Up:
                        if (Snake.Direction != Direction.Down) {
                            Snake.Direction = Direction.Up;
                        }
                        break;
                    case Direction.Right:
                        if (Snake.Direction != Direction.Left) {
                            Snake.Direction = Direction.Right;
                        }
                        break;
                    case Direction.Down:
                        if (Snake.Direction != Direction.Up) {
                            Snake.Direction = Direction.Down;
                        }
                        break;
                    case Direction.Left:
                        if (Snake.Direction != Direction.Right) {
                            Snake.Direction = Direction.Left;
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private Cell GetEmptyCell() {
            var emptyCells = SnakeGameArea.Cells
                .SelectMany(subArray => subArray)
                .Where(cell => cell.State == CellState.Empty)
                .ToList();

            if (!emptyCells.Any())
            {
                return null; // TODO: YOU WIN
            }

            return emptyCells[new Random().Next(emptyCells.Count)];
        }

        private bool IsNextMoveInBounds(Cell headPosition, Direction direction) {
            switch (Snake.Direction)
            {
                case Direction.Up: return headPosition.Y - 1 >= 0;
                case Direction.Right: return headPosition.X + 1 < SnakeGameArea.Cells[0].Length;
                case Direction.Down: return headPosition.Y + 1 < SnakeGameArea.Cells.Length;
                case Direction.Left: return headPosition.X - 1 >= 0;
                default: break;
            }
            return true;
        }

        private List<CellUpdateCommand> MoveSnake() {

            var cellUpdateCommands = new List<CellUpdateCommand>();
            
            var oldSnakeHeadCell = Snake.GetHead();
            
            if (!IsNextMoveInBounds(oldSnakeHeadCell, Snake.Direction)) {
                Console.WriteLine($"GAME OVER! Out of Bounds. SCORE: {Snake.Cells.Count()}");
            }

            Cell newSnakeHeadCell;
            switch (Snake.Direction)
            {
                case Direction.Up:
                    newSnakeHeadCell = SnakeGameArea.Cells[Snake.GetHead().Y - 1][Snake.GetHead().X];
                    break;
                case Direction.Right:
                    newSnakeHeadCell = SnakeGameArea.Cells[Snake.GetHead().Y][Snake.GetHead().X + 1];
                    break;
                case Direction.Down:
                    newSnakeHeadCell = SnakeGameArea.Cells[Snake.GetHead().Y + 1][Snake.GetHead().X];
                    break;
                case Direction.Left:
                    newSnakeHeadCell = SnakeGameArea.Cells[Snake.GetHead().Y][Snake.GetHead().X - 1];
                    break;
                default:
                    newSnakeHeadCell = SnakeGameArea.Cells[Snake.GetHead().Y][Snake.GetHead().X - 1];
                    break;
            };

            

            if (newSnakeHeadCell.State == CellState.Obstacle) {
                Console.WriteLine($"GAME OVER! Ouch! SCORE: {Snake.Cells.Count()}");
            }


            Snake.Enqueue(newSnakeHeadCell);

            cellUpdateCommands.Add(new CellUpdateCommand { CellToUpdate = newSnakeHeadCell, NewState = CellState.SnakeHead });
            cellUpdateCommands.Add(new CellUpdateCommand { CellToUpdate = oldSnakeHeadCell, NewState = CellState.Snake }); // assumes snake len > 1
            
            if (newSnakeHeadCell.State != CellState.Food) {
                var cellThatWasRemoved = Snake.Dequeue();
                cellUpdateCommands.Add(new CellUpdateCommand { CellToUpdate = cellThatWasRemoved, NewState = CellState.Empty });
            } else {
                var FoodCell = GetEmptyCell();
                cellUpdateCommands.Add(new CellUpdateCommand { CellToUpdate = FoodCell, NewState = CellState.Food });
            }

            // Console.WriteLine($"Move Snake, {cellUpdateCommands.Count()} updates");

            return cellUpdateCommands;
        }

        public void ExecuteNextFrameOfGame()
        {
            var cellUpdateCommands = MoveSnake();
            var updatedCells = UpdateCells(cellUpdateCommands);
            SnakeGameRenderer.RenderCells(updatedCells);

            // Console.ForegroundColor = ConsoleColor.White;
            // Console.BackgroundColor = ConsoleColor.Black;
            // Console.Write(String.Join(' ', Snake.Cells.Select(c => c.ToString())));

        }
    }
}