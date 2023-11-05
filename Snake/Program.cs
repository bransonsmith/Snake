using System;
using System.Diagnostics;
using System.Threading;

using Snake;

const int GAME_AREA_HEIGHT = 20;
const int GAME_AREA_WIDTH = 60;

Console.BackgroundColor = ConsoleColor.Black;
Console.ForegroundColor = ConsoleColor.White;
Console.Clear();

var userInputHandler = new ConsoleSnakeUserInputHandler();
var cellRenderer = new ConsoleCellRenderer();
var gameRenderer = new ConsoleSnakeGameRenderer(cellRenderer);

var gameRunner = new ConsoleSnakeGameRunner(userInputHandler, gameRenderer);
gameRunner.ResetGame(GAME_AREA_HEIGHT, GAME_AREA_WIDTH);


const int FPS = 15;
TimeSpan frameTimeSpan = TimeSpan.FromSeconds(1.0 / FPS);

Stopwatch stopwatch = new Stopwatch();
TimeSpan lastFrameTime = TimeSpan.Zero;
stopwatch.Start();

var frameCounter = 0;
var loopCounter = 0;

gameRunner.ExecuteNextFrameOfGame();
while (true)
{
    TimeSpan elapsedTime = stopwatch.Elapsed;
    if (elapsedTime - lastFrameTime >= frameTimeSpan)
    {
        gameRunner.ExecuteNextFrameOfGame();
        frameCounter++;
        lastFrameTime = elapsedTime;

        // Correct for any overshoot in time
        while (elapsedTime - lastFrameTime < frameTimeSpan)
        {
            elapsedTime = stopwatch.Elapsed;
        }
    }       

    // Check for user input
    gameRunner.CheckUserInput();

    Console.SetCursorPosition(1, GAME_AREA_HEIGHT + 2);
    Console.BackgroundColor = ConsoleColor.Black;
    Console.ForegroundColor = ConsoleColor.White;
    
    Console.WriteLine($"Frame Counter: {frameCounter}");
    Console.WriteLine($"Loop Counter: {loopCounter++}");
    Console.WriteLine($"Input Direction: {gameRunner.UserInputHandler.CurrentDirection}");
    Console.WriteLine($"          Snake: {gameRunner.Snake.ToString()}");
    Console.CursorVisible = false;

    // Sleep for a short duration to reduce CPU usage
    Thread.Sleep(1);
}

