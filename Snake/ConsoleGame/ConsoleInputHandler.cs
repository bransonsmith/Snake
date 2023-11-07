
namespace ConsoleGame
{
    /// <summary>
    /// Didn't end up using this, but I should probably have Snake Console Controller on top of this generic console controller
    /// </summary>

    public class InputBufferEntry
    {
        public ConsoleKey ConsoleKey { get; set; }
        public int Ttl { get; set; }
    }

    public interface IConsoleInputHandler
    {
        public Queue<InputBufferEntry> InputBuffer { get; set; }
        public void CheckUserInput();
    }

    public class ConsoleInputHandler : IConsoleInputHandler
    {
        public Queue<InputBufferEntry> InputBuffer { get; set; }

        public ConsoleInputHandler()
        {
            InputBuffer = new Queue<InputBufferEntry>();
        }

        public void CheckUserInput()
        {
            if (Console.KeyAvailable)
            {
                InputBuffer.Enqueue(new InputBufferEntry { 
                    ConsoleKey = Console.ReadKey().Key,
                    Ttl = 10
                });;
            }
            foreach (var inputEntry in InputBuffer)
            {
                inputEntry.Ttl--;
            }
            InputBuffer = new Queue<InputBufferEntry>(InputBuffer.Where(entry => entry.Ttl > 0));
        }

    }
}