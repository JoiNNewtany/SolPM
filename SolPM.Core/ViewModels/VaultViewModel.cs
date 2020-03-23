using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using SolPM.Core.Helpers;
using SolPM.Core.Models;
using System;
using System.Drawing;
using System.IO;
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

            // TODO: Get rid of test actions

            // Test Singleton Vault

            Vault vault = Vault.GetInstance();
            vault.Name = "My Test Vault";

            vault.FolderList = new MvxObservableCollection<Folder>()
            {
                new Folder()
                {
                    Name = "Folder 1",
                    Color = new XmlColor(Color.FromArgb(255, 0, 255)),

                    EntryList = new MvxObservableCollection<Entry>()
                    {
                        new Entry()
                        {
                            Name = "Google",
                            // Image = new BitmapImage(new Uri("E:\\Downloads\\PlaceholderLogo.png")),
                            Color = new XmlColor(Color.FromArgb(50, 50, 50)),
                            LastModified = DateTime.Now,

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
                                    Value = "BatIsQt",
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
                            // Image = new BitmapImage(new Uri("E:\\Downloads\\PlaceholderLogo.png")),
                            Color = new XmlColor(Color.FromArgb(70, 50, 100)),
                            LastModified = DateTime.Now,

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
                    Color = new XmlColor(Color.FromArgb(0, 0, 255)),

                    EntryList = new MvxObservableCollection<Entry>()
                    {
                        new Entry()
                        {
                            Name = "Mehcrossoft",
                            // Image = new BitmapImage(new Uri("E:\\Downloads\\PlaceholderLogo.png")),
                            Color = new XmlColor(Color.FromArgb(50, 50, 50)),
                            LastModified = DateTime.Now,

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
                            // Image = new BitmapImage(new Uri("E:\\Downloads\\PlaceholderLogo.png")),
                            Color = new XmlColor(Color.FromArgb(70, 50, 100)),
                            LastModified = DateTime.Now,

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
                            // Image = new BitmapImage(new Uri("E:\\Downloads\\PlaceholderLogo.png")),
                            Color = new XmlColor(Color.FromArgb(70, 50, 100)),
                            LastModified = DateTime.Now,

                            FieldList = new MvxObservableCollection<Field>()
                            {
                                new Field()
                                {
                                    Name = "Username",
                                    Value = "OWO UWU",
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
                    xml = sww.ToString(); // Your XML
                }
            }

            File.WriteAllText("E:\\Downloads\\DEBUG.txt", xml);

            // Commands

            NavigateDatabaseView = new MvxAsyncCommand(() => _navigationService.Navigate<DatabaseViewModel>());
            NavigateEntryView = new MvxAsyncCommand(() => _navigationService.Navigate<EntryViewModel>());
        }

        #region Commands

        public IMvxAsyncCommand NavigateDatabaseView { get; private set; }
        public IMvxAsyncCommand NavigateEntryView { get; private set; }

        #endregion Commands
    }
}