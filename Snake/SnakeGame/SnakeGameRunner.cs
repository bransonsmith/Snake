using ConsoleGame;
using System.Diagnostics;

namespace Snake
{
    public enum SnakeGameState
    {
        INIT,
        PLAYING,
        GAME_OVER
    }

    public interface ISnakeGameRunner
    {
        public IGameArea SnakeGameArea { get; set; }
        public ISnakeInputHandler UserInputHandler { get; set; }
        public IConsoleGameDisplay SnakeGameDisplay { get; set; }
        public ISnake Snake { get; set; }
        public SnakeGameState SnakeGameState { get; set; }

        public void ResetGame();
        public List<ICell> UpdateCells(List<CellUpdateCommand> cellUpdateCommands);
        public void ExecuteNextFrameOfGame();
        public void CheckUserInput();
        public void Start();
    }

    public class ConsoleSnakeGameRunner : ISnakeGameRunner
    {
        public IGameArea SnakeGameArea { get; set; }
        public ISnakeInputHandler UserInputHandler { get; set; }
        public IConsoleGameDisplay SnakeGameDisplay { get; set; }
        public ISnake Snake { get; set; }
        public ISnakeGameConfig SnakeGameConfig{ get; set; }
        public SnakeGameState SnakeGameState { get; set; }

        public ConsoleSnakeGameRunner(ISnakeInputHandler userInputHandler, IConsoleGameDisplay snakeGameDisplay, ISnakeGameConfig config)
        {
            UserInputHandler = userInputHandler;
            SnakeGameDisplay = snakeGameDisplay;
            SnakeGameConfig = config;
        }

        public void Start()
        {
            ResetGame();


            SnakeGameDisplay.DisplayMessage($"Press any key to start...");
            Console.ReadKey(); // This absolutely does not belong here. but im lazy
            SnakeGameState = SnakeGameState.PLAYING;
            SnakeGameDisplay.DisplayMessage($"Score: {Snake.Cells.Count()}");


            TimeSpan frameLength = TimeSpan.FromSeconds(1.0 / SnakeGameConfig.Fps);
            Stopwatch stopwatch = new Stopwatch();
            TimeSpan lastFrameTime = TimeSpan.Zero;
            stopwatch.Start();

            // MAIN GAME LOOP
            ExecuteNextFrameOfGame();
            while (SnakeGameState == SnakeGameState.PLAYING)
            {
                TimeSpan elapsedTime = stopwatch.Elapsed;

                if (itIsTimeForTheNextFrame(elapsedTime, lastFrameTime, frameLength))
                {
                    ExecuteNextFrameOfGame();
                    lastFrameTime = elapsedTime;
                    while (elapsedTime - lastFrameTime < frameLength)
                    {
                        elapsedTime = stopwatch.Elapsed;
                    }
                }

                CheckUserInput();
                Thread.Sleep(1);
            }
        }

        public void ExecuteNextFrameOfGame()
        {
            var cellUpdateCommands = MoveSnake();
            var updatedCells = UpdateCells(cellUpdateCommands);
            SnakeGameDisplay.RenderCells(updatedCells);
        }

        private bool itIsTimeForTheNextFrame(TimeSpan elapsedTime, TimeSpan lastFrameTime, TimeSpan frameLength)
        {
            return elapsedTime - lastFrameTime >= frameLength;
        }

        public void ResetGame()
        {
            SnakeGameState = SnakeGameState.INIT;
            SnakeGameArea = new SnakeGameArea(SnakeGameConfig.GameAreaHeight, SnakeGameConfig.GameAreaWidth);

            var cellUpdatesToStartTheGame = new List<CellUpdateCommand>();

            Snake = new Snake(SnakeGameArea.Cells[SnakeGameArea.Cells.Length / 2][SnakeGameArea.Cells[0].Length / 2]);
            cellUpdatesToStartTheGame.AddRange(SnakeGameAreaModifier.PutSnakeInTheGameArea(SnakeGameArea, Snake));
            cellUpdatesToStartTheGame.AddRange(SnakeGameAreaModifier.AddObstaclesToGameArea(SnakeGameArea));
            cellUpdatesToStartTheGame.AddRange(SnakeGameAreaModifier.AddAFoodToARandomEmptyCell(SnakeGameArea));
            UpdateCells(cellUpdatesToStartTheGame);
            SnakeGameDisplay.RenderEntireGameArea(SnakeGameArea);
        }

        public List<ICell> UpdateCells(List<CellUpdateCommand> cellUpdateCommands)
        {
            var updatedCells = new List<ICell>();
            foreach (var cellUpdateCommand in cellUpdateCommands)
            {
                SnakeGameArea.Cells[cellUpdateCommand.CellToUpdate.Y][cellUpdateCommand.CellToUpdate.X].State = cellUpdateCommand.NewState;
                updatedCells.Add(cellUpdateCommand.CellToUpdate);
            }
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

        private bool IsNextMoveInBounds(ICell headPosition, Direction direction) {
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

        private ICell? GetNextCell(ICell currentCell, Direction direction)
        {
            if (!IsNextMoveInBounds(currentCell, direction))
            {
                return null;
            }
            return direction switch
            {
                Direction.Up => SnakeGameArea.Cells[currentCell.Y - 1][currentCell.X],
                Direction.Right => SnakeGameArea.Cells[currentCell.Y][currentCell.X + 1],
                Direction.Down => SnakeGameArea.Cells[currentCell.Y + 1][currentCell.X],
                Direction.Left => SnakeGameArea.Cells[currentCell.Y][currentCell.X - 1],
                _ => null // Invalid direction
            };
        }

        private void GameOver(string reason)
        {
            SnakeGameState = SnakeGameState.GAME_OVER;
            SnakeGameDisplay.DisplayMessage($"GAME OVER! {reason} SCORE: {Snake.Cells.Count()}");
        }

        private List<CellUpdateCommand> MoveSnake() {

            var cellUpdateCommands = new List<CellUpdateCommand>();
            
            var oldSnakeHeadCell = Snake.GetHead();
            var newSnakeHeadCell = GetNextCell(Snake.GetHead(), Snake.Direction);

            if (newSnakeHeadCell == null) { GameOver("Out of Bounds!"); return cellUpdateCommands; }
            
            switch (newSnakeHeadCell.State)
            {
                case (int)SnakeCellState.Obstacle: GameOver("Don't touch the spikes!"); return cellUpdateCommands;
                case (int)SnakeCellState.Snake: GameOver("You tried to eat yourself!"); return cellUpdateCommands;
                case (int)SnakeCellState.Food:
                    var FoodCell = SnakeGameArea.GetEmptyCell();
                    if (FoodCell == null)
                    {
                        GameOver("WOW wow WOW! You won! or you cheated!"); return cellUpdateCommands;
                    }
                    else
                    {
                        cellUpdateCommands.Add(new CellUpdateCommand { CellToUpdate = FoodCell, NewState = (int)SnakeCellState.Food });
                        SnakeGameDisplay.DisplayMessage($"Score: {Snake.Cells.Count()}");
                    }
                    break;
                default:
                    break;
            }

            Snake.Enqueue(newSnakeHeadCell);

            cellUpdateCommands.Add(new CellUpdateCommand { CellToUpdate = newSnakeHeadCell, NewState = (int)SnakeCellState.SnakeHead });
            cellUpdateCommands.Add(new CellUpdateCommand { CellToUpdate = oldSnakeHeadCell, NewState = (int)SnakeCellState.Snake }); // assumes snake len > 1
                
            if (newSnakeHeadCell.State != (int)SnakeCellState.Food) {
                var cellThatWasRemoved = Snake.Dequeue();
                cellUpdateCommands.Add(new CellUpdateCommand { CellToUpdate = cellThatWasRemoved, NewState = (int)SnakeCellState.Empty });
            }

            return cellUpdateCommands;
        }
    }
}