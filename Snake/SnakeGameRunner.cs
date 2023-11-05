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

        private List<CellUpdateCommand> MoveSnake() {

            var cellUpdateCommands = new List<CellUpdateCommand>();
            
            var oldSnakeHeadCell = Snake.GetHead();
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

            Snake.Enqueue(newSnakeHeadCell);

            cellUpdateCommands.Add(new CellUpdateCommand { CellToUpdate = newSnakeHeadCell, NewState = CellState.SnakeHead });
            cellUpdateCommands.Add(new CellUpdateCommand { CellToUpdate = oldSnakeHeadCell, NewState = CellState.Snake }); // assumes snake len > 1
            
            if (newSnakeHeadCell.State != CellState.Food) {
                var cellThatWasRemoved = Snake.Dequeue();
                cellUpdateCommands.Add(new CellUpdateCommand { CellToUpdate = cellThatWasRemoved, NewState = CellState.Empty });
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