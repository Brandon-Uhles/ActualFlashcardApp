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
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using Microsoft.Win32;
using ActualFlashcardApp.CoreDependencies;
using ActualFlashcardApp.Enums;

namespace ActualFlashcardApp.Views
{
    /// Interaction logic for LoadDeckView.xaml
    public partial class LoadDeckView : UserControl
    {
        /// Reference to the main window for when the application first starts.
        private MainWindow mainWindow;

        /// The current flashcard set the user is studying.
        private FlashcardSet currentFlashcardSet;

        /// The current flashcard index the user is looking at in the set.
        private int flashcardsListCurrentIndex = 0;

        /// States the flashcard face that the user is currently looking at (either term of definition).
        private FlashcardState currentFlashcardState;

        /// Main constructor for creating the window for studying flashcards.
        public LoadDeckView()
        {
            InitializeComponent();
            currentFlashcardState = FlashcardState.Term; // Make the term be the first thing the user sees for the first flashcard loaded
        }
        private void UpdateProgressText()
        {
            string progress = (currentFlashcardSet == null) ? "0/0" : (flashcardsListCurrentIndex + 1) + "/" + currentFlashcardSet.FlashcardList.Count;
            progressText.Text = progress;
        }


        /// Updates the UI area where the user can interact with the flashcard set.
 
        private void UpdateFlashcardUI()
        {
            UpdateProgressText();
            ToggleNavigationButtons();
            ToggleUnloadCurrentSetButton();

            if (currentFlashcardSet == null || currentFlashcardSet.FlashcardList.Count == 0) // If no flashcard is loaded, set the cursor for the flashcard container to the "No" cursor
            {
                flashcardContainer.Cursor = Cursors.No;
                setTitle.Text = "Load a set to start studying...";
                flashcardText.Text = "";

                return;
            }
            else
            {
                flashcardContainer.Cursor = Cursors.Hand;
            }

            setTitle.Text = currentFlashcardSet.SetName;

            // Update the flashcard text on the GUI depending on which side is visible
            Flashcard currentFlashcard = currentFlashcardSet.FlashcardList[flashcardsListCurrentIndex];
            flashcardText.Text = (currentFlashcardState == FlashcardState.Term) ? currentFlashcard.Term : currentFlashcard.Definition;
            flashcardText.FontWeight = (currentFlashcardState == FlashcardState.Term) ? FontWeights.Bold : FontWeights.Normal; // Definition is bold while term is normal font weight
        }

        /// Toggles the enable/disable status of the buttons for navigating through the flashcard set.
        private void ToggleNavigationButtons()
        {
            previousFlashcardButton.IsEnabled = (flashcardsListCurrentIndex > 0);
            nextFlashcardButton.IsEnabled = (flashcardsListCurrentIndex < (currentFlashcardSet == null ? -1 : currentFlashcardSet.FlashcardList.Count - 1));
        }

        /// Toggles the enable/disable status of the "Unload Current Set" button
        private void ToggleUnloadCurrentSetButton()
        {
            unloadCurrentSetButton.IsEnabled = (flashcardSetListBox.Items.Count > 0);
        }

        /// Checks whether a flashcard set is already present in the flashcard set list box on the window.
        private bool AlreadyLoaded(FlashcardSet flashcardSet)
        {
            foreach (FlashcardSet fs in flashcardSetListBox.Items)
            {
                if (flashcardSet.Equals(fs)) // Duplicate found
                {
                    return true;
                }
            }

            return false;
        }

        /// Goes to the previous flashcard in the current set (if there is one).
        private void PreviousFlashcard()
        {
            flashcardsListCurrentIndex--;
            if (flashcardsListCurrentIndex < 0)
            {
                flashcardsListCurrentIndex = 0;

                return;
            }

            currentFlashcardState = FlashcardState.Term;

            UpdateFlashcardUI();
        }


        /// Goes to the next flashcard in the current set (if there is one).
        private void NextFlashcard()
        {
            flashcardsListCurrentIndex++;
            if (flashcardsListCurrentIndex >= currentFlashcardSet.FlashcardList.Count)
            {
                flashcardsListCurrentIndex = currentFlashcardSet.FlashcardList.Count - 1;

                return;
            }

            currentFlashcardState = FlashcardState.Term;

            UpdateFlashcardUI();
        }

        /// Toggles the current flashcard face between the term and definition.
        private void ToggleFlashcardFace()
        {
            if (currentFlashcardSet == null)
            {
                return;
            }

            currentFlashcardState = (currentFlashcardState == FlashcardState.Definition) ? FlashcardState.Term : FlashcardState.Definition;

            UpdateFlashcardUI();
        }

        /// Event handler for when the "Load Flashcard Set" button is clicked
        private void LoadSetButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = JSONHelper.DialogFilter;

            if (openDialog.ShowDialog() == true) // Ok button was pressed on the dialog
            {
                try
                {
                    string fileText = File.ReadAllText(openDialog.FileName);

                    // Extract the information about the flashcard set from the JSON text
                    using (JsonDocument document = JsonDocument.Parse(fileText))
                    {
                        JsonElement root = document.RootElement;

                        string setName = root.GetProperty(JSONHelper.SetNameJsonPropertyName).ToString();

                        // Get the flashcards in the JSON
                        List<Flashcard> flashcardsList = new List<Flashcard>();
                        foreach (JsonElement element in root.GetProperty(JSONHelper.FlashcardsListJsonPropertyName).EnumerateArray())
                        {
                            string term = element.GetProperty(JSONHelper.TermJsonPropertyName).ToString();
                            string definition = element.GetProperty(JSONHelper.DefinitionJsonPropertyName).ToString();

                            Flashcard flashcard = new Flashcard(term, definition);
                            flashcardsList.Add(flashcard);
                        }

                        FlashcardSet set = new FlashcardSet(setName, flashcardsList);

                        if (AlreadyLoaded(set)) // If the flashcard set is already loaded, prompt the user to make sure that that want to load in the flashcard set again
                        {
                            MessageBoxResult msgResult = MessageBox.Show("Flashcard set has already been loaded. Are you sure you still want to load in flashcard set?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);

                            if (msgResult == MessageBoxResult.No)
                            {
                                return;
                            }
                        }

                        flashcardSetListBox.Items.Add(set);
                        flashcardSetListBox.SelectedItem = set;
                        flashcardsListCurrentIndex = 0;

                        UpdateFlashcardUI();

                        MessageBox.Show("Flashcard set loaded succesfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occured while trying to parse the file '" + openDialog.FileName + "' which resulted in the flashcard set not being loaded into the program.\n\nException message: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// Event handler for when the "Unload Current Set" button is clicked
        private void UnloadCurrentSetButton_Click(object sender, RoutedEventArgs e)
        {
            if (flashcardSetListBox.Items.Count > 0)
            {
                int selectedIndex = flashcardSetListBox.Items.IndexOf(currentFlashcardSet);
                flashcardSetListBox.Items.RemoveAt(selectedIndex);

                if (flashcardSetListBox.Items.Count > 0)
                {
                    flashcardSetListBox.SelectedIndex = 0;
                }
                else
                {
                    currentFlashcardSet = null;
                }

                currentFlashcardState = FlashcardState.Term;
                flashcardsListCurrentIndex = 0;

                UpdateFlashcardUI();
            }
        }

        /// Event handler for when the user clicks on the flashcard container with the left mouse button.
        private void FlashcardContainer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            flashcardContainer.Focus();

            ToggleFlashcardFace();
        }

        /// Event handler for when the "Previous Flashcard" button is clicked.
        private void PreviousFlashcardButton_Click(object sender, RoutedEventArgs e)
        {
            PreviousFlashcard();
        }

        /// Event handler for when the "Next Flashcard" button is clicked.
        private void NextFlashcardButton_Click(object sender, RoutedEventArgs e)
        {
            NextFlashcard();
        }

        /// Event handler for when the item selection changes in the flashcard set list box.
        private void FlashcardSetListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Update the current flashcard to the one selected
            currentFlashcardSet = (FlashcardSet)flashcardSetListBox.SelectedItem;
            currentFlashcardState = FlashcardState.Term;
            flashcardsListCurrentIndex = 0;

            UpdateFlashcardUI();
        }

        /// Event handler for when the window is closing
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (currentFlashcardSet != null && currentFlashcardSet.FlashcardList.Count > 0) // Show warning message if the user has a flashcard set currently loaded
            {
                string loadedFlashcardMessage = (currentFlashcardSet.FlashcardList.Count > 0) ? "You have flashcard sets" : "You have a flashcard set";

                if (MessageBox.Show("Are you sure you want to exit the window? " + loadedFlashcardMessage + " loaded in the program.", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    e.Cancel = true; // Set this to true to cancel the window from closing

                    return;
                }
            }

            mainWindow.Show();
        }

        /// Event handler for when a key is held down on the flashcard container
        private void FlashcardContainer_KeyDown(object sender, KeyEventArgs e)
        {
            if (currentFlashcardSet == null)
            {
                return;
            }

            if (e.Key == Key.Left || e.Key == Key.PageDown || e.Key == Key.Right || e.Key == Key.PageUp
                || e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Space)
            {
                e.Handled = true;
            }

            if (e.Key == Key.Left || e.Key == Key.PageDown) // Go to previous flashcard in the current set
            {
                PreviousFlashcard();
            }

            if (e.Key == Key.Right || e.Key == Key.PageUp) // Go to next flashcard in current set
            {
                NextFlashcard();
            }

            if (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Space) // Toggle term/definition face of current flashcard
            {
                ToggleFlashcardFace();
            }
        }



    }
}
