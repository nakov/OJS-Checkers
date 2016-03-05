using System;
using OJS.Workers.Common;

class TextSnippetCheckerTest
{
    static void Main7()
    {
        var checker = new TextSnippetChecker();

        string userOutput =
            " Добър ден. Въведете 2 числа: " + "\r\n" +
            " Търсена  сума:120.50 лв!!!" + "\r\n" +
            "Довиждане. Мерси за участието!" + "\r\n";
        string expectedOutput =
            " сума: 120.50 \r\n";

        TestChecker(checker, expectedOutput, userOutput);

        TestChecker(checker,
            expectedOutput: "Yes." + "\r\n" + "5 + 3 = 8",
            userOutput: "yes: 5+3=8");

        TestChecker(checker,
            expectedOutput: "No",
            userOutput: "sorry, no solution is found");
    }

    private static void TestChecker(IChecker checker, string expectedOutput, string userOutput)
    {
        var input = "";
        var result = checker.Check(input, userOutput, expectedOutput, false);

        Console.WriteLine("Correct: " + result.IsCorrect);
        if (!result.IsCorrect)
        {
            Console.WriteLine(result.CheckerDetails.Comment);
            Console.WriteLine("User output:");
            Console.WriteLine(result.CheckerDetails.UserOutputFragment);
            Console.WriteLine("Expected output:");
            Console.WriteLine(result.CheckerDetails.ExpectedOutputFragment);
        }
    }
}
