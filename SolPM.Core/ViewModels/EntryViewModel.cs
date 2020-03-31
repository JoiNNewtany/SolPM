using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using SolPM.Core.Models;
using System.Threading.Tasks;

namespace SolPM.Core.ViewModels
{
    public enum FieldTypes
    {
        Username,
        Password,
        Note,
        File,
    }

    public class EntryViewModel : MvxViewModel<Entry, Entry>
    {
        private readonly IMvxNavigationService _navigationService;
        public Entry Entry { get; set; }

        public EntryViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            // CHECK IT OUT AND DO IT
            // https://docs.microsoft.com/en-us/uwp/api/windows.ui.xaml.controls.passwordbox.passwordrevealmode
            // https://stackoverflow.com/questions/5018613/wpf-switching-usercontrols-depending-on-corresponding-viewmodels-mvvm

            // Commands

            NavigateVaultView = new MvxAsyncCommand(() => _navigationService.Navigate<VaultViewModel>());
            SaveEntryCommand = new MvxAsyncCommand(SaveEntry);
        }

        public override void Prepare()
        {
            System.Diagnostics.Debug.WriteLine("EntryViewModel: Prepare() called.");
            // first callback. Initialize parameter-agnostic stuff here
        }

        public override void Prepare(Entry parameter)
        {
            System.Diagnostics.Debug.WriteLine($"EntryViewModel: Prepare with parameter called: {parameter}.");
            Entry = parameter;
        }

        public override async Task Initialize()
        {
            System.Diagnostics.Debug.WriteLine($"EntryViewModel: Initialize called.");
            //Do heavy work and data loading here
            Entry.Name = "Silly Solr Entry";
            System.Diagnostics.Debug.WriteLine($"EntryViewModel: Recieved {Entry.Name}.");
        }

        #region Commands

        public IMvxAsyncCommand NavigateVaultView { get; private set; }

        public IMvxAsyncCommand SaveEntryCommand { get; private set; }

        private async Task SaveEntry()
        {
            System.Diagnostics.Debug.WriteLine($"EntryViewModel: Returning {Entry.Name}...");
            await _navigationService.Close(this, Entry);
        }

        public IMvxCommand AddFieldCommand { get; private set; }

        private void AddField()
        {
        }

        #endregion Commands
    }
}