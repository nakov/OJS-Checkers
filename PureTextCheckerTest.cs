using System;

class PureTextCheckerTest
{
    static void Main()
    {
        var checker = new PureTextChecker();
        string input = "";
        string expectedOutput =
            " Търсена  сума:500 " + "\r\n" +
            "Довиждане" + "\r\n";
        string userOutput =
            " Търсена  сума = 500.Довиждане!";
        var result = checker.Check(input, userOutput, expectedOutput, false);
        Console.WriteLine(result.IsCorrect);
        Console.WriteLine(result.CheckerDetails.Comment);
        Console.WriteLine(result.CheckerDetails.UserOutputFragment);
        Console.WriteLine(result.CheckerDetails.ExpectedOutputFragment);
    }
}
