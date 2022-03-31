# ActualFlashcardApp

Hello! This is a quick WPF flashcard application designed to create, review, and edit flashcard decks. I initially planned to make this in Unity but
github source control is difficult to make work with large assets without additional paid resources. Eventually, I will port this over to Unity, to clean up
& beautify the assets. I'll add some gamification/RPG mechanics and turn it into a mobile app made in Unity, with integrated language learning! This is just a 
prototype of the review system. Assuming everything is up and functional, it will meet these requirements:

- Implement a “master loop” console application where the user can repeatedly enter commands/perform actions, including choosing to exit the program.
-     Met by applying the MVVM model to WPF (I hope), where the button bar is tied to functions within the view, the view passes through the view model, 
-     and the model passes commands through the viewmodel back to the view. This only is implemented for swapping to secondary views, I was having trouble
-     getting it to work with other program functions.

- Create an additional class which inherits one or more properties from its parent
-     Due to the implementation of MVVM, every view model inherits a property notification function from a BaseModel class, which invokes an event that is necessary for 
-     swapping menus. If that doesn't count, then the updateviewcommand class also inherits from ICommand, which is necessary for method definition when commands are invoked

- Create a dictionary or list, populate it with several values, retrieve at least one value, and use it in your program.
-     My NewDeckView.cs should create a list of new flashcards, and this is reflected in the list view. the key:value (term:definition) pairs are identified when
-     when you go to move or delete an existing card.

- Read data from an external file, such as text, JSON, CSV, etc and use that data in your application.
-     The program should read and write to a JSON file. The write functionality may not work. If it doesn't, I will include a test file for the read portion which you will
-     have to select for yourself. The data is used to display and test flash cards!

- Analyze text and display information about it (ex: how many words in a paragraph)
-   Analyzes inputted strings and displays errors if there are incorrect characters or missing whitespace.
