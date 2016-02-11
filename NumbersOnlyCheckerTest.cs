using System;

class NumbersOnlyCheckerTest
{
    static void Main2()
    {
        var checker = new NumbersOnlyChecker();
        string input = "";
        string expectedOutput =
            "1000" + "\r\n" +
            "1200.000" + "\r\n" +
            "-123.56" + "\r\n" +
            "100" + "\r\n" +
            "-200" + "\r\n" +
            "3.14159" + "\r\n" +
            "-12.000" + "\r\n";
        string userOutput =
            "Това е най-якото решение!\r\n" +
            "1000.00" + "\r\n" +
            "12e2" + "\r\n" +
            "b1 = \r\n" +
            "b2 = \r\n" +
            "Сума за плащане: -123.5600000000001 лв." + "\r\n" +
            "По-голямо число: 100." + "\r\n" +
            "По-малко число:-200" + "\r\n" +
            "Пи = 3.14159" + "\r\n" +
            "\r\n" + "----------------" + "\r\n" +
            "Ресто: -12.00" + "\r\n";
        var result = checker.Check(input, userOutput, expectedOutput, false);
        Console.WriteLine(result.IsCorrect);
        Console.WriteLine(result.CheckerDetails.Comment);
        Console.WriteLine(result.CheckerDetails.UserOutputFragment);
        Console.WriteLine(result.CheckerDetails.ExpectedOutputFragment);
    }
}
