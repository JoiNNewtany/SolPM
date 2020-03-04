using MvvmCross.IoC;
using MvvmCross.ViewModels;
using SolPM.Core.ViewModels;

namespace SolPM.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<VaultViewModel>();
        }
    }
}