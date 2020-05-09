using MvvmCross.Commands;
using MvvmCross.ViewModels;
using SolPM.Core.Cryptography.PwdGenerator;
using SolPM.Core.ViewModels.Parameters;
using System.Threading.Tasks;

namespace SolPM.Core.ViewModels
{
    public class PwdGeneratorViewModel : MvxViewModel
    {
        private PwdGenerator generator;

        public PwdGeneratorViewModel()
        {
            generator = new PwdGenerator();

            GenerateCommand = new MvxCommand<PwdGenParams>((s) => Generate(s));
        }

        private string generatedPwd;

        public string GeneratedPwd
        {
            get 
            { 
                return generatedPwd; 
            }
            
            set 
            { 
                generatedPwd = value;
                RaisePropertyChanged(() => GeneratedPwd);
            }
        }

        public IMvxCommand GenerateCommand { get; private set; }

        private void Generate(PwdGenParams parameters)
        {
            if (null == parameters)
            {
                return;
            }

            // TODO: Add other params

            generator.Length = parameters.Length;
            generator.ConsecutiveCharacters = parameters.Consecutive;
            generator.Exclusions = parameters.Excluded;
            generator.ExcludeLowerCase = !parameters.LowerCase;
            generator.ExcludeUpperCase = !parameters.UpperCase;
            generator.ExcludeNumbers = !parameters.Numbers;
            generator.ExcludeSymbols = !parameters.Symbols;

            GeneratedPwd = generator.Generate();
        }
    }
}