using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Microsoft.Maui.Controls;

namespace KolekcjeKania
{
    public partial class CollectionPage : ContentPage
    {
        public string Name { get; set; }

        public ObservableCollection<Element> _elements;

        public CollectionPage()
        {
            InitializeComponent();
            _elements = new ObservableCollection<Element>();
            elements.ItemsSource = _elements;
            LoadElementsFromFile();
        }

        private void AddElement_Clicked(object sender, EventArgs e)
        {
            string name = elementEntry.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
                return;

            Element element = new Element();
            element.Name = name;
            _elements.Add(element);
            SaveElementsToFile();

            elementEntry.Text = string.Empty; 
        }

        private void DeleteElement_Clicked(object sender, EventArgs e)
        {
            if (elements.SelectedItem is Element selectedElement)
            {
                _elements.Remove(selectedElement);
                SaveElementsToFile();
            }
        }

        private void LoadElementsFromFile()
        {
            string filePath = GetFilePath();
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    Element element = new Element();
                    element.Name = line;
                    _elements.Add(element);
                }
            }
        }

        private async void EditElement_Clicked(object sender, EventArgs e)
        {
            if (elements.SelectedItem is Element selectedElement)
            {
                string newName = await DisplayPromptAsync("Edycja elementu", "WprowadŸ now¹ nazwê elementu", initialValue: selectedElement.Name, accept: "Edytuj", cancel: "Anuluj");
                if (!string.IsNullOrWhiteSpace(newName) && newName != "Anuluj")
                {
                    selectedElement.Name = newName;
                    SaveElementsToFile();
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadElementsFromFile();
        }

        private void SaveElementsToFile()
        {
            string filePath = GetFilePath();
            File.WriteAllLines(filePath, _elements.Select(e => e.Name));
        }

        private string GetFilePath()
        {
            return Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Collections"), $"{Name}.txt");
        }
    }
}