using System.Threading;
using System.Threading.Tasks;

using Augmented.Messages;
using Augmented.UserInterface.Data;

using DavidFidge.MonoGame.Core.UserInterface;

using MediatR;

namespace Augmented.UserInterface.ViewModels
{
    public class ConsoleViewModel : BaseViewModel<ConsoleData>,
        IRequestHandler<ExecuteConsoleCommandRequest>
    {
        public Task<Unit> Handle(ExecuteConsoleCommandRequest request, CancellationToken cancellationToken)
        {
            Data.LastCommands.AddFirst(new ConsoleCommand
            {
                Text = request.Command
            });

            if (Data.LastCommands.Count > 10)
                Data.LastCommands.RemoveLast();

            Notify();

            return Unit.Task;
        }
    }
}