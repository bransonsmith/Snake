using Snake;

var snakeGameConfig = new SnakeGameConfig {
    Name = "Normal",
    Order = 10,
    GameAreaWidth = 60,
    GameAreaHeight = 20,
    Fps = 15,
};

var snakeKeyboardInputHandler = new SnakeKeyboardInputHandler();
var snakeGameDisplay = SnakeConsoleDisplayFactory.CreateSnakeConsoleDisplay(snakeGameConfig);
var snakeGameRunner = new ConsoleSnakeGameRunner(snakeKeyboardInputHandler, snakeGameDisplay, snakeGameConfig);
snakeGameRunner.Start();
