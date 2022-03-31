using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ActualFlashcardApp;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using Microsoft.Win32;
using ActualFlashcardApp.CoreDependencies;
using ActualFlashcardApp.ViewModels;
using ActualFlashcardApp.Enums;
namespace ActualFlashcardApp.Views
{
    /// display information for NewDeckView.xaml. Should integrate with NewDeckViewModel; Going to have to refactor for that. Ask for help on Thurs?
    public partial class NewDeckView : UserControl
    {
        public NewDeckView()
        {
            InitializeComponent();
        }

        private void ToggleListBoxButtons()
        {
            // Enables buttons based on criteria of flashcard set
            moveDownButton.IsEnabled = moveUpButton.IsEnabled = flashcardsSetListBox.Items.Count > 1;
            removeButton.IsEnabled = flashcardsSetListBox.Items.Count > 0;
        }

        ///Takes user input and makes the flashcard object
        private void AddToSet()
        {
            if (string.IsNullOrWhiteSpace(termText.Text) || string.IsNullOrWhiteSpace(definitionText.Text))
            {
                MessageBox.Show("Please make sure none of the given fields contains whiespace", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            Flashcard flashcard = new Flashcard(termText.Text, definitionText.Text);
            flashcardsSetListBox.Items.Add(flashcard);

            termText.Clear();
            definitionText.Clear();

            ToggleListBoxButtons();
        }

        /// Swaps two Flashcard objects in the main list
        private void SwapCards(int index1, int index2)
        {
            Flashcard tempFlashcard = (Flashcard)flashcardsSetListBox.Items[index1];

            flashcardsSetListBox.Items[index1] = flashcardsSetListBox.Items[index2];
            flashcardsSetListBox.Items[index2] = tempFlashcard;
        }

        /// Event handler for when the "Add To Set" button is clicked.
        private void AddToSetButton_Click(object sender, RoutedEventArgs e)
        {
            AddToSet();
        }

        /// Event handler for when the "Save Flashcard Set" button is clicked.
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (flashcardsSetListBox.Items.Count == 0)
            {
                MessageBox.Show("You can't save an empty flashcard set!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            if (string.IsNullOrWhiteSpace(setNameText.Text))
            {
                MessageBox.Show("Invalid set name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            List<Flashcard> flashcards = new List<Flashcard>();
            foreach (Flashcard flashcard in flashcardsSetListBox.Items)
            {
                flashcards.Add(flashcard);
            }

            FlashcardSet flashcardSet = new FlashcardSet(setNameText.Text, flashcards);
            string flashcardsSetJson = JsonSerializer.Serialize(flashcardSet); // Serialize the 'flashcardSet' to JSON text

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = JSONHelper.DialogFilter;

            if (saveDialog.ShowDialog() == true) // Ok button was pressed on the save dialog, save the JSON text
            {
                try
                {
                    File.WriteAllText(saveDialog.FileName, flashcardsSetJson);

                    MessageBox.Show("Flashcard set saved succesfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured while trying to save the flashcard set.\n\nException message: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// Event handler for when the "Remove Selected Flashcard" button is clicked.
        private void RemoveSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = flashcardsSetListBox.SelectedIndex;
            flashcardsSetListBox.Items.RemoveAt(selectedIndex);

            // Find a new list box index to select
            selectedIndex = (selectedIndex == 0 && flashcardsSetListBox.Items.Count >= 1) ? selectedIndex : --selectedIndex;
            if (selectedIndex >= 0)
            {
                flashcardsSetListBox.SelectedItem = flashcardsSetListBox.Items.GetItemAt(selectedIndex);
            }

            ToggleListBoxButtons();
        }

        /// Event handler for when the "Move Flashcard Up" button is clicked.
        private void MoveUpButton_Click(object sender, RoutedEventArgs e)
        {
            if (flashcardsSetListBox.SelectedItem == null) // Don't move the flashcard up if no item is selected in the list box
            {
                MessageBox.Show("No flashcard is selected! Please select a flashcard and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            int selectedFlashcardIndex = flashcardsSetListBox.SelectedIndex;
            int upperIndex = selectedFlashcardIndex - 1; // Subtracting one since it points to the item above the selected index

            if (upperIndex < 0)
            {
                upperIndex = flashcardsSetListBox.Items.Count - 1;
            }

            SwapCards(selectedFlashcardIndex, upperIndex);

            flashcardsSetListBox.SelectedItem = flashcardsSetListBox.Items.GetItemAt(upperIndex);
        }

        /// Event handler for when the "Move Flashcard Down" button is clicked.
        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            if (flashcardsSetListBox.SelectedItem == null) // Don't move the flashcard down if no item is selected in the list box
            {
                MessageBox.Show("No flashcard is selected! Please select a flashcard and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            int selectedFlashcardIndex = flashcardsSetListBox.SelectedIndex;
            int lowerIndex = selectedFlashcardIndex + 1; // Adding one since it points to the item below the selected index

            if (lowerIndex >= flashcardsSetListBox.Items.Count)
            {
                lowerIndex = 0;
            }

            SwapCards(selectedFlashcardIndex, lowerIndex);

            flashcardsSetListBox.SelectedItem = flashcardsSetListBox.Items.GetItemAt(lowerIndex);
        }

        /// Event handler for when a key is held down on the window.
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AddToSet();
            }
        }
    }
}
