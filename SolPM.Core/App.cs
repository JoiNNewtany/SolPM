using MvvmCross.ViewModels;
using MvvmCross.IoC;
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

            RegisterAppStart<HomeViewModel>();
        }
    }
}
