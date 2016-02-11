using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using OJS.Workers.Common;

public class TextSnippetChecker : IChecker
{
    public CheckerResult Check(string inputData, string receivedOutput,
        string expectedOutput, bool isTrialTest)
    {
        var receivedOutputCleaned = ExtractWordsAndNumbers(receivedOutput);
        var expectedOutputCleaned = ExtractWordsAndNumbers(expectedOutput);

        if (! Regex.IsMatch(receivedOutputCleaned, @"\b" + expectedOutputCleaned + @"\b"))
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

    private string ExtractWordsAndNumbers(string text)
    {
        // Skip all punctiation and whitespace and extract all words and numbers
        var matches = Regex.Matches(text, @"((-)?[0-9]+(\.[0-9]+)?(e[+-]?[0-9]+)?)|\w+");
        var wordsAndNumbers = new List<string>();
        foreach (var m in matches)
        {
            Match match = (Match)m;
            wordsAndNumbers.Add(match.Value);
        }
        var wordsAndNumbersStr = string.Join(" ", wordsAndNumbers).ToLowerInvariant();
        return wordsAndNumbersStr;
    }
    
    public void SetParameter(string parameter)
    {
        throw new NotImplementedException();
    }
}
