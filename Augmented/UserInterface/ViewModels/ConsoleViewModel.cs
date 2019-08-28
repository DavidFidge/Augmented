using Augmented.UserInterface.Data;

using DavidFidge.MonoGame.Core.UserInterface;

namespace Augmented.UserInterface.ViewModels
{
    public class ConsoleViewModel : BaseViewModel<ConsoleData>,
        IRequestHandler<SetDisplayModeRequest>,

    {
    }
}