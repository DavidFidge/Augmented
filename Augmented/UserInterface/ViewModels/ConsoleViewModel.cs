using System;
using System.Threading;
using System.Threading.Tasks;

using Augmented.Messages;
using Augmented.UserInterface.Data;

using DavidFidge.MonoGame.Core.ConsoleCommands;
using DavidFidge.MonoGame.Core.Interfaces.ConsoleCommands;
using DavidFidge.MonoGame.Core.UserInterface;

using MediatR;

namespace Augmented.UserInterface.ViewModels
{
    public class ConsoleViewModel : BaseViewModel<ConsoleData>,
        IRequestHandler<ExecuteConsoleCommandRequest>
    {
        private readonly IConsoleCommandServiceFactory _consoleCommandServiceFactory;

        public ConsoleViewModel(IConsoleCommandServiceFactory consoleCommandServiceFactory)
        {
            _consoleCommandServiceFactory = consoleCommandServiceFactory;
        }

        public Task<Unit> Handle(ExecuteConsoleCommandRequest request, CancellationToken cancellationToken)
        {
            if (!String.IsNullOrEmpty(request.Command))
            {
                var consoleCommand = new ConsoleCommand(request.Command);

                var command = _consoleCommandServiceFactory
                    .CommandFor(consoleCommand);

                if (command != null)
                    command.Execute(consoleCommand);
                else
                    consoleCommand.Result = "Command not found";

                Data.LastCommands.AddFirst(consoleCommand);

                if (Data.LastCommands.Count > 10)
                    Data.LastCommands.RemoveLast();

                Notify();
            }

            return Unit.Task;
        }
    }
}