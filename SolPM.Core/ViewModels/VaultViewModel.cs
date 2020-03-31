using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using SolPM.Core.Helpers;
using SolPM.Core.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;

namespace SolPM.Core.ViewModels
{
    public class VaultViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _navigationService;

        public VaultViewModel(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;

            // Commands

            NavigateDatabaseView = new MvxAsyncCommand(() => _navigationService.Navigate<DatabaseViewModel>());
            NavigateEntryView = new MvxAsyncCommand(() => _navigationService.Navigate<EntryViewModel>());
            AddEntryCommand = new MvxAsyncCommand(() => AddEntry());
        }

        public override async Task Initialize()
        {
            // TODO: Get rid of test actions AND MAKE VAULT A PROPERTY!!!

            // Test Singleton Vault

            Vault vault = Vault.GetInstance();
            vault.Name = "My Test Vault";

            vault.FolderList = new MvxObservableCollection<Folder>()
            {
                new Folder()
                {
                    Name = "Folder 1",
                    Color = new XmlColor(System.Windows.Media.Color.FromRgb(0, 255, 0)),

                    EntryList = new MvxObservableCollection<Entry>()
                    {
                        new Entry()
                        {
                            Name = "Google",
                            Icon = new BitmapImage(new Uri("E:\\Downloads\\google-logo.png")),
                            Color = new XmlColor(System.Windows.Media.Color.FromRgb(255, 255, 255)),
                            Created = DateTime.Now,
                            Modified = DateTime.Now,
                            Accessed = DateTime.Now,

                            FieldList = new MvxObservableCollection<Field>()
                            {
                                new Field()
                                {
                                    Name = "Username",
                                    Value = "Solar",
                                },

                                new Field()
                                {
                                    Name = "Password",
                                    Value = new byte[] { 1, 1, 1, 0, 0 },
                                },

                                new Field()
                                {
                                    Name = "URL",
                                    Value = "www.google.com",
                                },
                            }
                        },

                        new Entry()
                        {
                            Name = "Secret",
                            Image = new BitmapImage(new Uri("E:\\Downloads\\ShareX-ScreenRecordings\\ramiras.png")),
                            Created = DateTime.Now,
                            Modified = DateTime.Now,
                            Accessed = DateTime.Now,

                            FieldList = new MvxObservableCollection<Field>()
                            {
                                new Field()
                                {
                                    Name = "Username",
                                    Value = "Solar",
                                },

                                new Field()
                                {
                                    Name = "Password",
                                    Value = new byte[] { 1, 0, 1, 1, 0 },
                                },

                                new Field()
                                {
                                    Name = "URL",
                                    Value = "www.google.com",
                                },
                            }
                        },
                    }
                },

                new Folder()
                {
                    Name = "Super Folder 2",
                    Color = new XmlColor(System.Windows.Media.Color.FromRgb(0, 0, 255)),

                    EntryList = new MvxObservableCollection<Entry>()
                    {
                        new Entry()
                        {
                            Name = "Microsoft",
                            Icon = new BitmapImage(new Uri("E:\\Downloads\\microsoft-logo.png")),
                            Color = new XmlColor(System.Windows.Media.Color.FromRgb(0, 64, 131)),
                            Created = DateTime.Now,
                            Modified = DateTime.Now,
                            Accessed = DateTime.Now,

                            FieldList = new MvxObservableCollection<Field>()
                            {
                                new Field()
                                {
                                    Name = "Username",
                                    Value = "Solar Flaer",
                                },

                                new Field()
                                {
                                    Name = "Password",
                                    Value = new byte[] { 0, 0, 1, 0, 1 },
                                },

                                new Field()
                                {
                                    Name = "URL",
                                    Value = "www.microsoft.com",
                                },
                            }
                        },

                        new Entry()
                        {
                            Name = "Ducc",
                            Image = new BitmapImage(new Uri("E:\\Downloads\\duck-image.jpg")),
                            Created = DateTime.Now,
                            Modified = DateTime.Now,
                            Accessed = DateTime.Now,

                            FieldList = new MvxObservableCollection<Field>()
                            {
                                new Field()
                                {
                                    Name = "Username",
                                    Value = "Sauce Fire",
                                },

                                new Field()
                                {
                                    Name = "Password",
                                    Value = new byte[] { 0, 0, 0, 1, 0 },
                                },

                                new Field()
                                {
                                    Name = "URL",
                                    Value = "www.duckduckgo.com",
                                },

                                new Field()
                                {
                                    Name = "Note",
                                    Value = "Ducc is cute\nDucc needs pat",
                                },
                            }
                        },

                        new Entry()
                        {
                            Name = "Duce Nuce",
                            Icon = new BitmapImage(new Uri("E:\\Downloads\\minecraft-logo-big.png")),
                            Created = DateTime.Now,
                            Modified = DateTime.Now,
                            Accessed = DateTime.Now,

                            FieldList = new MvxObservableCollection<Field>()
                            {
                                new Field()
                                {
                                    Name = "Username",
                                    Value = "Sauce Fire",
                                },

                                new Field()
                                {
                                    Name = "Password",
                                    Value = new byte[] { 1, 1, 0, 1, 1 },
                                },
                            }
                        },
                    }
                },
            };

            // Test export

            XmlSerializer xsSubmit = new XmlSerializer(typeof(Vault));
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, vault);
                    xml = sww.ToString();
                }
            }

            //File.WriteAllText("E:\\Downloads\\DEBUG.txt", xml);
        }

        #region Commands

        public IMvxAsyncCommand NavigateDatabaseView { get; private set; }
        public IMvxAsyncCommand NavigateEntryView { get; private set; }

        public IMvxAsyncCommand AddEntryCommand { get; private set; }

        private async Task AddEntry()
        {
            var newEntry = await _navigationService.Navigate<EntryViewModel, Entry, Entry>(
                new Entry()
                {
                    // Setting default color to special gray
                    Color = System.Windows.Media.Color.FromRgb(102, 115, 121)
                });
            if (newEntry != null)
            {
                System.Diagnostics.Debug.WriteLine($"VaultViewModel: Recieved {newEntry.Name}.");
                FolderEntries.Add(newEntry);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"EntryViewModel: Recieved no entries.");
            }
        }

        public IMvxAsyncCommand AddFolderCommand { get; private set; }

        public IMvxAsyncCommand EditEntryCommand { get; private set; }
        public IMvxAsyncCommand EditFolderCommand { get; private set; }

        public IMvxAsyncCommand RemoveEntryCommand { get; private set; }
        public IMvxAsyncCommand RemoveFolderCommand { get; private set; }

        #endregion Commands

        #region Properties

        public Entry SelectedEntry { get; set; }

        private Folder selectedFolder;

        public Folder SelectedFolder
        {
            get
            {
                return selectedFolder;
            }

            set
            {
                selectedFolder = value;
                RaisePropertyChanged(() => SelectedFolder);
                // Causing FolderEntries to update on UI
                RaisePropertyChanged(() => FolderEntries);
            }
        }

        public MvxObservableCollection<Folder> VaultFolders
        {
            get
            {
                if (Vault.Exists())
                {
                    return Vault.GetInstance().FolderList;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (Vault.Exists())
                {
                    Vault.GetInstance().FolderList = value;
                    RaisePropertyChanged(() => VaultFolders);
                }
            }
        }

        public MvxObservableCollection<Entry> FolderEntries
        {
            get
            {
                if (SelectedFolder != null)
                {
                    return SelectedFolder.EntryList;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (SelectedFolder != null)
                {
                    SelectedFolder.EntryList = value;
                    RaisePropertyChanged(() => FolderEntries);
                }
            }
        }

        #endregion Properties
    }
}