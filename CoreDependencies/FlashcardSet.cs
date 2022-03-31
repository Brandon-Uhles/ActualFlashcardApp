//Based on an opensource flashcard container I found, will need editing for language learning specifics
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ActualFlashcardApp.CoreDependencies
{
    public class FlashcardSet
    {
        //Name of the flashcard set
        //Assigns the JSON property based on JSONHelper.cs & then sets that name when constructor is called.

        [JsonPropertyName(JSONHelper.SetNameJsonPropertyName)]
        public string SetName { get; set; }


        [JsonPropertyName(JSONHelper.FlashcardsListJsonPropertyName)]
        public List<Flashcard> FlashcardList { get; set; }


        //Constructor for creating flashcard list.
        //Sets the name of the set and attaches the flashcards in the set to the name.
        public FlashcardSet(string SetName, List<Flashcard> flashcardsList)
        {
            this.SetName = SetName;
            this.FlashcardList = flashcardsList;
        }


        //Verfies that the JSON being passed to the method is an actual flashcard set
        public override bool Equals(object? obj)
        {
            FlashcardSet flashSetObject = (FlashcardSet)obj;

            bool isListEqual = this.FlashcardList.SequenceEqual(flashSetObject.FlashcardList);
            if (flashSetObject.SetName.Equals(this.SetName) && isListEqual)
            {
                return true;
            }
            return false;
        }

        //Pulls hashcode for a specific flashcard set. Not currently implemented but can be used to define and apply flashcard statistics
        public override int GetHashCode()
        {
            return (SetName, FlashcardList).GetHashCode();
        }


        //Returns a string that represents the number of flashcards within a set. [_NUMBER flashcard(s)]
        public override string ToString()
        {
            return SetName + " [" +FlashcardList.Count + " flashcard" + (FlashcardList.Count == 1 ? "" : "s") + "]";
        }
    }
}
