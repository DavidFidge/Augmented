using System.Collections.Generic;
using System.Linq;

using DavidFidge.MonoGame.Core.ConsoleCommands;

namespace Augmented.UserInterface.Data
{
    public class ConsoleData
    {
        public LinkedList<ConsoleCommand> LastCommands { get; set; } = new LinkedList<ConsoleCommand>();
    }
}