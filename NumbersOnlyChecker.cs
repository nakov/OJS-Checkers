﻿using System;

using OJS.Workers.Common;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;

public class NumbersOnlyChecker : IChecker
{
    public CheckerResult Check(string inputData, string receivedOutput,
        string expectedOutput, bool isTrialTest)
    {
        string[] receivedNumbers = ExtractLinesHoldingNumbers(receivedOutput);
        string[] expectedNumbers = expectedOutput.Split(
            new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        if (receivedNumbers.Length != expectedNumbers.Length)
        {
            // Different line numbers --> incorrect result
            string differenceText = (receivedNumbers.Length < expectedNumbers.Length) ? "less" : "greater";
            return new CheckerResult()
            {
                IsCorrect = false,
                ResultType = CheckerResultType.WrongAnswer,
                CheckerDetails = new CheckerDetails()
                {
                    Comment = "The count of numbers in the user output is " + differenceText + " than the expected output.",
                    UserOutputFragment = string.Join("\r\n", receivedNumbers),
                    ExpectedOutputFragment = string.Join("\r\n", expectedNumbers)
                }
            };
        }

        // Scan for differences line by line (number by number)
        for (int i = 0; i < receivedNumbers.Length; i++)
        {
            bool equalNums;
            try
            {
                var numReceived = double.Parse(receivedNumbers[i], CultureInfo.InvariantCulture);
                var numExpected = double.Parse(expectedNumbers[i], CultureInfo.InvariantCulture);
                equalNums = Math.Abs(numReceived - numExpected) < 0.001;
            }
            catch (Exception)
            {
                equalNums = false;
            }

            if (!equalNums)
            {
                // Numbers do not match (or parse failed) --> incorrect result
                return new CheckerResult()
                {
                    IsCorrect = false,
                    ResultType = CheckerResultType.WrongAnswer,
                    CheckerDetails = new CheckerDetails()
                    {
                        Comment = "Numbers at line #" + i + " do not match.",
                        UserOutputFragment = receivedNumbers[i],
                        ExpectedOutputFragment = expectedNumbers[i]
                    }
                };
            }
        }
        
        // Correct result
        return new CheckerResult()
        {
            IsCorrect = true,
            ResultType = CheckerResultType.Ok,
            CheckerDetails = new CheckerDetails()
        };
    }

    private string[] ExtractLinesHoldingNumbers(string outputText)
    {
        string numberPattern = @"(-)?[0-9]+(.[0-9]+)?";
        var numbers = new List<string>();
        var matches = Regex.Matches(outputText, numberPattern);
        foreach (var m in matches)
        {
            Match match = (Match)m;
            if ((match.Index == 0) || (!char.IsLetter(outputText[match.Index-1])))
            {
                if ((match.Index + match.Length == outputText.Length) ||
                    (!char.IsLetter(outputText[match.Index + match.Length])))
                {
                    numbers.Add(match.Value);
                }
            }
        }
        return numbers.ToArray();
    }

    public void SetParameter(string parameter)
    {
        throw new NotImplementedException();
    }
}
