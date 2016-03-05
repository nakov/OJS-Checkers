using System;

using OJS.Workers.Common;

class LastNumberCheckerTest
{
    static void Main2()
    {
        var checker = new LastNumberChecker();

        string userOutput =
            "Моля въведете 3 числа:" + "\r\n" +
            "num[0] =" + "\r\n" +
            "num[1] =" + "\r\n" +
            "num[2] =" + "\r\n" +
            "Резултатът е: 500.26" + "\r\n" +
            "Довиждане!" + "\r\n";
        string expectedOutput =
            "500.255555555555555";

        TestChecker(checker, expectedOutput, userOutput);

        TestChecker(checker,
            expectedOutput: "error",
            userOutput: "Syntax Error!");
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
