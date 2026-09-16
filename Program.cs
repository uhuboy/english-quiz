using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace EnglishQuizApp
{
    class Question
    {
        public string Text { get; set; }
        public string[] Options { get; set; }
        public int CorrectIndex { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // โหลดข้อมูลจากไฟล์ JSON
            string jsonString = File.ReadAllText("questions.json");
            List<Question> bank = JsonSerializer.Deserialize<List<Question>>(jsonString);

            Random rng = new Random();
            // สุ่มเลือก 10 ข้อ
            var quiz = bank.OrderBy(x => rng.Next()).Take(10).ToList();
            int score = 0;

            Console.WriteLine("--- เริ่มทำแบบทดสอบภาษาอังกฤษ (10 ข้อ) ---");

            for (int i = 0; i < quiz.Count; i++)
            {
                var q = quiz[i];
                // สลับตัวเลือก
                var optionsWithIndex = q.Options.Select((val, idx) => new { val, idx }).OrderBy(x => rng.Next()).ToList();

                Console.WriteLine($"\nข้อ {i + 1}: {q.Text}");
                for (int j = 0; j < optionsWithIndex.Count; j++)
                {
                    Console.WriteLine($"{(char)('A' + j)}) {optionsWithIndex[j].val}");
                }

                Console.Write("ตอบ (A/B/C/D): ");
                string input = Console.ReadLine()?.ToUpper();
                int userChoice = input switch { "A" => 0, "B" => 1, "C" => 2, "D" => 3, _ => -1 };

                // --- ส่วนที่ปรับปรุง: เฉลยทันที ---
                if (userChoice != -1 && optionsWithIndex[userChoice].idx == q.CorrectIndex)
                {
                    Console.WriteLine("✅ ถูกต้อง!");
                    score++;
                }
                else
                {
                    // หาว่าตัวเลือกที่ถูกต้องคือตัวอักษรอะไร (A, B, C หรือ D)
                    int correctDisplayIdx = optionsWithIndex.FindIndex(x => x.idx == q.CorrectIndex);
                    char correctLetter = (char)('A' + correctDisplayIdx);
                    Console.WriteLine($"❌ ผิด! คำตอบที่ถูกต้องคือ {correctLetter}) {q.Options[q.CorrectIndex]}");
                }
                // -------------------------------
            }

            Console.WriteLine($"\n--- จบการทดสอบ ---");
            Console.WriteLine($"คะแนนของคุณ: {score}/20");
        }

        private static List<Question> GenerateQuestionBank()
        {
            List<Question> bank = new List<Question>();
            // จำลองการเพิ่ม 100 ข้อ
            for (int i = 1; i <= 100; i++)
            {
                bank.Add(new Question
                {
                    Text = $"คำถามข้อที่ {i}: เลือกคำตอบที่ถูกต้อง",
                    Options = new string[] { "Option A", "Option B", "Option C", "Option D" },
                    CorrectIndex = 0 // สมมติว่า index 0 คือคำตอบที่ถูก
                });
            }
            return bank;
        }
    }
}
