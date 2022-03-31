using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ActualFlashcardApp.CoreDependencies
{
    public class Flashcard
    {
        //defines the term of the flashcard and assigns the JSON property to it
        [JsonPropertyName(JSONHelper.TermJsonPropertyName)]
        public string Term { get; set; }

        //defines the definition (wow I should come up with a better term) of the flashcard and assigns JSON property
        [JsonPropertyName(JSONHelper.DefinitionJsonPropertyName)]
        public string Definition { get; set; }

        //Primary constructor for creating flashcards
        public Flashcard(string term, string definition)
        {
            this.Term = term;
            this.Definition = definition;
        }


        //Verifies that the flashcard object being passed to it is a real flashcard object.
        public override bool Equals(object? obj)
        {
            if(obj is Flashcard)
            {
                Flashcard flashObject = (Flashcard)obj;
                if (flashObject.Definition.Equals(this.Definition) && flashObject.Term.Equals(this.Term)){
                    return true;
                }
            }
            return false;
        }

        //Pulls the unique hash code for the individual flashcard. Can be used for statistics later
        public override int GetHashCode()
        {
            return (Term, Definition).GetHashCode();
        }

        //Returns a string that is representative of the indiviaul card in TERM : DEFINITION format
        public override string ToString()
        {
            return Term + " : " + Definition;
        }




    }
}
