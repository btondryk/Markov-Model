using SymbolTables;
using System.Diagnostics;
using TreeSymbolTable;


class Program
{

    public static void Main(string[] args)
    {
        ///I TRIED TO MAKE IT SO I COULD READ IN MULTIPLE TEXT FILES BUT I WAS UNABLE TO
        if (args.Length < 3)
        {
            Console.WriteLine("Need more values");
        }
        //Reads the text file
        if (args[0] is not string)
        {
            throw new ArgumentException("Argument 1 was not a string or textfile");
        }

        string[] lines = File.ReadAllLines(args[0]);
        // I could not figure out how to ignore the enter keys I was unable to sadly
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = lines[i].Replace("\n", " ");
            lines[i] = lines[i].Replace("\r", " ");
        }


        if (!int.TryParse(args[1], out int N)) 
        {
            Console.WriteLine("Error: invalid Markov Model number. Please enter an integer.");
            return;
        }
        if (!int.TryParse(args[2], out int L)) // length
        {
            Console.WriteLine("Error: invalid Markov Model length. Please enter an integer.");
            return;
        }

        //stopwatch
        Stopwatch sortedDictionaryTime = new Stopwatch();
        //sorted dictionary
        sortedDictionaryTime.Start();
        SortedDictionary<string, MarkovEntry> sd_model = new SortedDictionary<string, MarkovEntry>();
        

        foreach (string l in lines)
        {

            string[] words = l.Split();

            if (l == "\n")
            {
                continue;
            }
            for (int i = 0; i < words.Length - N - 1; i++)
            {
                string markovKey = string.Join(" ", words.Skip(i).Take(N)); // Join N words - used Geeks for Geeks for string .join help

                if (!sd_model.ContainsKey(markovKey))
                {
                    sd_model[markovKey] = new MarkovEntry(markovKey);
                }

                string addvalue = words[i + N];               
                sd_model[markovKey].Add(addvalue);
            }
        }
        //I tried to use a list to put all the capital letters in but the title words and enter key kept messing me up
        List<string> randomWords = new List<string>();

        foreach (string key in sd_model.Keys)
        {
            if (key != key.ToLower() && key != key.ToUpper())
            {
                if (char.IsUpper(key[0]))
                {
                    randomWords.Add(key);
                }
            }
        }
        Random rng = new Random();

        string story1 = "";
        string startingword = randomWords[rng.Next(0, randomWords.Count)];
        story1 += startingword + " ";

        string[] word = lines[1].Split(); /*rng.Next(0, lines.Length)*/


        for (int i = 0; i < N; i++)
        {
            if (word[i] == "\n")
            {
                continue;
            }
            story1 += word[i] + " ";
        }


        // Continue generating the story
        for (int i = 0; i < L - N; i++)
        {
            string target = string.Join(" ", word.Skip(i).Take(N));
            story1 += sd_model[target].Random() + " ";

            if (i == L) // Check if length L is reached
            {
                if (!story1.EndsWith(".") ||!story1.EndsWith("?") ||!story1.EndsWith("!")) // Ensure the last word has an ending punctuation
                {
                    story1 += "."; // If the last word doesn't have punctuation, add one
                }
            }

        }
        


        Console.WriteLine("Generated Story1:");
        Console.WriteLine(story1);
        sortedDictionaryTime.Stop();
        Console.WriteLine($"Elapsed Time: {sortedDictionaryTime.ElapsedMilliseconds} milliseconds");

        Console.WriteLine();

        /////////////////NEXT STORY

        //same thing but for a sorted linked list
        Stopwatch sortedListTime = new Stopwatch();
        sortedListTime.Start();
        ListSymbolTable<string, MarkovEntry> lt_model = new ListSymbolTable<string, MarkovEntry>();

        

        foreach (string l in lines)
        {
            string[] words = l.Split();
            if (l == "\n")
            {
                continue;
            }
            for (int i = 0; i < words.Length - N - 1; i++)
            {
                string markovKey = string.Join(" ", words.Skip(i).Take(N)); // Join N words - used Geeks for Geeks for string .join help

                if (!lt_model.Contains(markovKey))
                {
                    MarkovEntry listKey = new MarkovEntry(markovKey);
                    lt_model.Add(markovKey, listKey);
                }

               
                string addvalue = words[i + N];
                
                lt_model[markovKey].Add(addvalue);

            }
        }
        string story2 = "";
        
        story2 += startingword + " ";

        string[] word2 = lines[1].Split();/*rng.Next(0, lines.Length)*/



        for (int i = 0; i < N; i++)
        {
            if (word2[i] == "\n")
            {
                continue;
            }
            story2 += word2[i] + " ";
        }


        // Continue generating the story
        for (int i = 0; i < L - N; i++)
        {
            string target = string.Join(" ", word2.Skip(i).Take(N));
            story2 += lt_model[target].Random() + " ";

        }

        Console.WriteLine("Generated Story2:");
        Console.WriteLine(story2);
        sortedListTime.Stop();
        Console.WriteLine($"Elapsed Time: {sortedListTime.ElapsedMilliseconds} milliseconds");

        Console.WriteLine();


        //////////// NEXT STORY

        //same thing but for the binary search stree
        Stopwatch SearchTreeTime = new Stopwatch();
        SearchTreeTime.Start();
        TreeSymbolTable<string, MarkovEntry> bst_model = new TreeSymbolTable<string, MarkovEntry>();
        

        foreach (string l in lines)
        {

            string[] words = l.Split();

            if (l == "\n")
            {
                continue;
            }
            for (int i = 0; i < words.Length - N - 1; i++)
            {
                string markovKey = string.Join(" ", words.Skip(i).Take(N)); // Join N words - used Geeks for Geeks for string .join help

                if (!bst_model.Contains(markovKey))
                {
                    MarkovEntry treeKey = new MarkovEntry(markovKey);
                    
                    bst_model.Add(markovKey, treeKey);
                }

                
                string addvalue = words[i + N];
                
                bst_model[markovKey].Add(addvalue);

            }
        }
        string story3 = "";
        
        story3 += startingword + " ";

        string[] word3 = lines[1].Split();/*rng.Next(0, lines.Length)*/

        

        for (int i = 0; i < N; i++)
        {
            if (word3[i] == "\n")
            {
                continue;
            }
            story3 += word3[i] + " ";
        }


        // Continue generating the story
        for (int i = 0; i < L - N; i++)
        {
            string target = string.Join(" ", word3.Skip(i).Take(N));
            story3 += bst_model[target].Random() + " ";

        }

        Console.WriteLine("Generated Story:");
        Console.WriteLine(story3);
        SearchTreeTime.Stop();
        Console.WriteLine($"Elapsed Time: {SearchTreeTime.ElapsedMilliseconds} milliseconds");

        Console.WriteLine();

    }
}



/// <summary>
/// This is the Markov entry class
/// </summary>
class MarkovEntry
{
    public string root;
    public int count;
    private SortedDictionary<string, int> nextWords;
    private Random random;
    private List<string> words = new List<string>();

    /// <summary>
    /// Constructor that initializes the root, nextwords, count and a random
    /// </summary>
    /// <param name="root"></param>
    public MarkovEntry(string root)
    {
        this.root = root;
        nextWords = new SortedDictionary<string, int>();
        count = 0;
        this.random = new Random();

    }
    /// <summary>
    /// Adds a word to the markov entry
    /// </summary>
    /// <param name="nextWord">keeps track of the words. Knows what the next word is</param>
    public void Add(string nextWord)
    {
        if (!nextWords.ContainsKey(nextWord))
        {
            nextWords.Add(nextWord, 0);
        }
        nextWords[nextWord]++;
        words.Add(nextWord);
        count++;

    }
    /// <summary>
    /// ToString part of the stepping stone 1 assignmnt
    /// </summary>
    /// <returns></returns>
    public override string ToString() //Geist Helped me with this part
    {
        string words = "";
        foreach (string item in nextWords.Keys)
        {
            words += " " + item + " " + nextWords[item] + ",";
        }
        return $"{root}({count}) : {words}";
    }
    /// <summary>
    /// returns a random word
    /// </summary>
    /// <returns></returns>
    public string Random()
    {
        return words[random.Next(0, count)];
    }
}

