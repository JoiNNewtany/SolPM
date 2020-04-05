using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using SolPM.Core.Models;
using System.Threading.Tasks;

namespace SolPM.Core.ViewModels
{
    public class EntryViewModel : MvxViewModel<Entry, Entry>
    {
        private readonly IMvxNavigationService _navigationService;
        public Entry Entry { get; set; }

        public EntryViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            // Commands

            NavigateVaultView = new MvxAsyncCommand(() => _navigationService.Navigate<VaultViewModel>());
            SaveEntryCommand = new MvxAsyncCommand(SaveEntry);
            AddFieldCommand = new MvxCommand<FieldTypes>((s) => AddField(s));
            RemoveFieldCommand = new MvxCommand<Field>((s) => RemoveField(s));
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

            // Creating a field list for an entry if it didn't already have one
            if (Entry != null && Entry.FieldList == null)
            {
                Entry.FieldList = new MvxObservableCollection<Field>();
            }
        }

        #region Commands

        public IMvxAsyncCommand NavigateVaultView { get; private set; }

        public IMvxAsyncCommand SaveEntryCommand { get; private set; }

        public IMvxCommand AddFieldCommand { get; private set; }

        public IMvxCommand RemoveFieldCommand { get; private set; }

        private async Task SaveEntry()
        {
            System.Diagnostics.Debug.WriteLine($"EntryViewModel: Returning {Entry.Name}...");
            await _navigationService.Close(this, Entry);
        }

        private void AddField(FieldTypes type)
        {
            System.Diagnostics.Debug.WriteLine(type.ToString());
            Entry.FieldList.Add(new Field() { Type = type });
        }

        private void RemoveField(Field field)
        {
            if (null != field)
            {
                System.Diagnostics.Debug.WriteLine(field.Name);
                Entry.FieldList.Remove(field);
            }
        }

        #endregion Commands
    }
}