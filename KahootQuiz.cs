using KahootUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KahootUtils
{
    class KahootQuiz(string quizId, string creator, string language, string title, DateTime created, DateTime modifed, List<KahootQuestion> questions)
    {
        #region read only fields
        private const string VALID_URL_START = "https://play.kahoot.it/v2/?quizId=";
        private const string URL = "https://create.kahoot.it/rest/kahoots/";

        public string QuizId { get; } = quizId;
        public string Creator { get; } = creator;
        public string Language { get; } = language;
        public string Title { get; } = title;
        public DateTime Created { get; } = created;
        public DateTime Modified { get; } = modifed;
        public List<KahootQuestion> Questions { get; } = questions;
        public int NumberOfQuestions => Questions.Count;
        public int TotalMaxPoints => Questions.Sum(q => q.MaxPoints);
        #endregion

        #region create quiz from input
        public static bool IsKahootURL(string url) => url.StartsWith(VALID_URL_START);
        public static KahootQuiz Get(string input)
        {
            if (IsKahootURL(input))
                return FromURL(input);
            return FromID(input);
        }
        public static KahootQuiz FromURL(string url)
        {
            if (!url.StartsWith(""))
                throw new ArgumentException("Invalid Kahoot URL!\nURL format should be: https://play.kahoot.it/v2/?quizId=QuizID");

            string quizId = url.Split("quizId=")[1].Split('&')[0];
            Console.WriteLine(quizId);
            return FromID(quizId);
        }
        public static KahootQuiz FromID(string quizId)
        {
            using HttpClient httpClient = new();
            using HttpRequestMessage request = new(HttpMethod.Get, $"{URL}{quizId}");
            HttpResponseMessage response = httpClient.Send(request);
            response.EnsureSuccessStatusCode();
            return FromJSON(response.Content.ReadAsStringAsync().Result);
        }
        public static KahootQuiz FromJSON(string json)
        {
            using JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;
            JsonElement.ArrayEnumerator questions = root.GetProperty("questions").EnumerateArray();
            return new KahootQuiz(
                root.GetProperty("uuid").GetString() ?? throw new Exception("Quiz ID not found in JSON!"),
                root.GetProperty("creator_username").GetString() ?? throw new Exception("Creator not found in JSON!"),
                root.GetProperty("language").GetString() ?? throw new Exception("Language not found in JSON!"),
                root.GetProperty("title").GetString() ?? throw new Exception("Title not found in JSON!"),
                HelpfulMethods.FromUnixTime(root.GetProperty("created").GetInt64()),
                HelpfulMethods.FromUnixTime(root.GetProperty("modified").GetInt64()),
                KahootQuestion.FromJSON(questions)
            );
        }
        #endregion

        public override string ToString()
        {
            return $"{Title} by {Creator} ({Language}) - ID: {QuizId}";
        }
    }
}
