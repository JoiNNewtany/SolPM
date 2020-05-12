using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using SolPM.Core.Helpers;
using SolPM.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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
            CancelCommand = new MvxAsyncCommand(Cancel);
            SaveEntryCommand = new MvxAsyncCommand(SaveEntry);
            SetEntryIconCommand = new MvxCommand<string>((s) => SetEntryIcon(s));
            SetEntryImageCommand = new MvxCommand<string>((s) => SetEntryImage(s));
            ClearEntryIconCommand = new MvxCommand(ClearEntryIcon);
            ClearEntryImageCommand = new MvxCommand(ClearEntryImage);
            ClearEntryColorCommand = new MvxCommand(ClearEntryColor);
            AddFieldCommand = new MvxCommand<FieldTypes>((s) => AddField(s));
            RemoveFieldCommand = new MvxCommand<Field>((s) => RemoveField(s));
            MoveFieldUpCommand = new MvxCommand<Field>((s) => MoveFieldUp(s));
            MoveFieldDownCommand = new MvxCommand<Field>((s) => MoveFieldDown(s));
        }

        public override void Prepare(Entry parameter)
        {
            Entry = parameter;
        }

        public override async Task Initialize()
        {
            // Creating a field list for an entry if it didn't already have one
            if (Entry != null && Entry.FieldList == null)
            {
                Entry.FieldList = new MvxObservableCollection<Field>();
            }
        }

        #region Commands

        public IMvxAsyncCommand NavigateVaultView { get; private set; }

        public IMvxAsyncCommand CancelCommand { get; private set; }

        public IMvxAsyncCommand SaveEntryCommand { get; private set; }

        public IMvxCommand SetEntryIconCommand { get; private set; }

        public IMvxCommand SetEntryImageCommand { get; private set; }

        public IMvxCommand ClearEntryIconCommand { get; private set; }

        public IMvxCommand ClearEntryImageCommand { get; private set; }

        public IMvxCommand ClearEntryColorCommand { get; private set; }

        public IMvxCommand AddFieldCommand { get; private set; }

        public IMvxCommand RemoveFieldCommand { get; private set; }

        public IMvxCommand MoveFieldUpCommand { get; private set; }

        public IMvxCommand MoveFieldDownCommand { get; private set; }

        #endregion Commands

        private async Task Cancel()
        {
            await _navigationService.Close(this);
        }

        private async Task SaveEntry()
        {
            await _navigationService.Close(this, Entry);
        }

        private void SetEntryIcon(string fileName)
        {
            if (Entry != null)
            {
                Entry.Icon = new BitmapImage(new Uri(fileName));
                RaisePropertyChanged(() => Entry);
            }
        }

        private void SetEntryImage(string fileName)
        {
            if (Entry != null)
            {
                Entry.Image = new BitmapImage(new Uri(fileName));
                RaisePropertyChanged(() => Entry);
            }
        }

        private void ClearEntryIcon()
        {
            Entry.Icon = null;
            RaisePropertyChanged(() => Entry);
        }

        private void ClearEntryImage()
        {
            Entry.Image = null;
            RaisePropertyChanged(() => Entry);
        }

        private void ClearEntryColor()
        {
            Entry.Color = new XmlColor(System.Windows.Media.Color.FromArgb(0, 0, 0, 0));
            RaisePropertyChanged(() => Entry);
        }

        private void AddField(FieldTypes type)
        {
            switch (type)
            {
                case FieldTypes.Username:
                    Entry.FieldList.Add(new Field() { Type = type, Name = "Username" });
                    break;
                case FieldTypes.Password:
                    Entry.FieldList.Add(new Field() { Type = type, Name = "Password" });
                    break;
                case FieldTypes.Note:
                    Entry.FieldList.Add(new Field() { Type = type, Name = "Note" });
                    break;
                case FieldTypes.File:
                    Entry.FieldList.Add(new Field() { Type = type, Name = "File" });
                    break;
                default:
                    Entry.FieldList.Add(new Field() { Type = type });
                    break;
            }
        }

        private void RemoveField(Field field)
        {
            if (null != field)
            {
                Entry.FieldList.Remove(field);
            }
        }

        private void MoveFieldUp(Field field)
        {
            var index = Entry.FieldList.IndexOf(field);

            if ((index - 1) >= 0)
            {
                var old = Entry.FieldList[index - 1];
                Entry.FieldList[index - 1] = field;
                Entry.FieldList[index] = old;

                RaisePropertyChanged(() => Entry);
            }
        }

        private void MoveFieldDown(Field field)
        {
            var index = Entry.FieldList.IndexOf(field);

            if ((index + 1) < Entry.FieldList.Count)
            {
                var old = Entry.FieldList[index + 1];
                Entry.FieldList[index + 1] = field;
                Entry.FieldList[index] = old;

                RaisePropertyChanged(() => Entry);
            }
        }
    }
}