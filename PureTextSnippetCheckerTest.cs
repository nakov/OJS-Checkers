using System;

class PureTextSnippetCheckerTest
{
    static void Main()
    {
        var checker = new PureTextSnippetChecker();
        string input = "";
        string userOutput =
            " Добър ден. Въведете 2 числа: " + "\r\n" +
            " Търсена  сума:500 лв!!!" + "\r\n" +
            "Довиждане. Мерси за участието!" + "\r\n";
        string expectedOutput =
            " сума: 500";
        var result = checker.Check(input, userOutput, expectedOutput, false);
        Console.WriteLine(result.IsCorrect);
        Console.WriteLine(result.CheckerDetails.Comment);
        Console.WriteLine("User output: " + result.CheckerDetails.UserOutputFragment);
        Console.WriteLine("Expected output: " + result.CheckerDetails.ExpectedOutputFragment);
    }
}
