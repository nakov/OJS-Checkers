using System;

using OJS.Workers.Common;
using System.Text.RegularExpressions;

public class PureTextChecker : IChecker
{
    public CheckerResult Check(string inputData, string receivedOutput,
        string expectedOutput, bool isTrialTest)
    {
        var receivedOutputCleaned = CleanText(receivedOutput);
        var expectedOutputCleaned = CleanText(expectedOutput);

        if (receivedOutputCleaned != expectedOutputCleaned)
        {
            // Different cleaned text --> incorrect result
            return new CheckerResult()
            {
                IsCorrect = false,
                ResultType = CheckerResultType.WrongAnswer,
                CheckerDetails = new CheckerDetails()
                {
                    Comment = "The text in the user output is different than the text in the expected output.",
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

    private string CleanText(string text)
    {
        // Replace any punctuation in the text with a single space
        text = Regex.Replace(text, @"[,:.;'""`/\\<>?!@#$%^&*()+=_{}\[\]\-]", " ");

        // Replace any whitespace sequences anywhere in the text with a single space
        text = Regex.Replace(text, @"\s+", " ");

        // Make all letters lower-case
        text = text.ToLowerInvariant();

        // Trim text
        text = text.Trim();

        return text;
    }
    
    public void SetParameter(string parameter)
    {
        throw new NotImplementedException();
    }
}
