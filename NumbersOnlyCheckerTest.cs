﻿using System;

class NumbersOnlyCheckerTest
{
    static void Main()
    {
        var checker = new NumbersOnlyChecker();
        string input = "";
        string expectedOutput =
            "1 2 3" + "\r\n" +
            "1200.000" + "\r\n" +
            "-123.56" + "\r\n" +
            "100" + "\r\n" +
            "-200" + "\r\n" +
            "3.14159" + " " +
            "1.23e-1 1.23e+2 1.23e3" + " " +
            "-12.000" + "\r\n" +
            "4 5 6";
        string userOutput =
            "1 2 3 Това е най-якото решение!\r\n" +
            "12e2" + "\r\n" +
            "b1 = \r\n" +
            "b2 = \r\n" +
            "Сума за плащане: -123.5600000000001 лв." + "\r\n" +
            "По-голямо число: 100." + "\r\n" +
            "По-малко число:-200" + " " +
            "Пи = 3.14159" + "\r\n" +
            "Пи = 0.12300" + "\r\n" +
            "Пи = 123" + "\r\n" +
            "Пи = 1230" + "\r\n" +
            "\r\n" + "----------------" + "\r\n" +
            "Ресто: -12.00" + "\r\n" +
            "4 5 6";
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
