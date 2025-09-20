using System.Text;

namespace KahootUtils
{
    class Program
    {
        private const string VERSION = "1.0.0";
        private static Dictionary<string, Action> menuOptions = new()
        {
            { "About", About }
            { "Get Quiz Info", GetQuizInfo },
            { "Get Answers", GetAnswers },
            { "Exit", Exit }
        };
        private static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Kahoot Utils By Rost";
            Start();
        }
        private static void Start()
        {
            Console.Clear();
            Console.WriteLine("\r\n██╗░░██╗░█████╗░██╗░░██╗░█████╗░░█████╗░████████╗      ██╗░░░██╗████████╗██╗██╗░░░░░░██████╗\r\n██║░██╔╝██╔══██╗██║░░██║██╔══██╗██╔══██╗╚══██╔══╝      ██║░░░██║╚══██╔══╝██║██║░░░░░██╔════╝\r\n█████═╝░███████║███████║██║░░██║██║░░██║░░░██║░░░      ██║░░░██║░░░██║░░░██║██║░░░░░╚█████╗░\r\n██╔═██╗░██╔══██║██╔══██║██║░░██║██║░░██║░░░██║░░░      ██║░░░██║░░░██║░░░██║██║░░░░░░╚═══██╗\r\n██║░╚██╗██║░░██║██║░░██║╚█████╔╝╚█████╔╝░░░██║░░░      ╚██████╔╝░░░██║░░░██║███████╗██████╔╝\r\n╚═╝░░╚═╝╚═╝░░╚═╝╚═╝░░╚═╝░╚════╝░░╚════╝░░░░╚═╝░░░      ░╚═════╝░░░░╚═╝░░░╚═╝╚══════╝╚═════╝░");
            Console.WriteLine("Welcome to Kahoot Utils By Rost!\nType number of option that you want to do and then press enter!");
            
            int i = 0;
            foreach (string option in menuOptions.Keys)
                Console.WriteLine($"[{i++}] --- {option}");

            string? answer = Console.ReadLine();
            if (answer == null)
                Start();

            Console.WriteLine();

            if (int.TryParse(answer, out int index) && index > 0 && index <= menuOptions.Count)
            {
                string key = menuOptions.Keys.ElementAt(index);
                menuOptions[key]();
            }
            else
                Console.WriteLine("Invalid option!");

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            Start();
        }
        private static void About()
        {
            Console.WriteLine($"Kahoot Utils By Rost || Version: {VERSION} || GitHub: {}")
        }
        private static void GetQuizInfo()
        {
            Console.WriteLine("Please enter Kahoot Quiz URL or Quiz ID:");
            string? input = Console.ReadLine();
            if (input == null)
            {
                Console.WriteLine("Invalid input!");
                return;
            }
            try
            {
                KahootQuiz quiz = KahootQuiz.Get(input);
                Console.WriteLine($"\nQuiz Title: {quiz.Title}\nCreator: {quiz.Creator}\nLanguage: {quiz.Language}\nNumber of Questions: {quiz.Questions.Count}\nMax Points: {quiz.TotalMaxPoints}\nCreated: {quiz.Created}\nModified: {quiz.Modified}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private static void GetAnswers()
        {
            Console.WriteLine("Please enter Kahoot Quiz URL or Quiz ID:");
            string? input = Console.ReadLine();
            if (input == null)
            {
                Console.WriteLine("Invalid input!");
                return;
            }
            try
            {
                KahootQuiz quiz = KahootQuiz.Get(input);
                for (int i = 0; i < quiz.Questions.Count; i++)
                {
                    Console.ResetColor();
                    KahootQuestion question = quiz.Questions[i];
                    Console.WriteLine($"\nQ{i + 1}: {question.Question} ({question.MaxPoints} points, {question.SecondsTime} seconds)");
                    foreach (KahootChoice choice in question.Choices)
                    {
                        if (choice.Correct)
                            Console.ForegroundColor = ConsoleColor.Green;
                        
                        else
                            Console.ForegroundColor = ConsoleColor.Red;
                        
                        Console.WriteLine($" - {choice}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private static void Exit()
        {
            Environment.Exit(0);
        }
    }
}