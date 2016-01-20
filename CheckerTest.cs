
using System;

class CheckerTest
{
    static void Main()
    {
        var checker = new NumbersOnlyChecker();
        string input = "";
        string expectedOutput =
            "-123.56" + "\r\n" +
            "100" + "\r\n" +
            "-200" + "\r\n" +
            "3.14159" + "\r\n" +
            "-12.000" + "\r\n";
        string userOutput =
            "Това е най-якото решение!\r\n" +
            "Сума за плащане: -123.56 лв." + "\r\n" +
            "По-голямо число: 100." + "\r\n" +
            "По-малко число:-200" + "\r\n" +
            "Пи = 3.14159" + "\r\n" +
            "\r\n" + "----------------" + "\r\n" +
            "Ресто: -12.00" + "\r\n";
        var result = checker.Check(input, userOutput, expectedOutput, false);
        Console.WriteLine(result.IsCorrect);
        Console.WriteLine(result.CheckerDetails.Comment);
    }
}
