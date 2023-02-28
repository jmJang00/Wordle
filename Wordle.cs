using System;
using System.Collections;
using System.Collections.Specialized;

class Wordle {
    static bool IsCorrectString(HashSet<string> answers, string input)
    {
        bool bInputError = false;
        for (int i = 0; i < input.Length; ++i)
        {
            if (!Char.IsUpper(input[i]))
            {
                bInputError = true;
                break;
            }
        }

        if (bInputError || input.Length != 5)
        {
            Console.WriteLine("영어 대문자 5글자가 아닙니다.");
            return false;
        }

        if (!answers.Contains(input))
        {
            Console.WriteLine("사전에 없는 단어입니다.");
            return false;
        }

        return true;
    }

    // BATTA TTTDF

    static void Main() {
        var data = CSVReader.Read();

        /*
        Write your code below here.
        */
    
        Random random = new Random();
        var temp = random.Next(0, data.Count - 1);

        string answer = data[temp]["Name"];

        // Console.WriteLine(answer);

        var answers = new HashSet<string>();
        foreach (var row in data)
            answers.Add(row["Name"]);

        int test;
        for (test = 0; test < 5; ++test)
        {
            var numStrike = 0;
            var strike = new bool[5];
            var answerCharacters = new Dictionary<char, int>();
            var inputCharacters = new Dictionary<char, int>();
            var letterHit = new int[26];

            foreach (char c in answer)
                if (!answerCharacters.TryAdd(c, 1))
                    ++answerCharacters[c];

            
            Console.WriteLine("단어를 입력하세요. (영어 대문자 5글자) ; 이번을 포함한 남은 기회 {0}번", 5 - test);

            string input = Console.ReadLine();
            while (!IsCorrectString(answers, input))
                input = Console.ReadLine();

            // Strike Check
            for (int i = 0; i < 5; ++i)
            {
                if (input[i] == answer[i])
                {
                    strike[i] = true;
                    if (!inputCharacters.TryAdd(input[i], 1))
                        inputCharacters[input[i]] = 1;
                    ++numStrike;
                }
                inputCharacters.TryAdd(input[i], 0);
            }

            // Ball Check
            for (int i = 0; i < 5; ++i)
            {
                char ch = input[i];
                if (strike[i])
                {
                    letterHit[ch - 'A'] = 3;
                    Console.Write("G");
                    continue;
                }

                int answerCount = 0;
                int inputCount = 1000000000;
                answerCharacters.TryGetValue(ch, out answerCount);
                inputCharacters.TryGetValue(ch, out inputCount);
                if (answerCount > inputCount)
                {
                    letterHit[ch - 'A'] = int.Max(letterHit[ch - 'A'], 2);
                    Console.Write("Y");
                    ++inputCharacters[ch];
                }
                else
                {
                    letterHit[ch - 'A'] = 1;
                    Console.Write("B");
                }
            }
            Console.WriteLine();
            
            // Print all the alphabets and their records
            for (int i = 0; i < letterHit.Length; ++i)
            {
                Console.Write("{0}:{1}", (char)('A' + i), "NBYG"[letterHit[i]]);
                if (i < letterHit.Length - 1)
                    Console.Write(", ");
                if ((i + 1) % 5 == 0)
                    Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();

            if (numStrike == 5)
                break;
        }

        if (test != 5)
            Console.WriteLine("Correct!");
        else
            Console.WriteLine("Wrong!");
    }
}
