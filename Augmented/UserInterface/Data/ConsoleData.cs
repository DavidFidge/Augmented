using System.Collections.Generic;

namespace Augmented.UserInterface.Data
{
    public class ConsoleData
    {
        public LinkedList<ConsoleCommand> LastCommands { get; set; } = new LinkedList<ConsoleCommand>();
    }

    public class ConsoleCommand
    {
        public string Text { get; set; }
        public string Result { get; set; }
    }
}