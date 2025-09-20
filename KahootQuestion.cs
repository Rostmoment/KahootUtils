using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KahootUtils
{
    class KahootQuestion(string type, string question, int time, bool points, int pointsMultiplier, List<KahootChoice> choices)
    {
        public string Type { get; } = type;
        public string Question { get; } = question;

        public int Time { get; } = time;
        public int SecondsTime => Time / 1000;


        public bool GivesPoints { get; } = points;
        public int PointsMultiplier { get; } = pointsMultiplier;
        public int MaxPoints => GivesPoints ? 1000 * PointsMultiplier : 0;

        public List<KahootChoice> Choices { get; } = choices;
        public List<KahootChoice> CorrectChoices => Choices.Where(c => c.Correct).ToList();

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine($"{Question} ({Type}, {Time} seconds, x{PointsMultiplier} points)");
            foreach (KahootChoice choice in Choices)
                sb.AppendLine($" - {choice}");
            return sb.ToString().TrimEnd();
        }

        public static List<KahootQuestion> FromJSON(JsonElement.ArrayEnumerator questions)
        {
            List<KahootQuestion> questionList = new();
            while (questions.MoveNext())
            {
                JsonElement question = questions.Current;
                string type = question.GetProperty("type").GetString() ?? throw new Exception("Question type not found in JSON!");
                if (type != "quiz")
                    continue;

                string questionText = question.GetProperty("question").GetString() ?? throw new Exception("Question text not found in JSON!");
                int time = question.GetProperty("time").GetInt32();
                bool points = question.GetProperty("points").GetBoolean();
                int pointsMultiplier = question.GetProperty("pointsMultiplier").GetInt32();
                JsonElement.ArrayEnumerator choices = question.GetProperty("choices").EnumerateArray();
                List<KahootChoice> choiceList = new();
                while (choices.MoveNext())
                {
                    JsonElement choice = choices.Current;
                    string text = choice.GetProperty("answer").GetString() ?? throw new Exception("Choice text not found in JSON!");
                    bool correct = choice.GetProperty("correct").GetBoolean();
                    choiceList.Add(new KahootChoice(text, correct));
                }
                questionList.Add(new KahootQuestion(type, questionText, time, points, pointsMultiplier, choiceList));
            }
            return questionList;
        }
    }
}
