using System;

using OJS.Workers.Common;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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

        // Scan for differences line by line
        for (int i = 0; i < receivedNumbers.Length; i++)
        {
            if (receivedNumbers[i] != expectedNumbers[i])
            {
                // Numbers do not match --> incorrect result
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
        string numberPattern = "(-)?[0-9]+(.[0-9]+)?";
        var numbers = new List<string>();
        var matches = Regex.Matches(outputText, numberPattern);
        foreach (var match in matches)
        {
            numbers.Add(match.ToString());
        }
        return numbers.ToArray();
    }

    public void SetParameter(string parameter)
    {
        throw new NotImplementedException();
    }
}
