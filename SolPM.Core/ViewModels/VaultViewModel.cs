using MvvmCross.ViewModels;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace SolPM.Core.ViewModels
{
    // TODO: Remove test classes

    public class TestEntry
    {
        public TestEntry(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    public class TestFolder
    {
        public TestFolder(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    public class VaultViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public VaultViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            // Test set of entries

            TestCards.Add(new TestEntry("Google"));
            TestCards.Add(new TestEntry("Steam"));
            TestCards.Add(new TestEntry("Microsoft"));
            TestCards.Add(new TestEntry("Deviantart"));
            TestCards.Add(new TestEntry("Twitter"));
            TestCards.Add(new TestEntry("Mozilla"));
            TestCards.Add(new TestEntry("Adobe"));
            TestCards.Add(new TestEntry("Amazon"));
            TestCards.Add(new TestEntry("GOG"));
            TestCards.Add(new TestEntry("Humble Bundle"));

            // Test set of folders

            TestFolders.Add(new TestFolder("Accounts"));
            TestFolders.Add(new TestFolder("Temporary"));
            TestFolders.Add(new TestFolder("Bank"));
            TestFolders.Add(new TestFolder("Services"));
            TestFolders.Add(new TestFolder("Bin"));

            // Commands

            NavigateDatabaseView = new MvxAsyncCommand(() => _navigationService.Navigate<DatabaseViewModel>());
            NavigateEntryView = new MvxAsyncCommand(() => _navigationService.Navigate<EntryViewModel>());
        }

        #region Commands

        public IMvxAsyncCommand NavigateDatabaseView { get; private set; }
        public IMvxAsyncCommand NavigateEntryView { get; private set; }

        #endregion

        #region Properties

        public MvxObservableCollection<TestEntry> TestCards { get; set; } = new MvxObservableCollection<TestEntry>();
        public MvxObservableCollection<TestFolder> TestFolders { get; set; } = new MvxObservableCollection<TestFolder>();

        #endregion

        #region Functions

        #endregion
    }
}
