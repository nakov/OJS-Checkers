using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;

using OJS.Workers.Common;

public class LastNumberChecker : IChecker
{
    public CheckerResult Check(string inputData, string receivedOutput,
        string expectedOutput, bool isTrialTest)
    {
        string[] expectedNumbers = ExtractNumbersFromText(expectedOutput);
        if (expectedNumbers.Length == 0)
        {
            return CompareTextOutput(receivedOutput, expectedOutput);
        }

        if (expectedNumbers.Length != 1)
        {
            return new CheckerResult()
            {
                IsCorrect = false,
                ResultType = CheckerResultType.InvalidNumberOfLines,
                CheckerDetails = new CheckerDetails()
                {
                    Comment = "The test output should hold exactly one number.",
                    UserOutputFragment = receivedOutput,
                    ExpectedOutputFragment = string.Join("\r\n", expectedNumbers)
                }
            };
        }

        string[] receivedNumbers = ExtractNumbersFromText(receivedOutput);

        if (receivedNumbers.Length < 1)
        {
            return new CheckerResult()
            {
                IsCorrect = false,
                ResultType = CheckerResultType.InvalidNumberOfLines,
                CheckerDetails = new CheckerDetails()
                {
                    Comment = "The user output should hold at least one number.",
                    UserOutputFragment = receivedOutput,
                    ExpectedOutputFragment = string.Join("\r\n", expectedNumbers)
                }
            };
        }

        var lastReceivedNum = receivedNumbers[receivedNumbers.Length - 1];
        var expectedNum = expectedNumbers[0];
        bool equalNums;
        try
        {
            var numReceived = double.Parse(lastReceivedNum, CultureInfo.InvariantCulture);
            var numExpected = double.Parse(expectedNum, CultureInfo.InvariantCulture);
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
                    Comment = "The last number in the user output does not match the expected output.",
                    UserOutputFragment = lastReceivedNum,
                    ExpectedOutputFragment = expectedNum
                }
            };
        }
        
        // Correct result
        return new CheckerResult()
        {
            IsCorrect = true,
            ResultType = CheckerResultType.Ok,
            CheckerDetails = new CheckerDetails()
        };
    }

    private CheckerResult CompareTextOutput(string receivedOutput, string expectedOutput)
    {
        var receivedOutputCleaned =
            Regex.Replace(receivedOutput, @"\W+", " ").Trim().ToLowerInvariant();
        var expectedOutputCleaned =
            Regex.Replace(expectedOutput, @"\W+", " ").Trim().ToLowerInvariant();

        if (!Regex.IsMatch(receivedOutputCleaned, @"\b" + expectedOutputCleaned + @"\b"))
        {
            // The expected output was not found in the user output --> incorrect result
            return new CheckerResult()
            {
                IsCorrect = false,
                ResultType = CheckerResultType.WrongAnswer,
                CheckerDetails = new CheckerDetails()
                {
                    Comment = "The user output text does not contain the expected output text.",
                    UserOutputFragment = receivedOutputCleaned,
                    ExpectedOutputFragment = expectedOutputCleaned
                }
            };
        }

        // Correct result
        return new CheckerResult()
        {
            IsCorrect = true,
            ResultType = CheckerResultType.Ok,
            CheckerDetails = new CheckerDetails()
        };
    }

    private string[] ExtractNumbersFromText(string text)
    {
        string numberPattern = @"(-)?[0-9]+(\.[0-9]+)?(e[+-]?[0-9]+)?";
        var numbers = new List<string>();
        var matches = Regex.Matches(text, numberPattern);
        foreach (var m in matches)
        {
            Match match = (Match)m;
            if ((match.Index == 0) || (!char.IsLetter(text[match.Index - 1])))
            {
                if ((match.Index + match.Length == text.Length) ||
                    (!char.IsLetter(text[match.Index + match.Length])))
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
