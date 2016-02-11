using System;

using OJS.Workers.Common;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;

public class NumbersChecker : IChecker
{
    public CheckerResult Check(string inputData, string receivedOutput,
        string expectedOutput, bool isTrialTest)
    {
        string[] expectedNumbers = ExtractNumbersFromText(expectedOutput);
        if (expectedNumbers.Length == 0)
        {
            return CompareTextOutput(receivedOutput, expectedOutput);
        }

        string[] receivedNumbers = ExtractNumbersFromText(receivedOutput);

        if (receivedNumbers.Length != expectedNumbers.Length)
        {
            // Different number of lines --> incorrect result
            string differenceText = (receivedNumbers.Length < expectedNumbers.Length) ? "less" : "greater";
            return new CheckerResult()
            {
                IsCorrect = false,
                ResultType = CheckerResultType.WrongAnswer,
                CheckerDetails = new CheckerDetails()
                {
                    Comment = "The number of numbers in the user output is " + differenceText + " than the expected output.",
                    UserOutputFragment = string.Join("\r\n", receivedNumbers),
                    ExpectedOutputFragment = string.Join("\r\n", expectedNumbers)
                }
            };
        }

        // Scan for differences number by number (line by line)
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
                        Comment = String.Format("Numbers '{0}' and '{1}' at line #{2} do not match.", receivedNumbers[i], expectedNumbers[i], (i + 1)),
                        UserOutputFragment = string.Join("\r\n", receivedNumbers),
                        ExpectedOutputFragment = string.Join("\r\n", expectedNumbers)
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
            if ((match.Index == 0) || (!char.IsLetter(text[match.Index-1])))
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
