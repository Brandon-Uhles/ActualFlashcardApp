using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActualFlashcardApp.CoreDependencies
{
    public static class JSONHelper
    {
        /// Filter for windows filetypes when saving or opening a new file
        public const string DialogFilter = "JSON Files (*.json)|*.json|All files (*.*)|*.*";

        /// Sets the property name for the flashcard list
        public const string FlashcardsListJsonPropertyName = "flashcardsList";

        /// Sets the property name for the set of flashcards
        public const string SetNameJsonPropertyName = "setName";

        /// Sets the property name for one side of the flashcard
        public const string TermJsonPropertyName = "term";

        /// Sets the property name for the other side of the flashcard
        public const string DefinitionJsonPropertyName = "definition";
    }
}
