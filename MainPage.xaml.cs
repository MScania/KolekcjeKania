using System;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Maui.Controls;

namespace KolekcjeKania
{
    public partial class MainPage : ContentPage
    {
        private string _collectionsFolderPath;

        public ObservableCollection<CollectionPage> _collections;

        public MainPage()
        {
            InitializeComponent();

            _collectionsFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Collections");

            if (!Directory.Exists(_collectionsFolderPath))
                Directory.CreateDirectory(_collectionsFolderPath);

            _collections = new ObservableCollection<CollectionPage>();
            LoadCollections();
            collections.ItemsSource = _collections;
        }

        private async void AddCollection_Clicked(object sender, EventArgs e)
        {
            string name = collectionEntry.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
                return;

            CollectionPage newCollection = new CollectionPage();
            newCollection.Name = name;

            SaveCollectionFile(newCollection);
            _collections.Add(newCollection);

            collectionEntry.Text = string.Empty;
        }

        private void DeleteCollection_Clicked(object sender, EventArgs e)
        {
            CollectionPage selectedCollection = collections.SelectedItem as CollectionPage;

            if (selectedCollection == null)
                return;

            DeleteCollectionFile(selectedCollection);
            _collections.Remove(selectedCollection);
        }

        private async void CollectionSelected(object sender, EventArgs e)
        {
            if (collections.SelectedItem == null)
                return;

            CollectionPage selectedCollection = collections.SelectedItem as CollectionPage;
            await Navigation.PushAsync(new CollectionPage() { Name = selectedCollection.Name });
        }

        private void LoadCollections()
        {
            var files = Directory.GetFiles(_collectionsFolderPath);

            foreach (string file in files)
            {
                var fileName = Path.GetFileName(file).Split(".")[0];
                CollectionPage collection = new CollectionPage();
                collection.Name = fileName;

                _collections.Add(collection);
            }
        }

        private void SaveCollectionFile(CollectionPage collection)
        {
            string filePath = Path.Combine(_collectionsFolderPath, $"{collection.Name}.txt");
            try
            {
                File.Create(filePath).Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd podczas zapisywania kolekcji do pliku: " + ex.Message);
            }
        }

        private void DeleteCollectionFile(CollectionPage collection)
        {
            string filePath = Path.Combine(_collectionsFolderPath, $"{collection.Name}.txt");
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Błąd podczas usuwania kolekcji: " + ex.Message);
                }
            }
        }
    }
}