using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Notes
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" Notes\n Emilia Voronova\n");
            int command = 0;
            while (command != 5)
            {
                Console.WriteLine("\n Enter command :\n 1. Search Note\n 2. View Note\n 3. Create Note\n 4. Delete Note\n 5. Exit");
                HashSet<Note> notes = new HashSet<Note>();
                int id = 1;
                if (File.Exists("notes.json"))
                {
                    notes = JsonConvert.DeserializeObject<HashSet<Note>>(File.ReadAllText("notes.json"));
                    id = notes.Max(x => x.Id) + 1;
                }

                while (!int.TryParse(Console.ReadLine(), out command) || command < 1 || command > 5)
                    Console.WriteLine("Enter correct command :");
                switch (command)
                {
                    case 1:
                        Console.WriteLine(" Enter filter to search :");
                        string filter = Console.ReadLine();
                        List<Note> filtered = notes.Where(x => x.Text.Contains(filter)).OrderBy(x => x.Id).ToList();
                        Console.WriteLine(" Search results :");
                        foreach (Note filteredNote in filtered)
                        Console.WriteLine($"\n Note       : {filteredNote.Id}\n Header     : {filteredNote.Header}" +
                                        $"\n Created On : {filteredNote.CreatedOn}\n");
                    break;
                    case 2:
                        Console.WriteLine(" Enter an id to view the note :");
                        int search;
                        while (!int.TryParse(Console.ReadLine(), out search))
                            Console.WriteLine(" Invalid id. Enter an id :");
                        if (notes.Any(x => x.Id == search))
                        {
                            Note searchedNote = notes.Where(x => x.Id == search).First();
                            Console.WriteLine($"\n Note       : {searchedNote.Id}\n Header     : {searchedNote.Header}" +
                                            $"\n Text       : {searchedNote.Text}\n Created On : {searchedNote.CreatedOn}\n");
                        }
                        else
                            Console.WriteLine(" Note with such id doesn't exist.");
                        break;
                    case 3:
                        Console.WriteLine(" Enter your note :");
                        string text = Console.ReadLine().Trim();
                        string header = "";
                        if (!String.IsNullOrEmpty(text))
                        {
                            if (text.Length > 32)
                                header = text.Substring(0, 32);
                            else
                                header = text;
                            notes.Add(new Note(id, header, text, DateTime.UtcNow));
                            var jArray = JArray.Parse(JsonConvert.SerializeObject(notes)).ToString();
                            File.WriteAllText("notes.json", jArray);
                            Console.WriteLine(" Note cteated.");
                        }
                        else 
                            Console.WriteLine(" The note is empty so it is not created.");
                        break;
                    case 4:
                        Console.WriteLine(" Enter an id of note to delete :");
                        int identifier;
                        while (!int.TryParse(Console.ReadLine(), out identifier))
                            Console.WriteLine(" Incorrect identifier.");
                        if (notes.Any(x => x.Id == identifier))
                        {
                            Note noteToDelete = notes.Where(x => x.Id == identifier).First();
                            Console.WriteLine($"\n Note       : {noteToDelete.Id}\n Header     : {noteToDelete.Header}" +
                                            $"\n Text       : {noteToDelete.Text}\n Created On : {noteToDelete.CreatedOn}\n");
                            Console.WriteLine(" Are you sure you want to delete the note? y/n");
                            string reply = Console.ReadLine();
                            while (reply != "y" && reply != "n" && reply != "Y" && reply != "N")
                                reply = Console.ReadLine();
                            switch (reply)
                            {
                                case "Y":
                                case "y":
                                    notes.Remove(noteToDelete);
                                    var clone = JsonConvert.SerializeObject(notes);
                                    File.WriteAllText("notes.json", clone);
                                    Console.WriteLine($" Note with id {identifier} is deleted.");
                                    break;
                                case "N":
                                case "n":
                                    Console.WriteLine($" Note with id {identifier} is not deleted.");
                                    break;
                            }
                        }
                        else
                            Console.WriteLine(" There is no note with such id.");
                        break;
                    case 5:
                        return;
                }
            }
        }
    }
}
