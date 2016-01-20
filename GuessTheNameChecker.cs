using System;
using System.Linq;
using System.Text;

using OJS.Workers.Common;

public class GuessTheNameChecker : IChecker
{
    public CheckerResult Check(string inputData, string receivedOutput, string expectedOutput, bool isTrialTest)
    {
        var result = new CheckerResult();
        var details = new StringBuilder();

        var received = receivedOutput.Split(new[] { '\n', '\r' }, 
            StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower().Trim()).ToList();
        var expected = expectedOutput.Split(new[] { '\n', '\r' }, 
            StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower().Trim()).ToList();

        decimal points = 0;
        for (int i = 0; i < expected.Count; i++)
        {
            var exp = expected[i];

            if (i >= received.Count)
            {
                points -= 2;
                result.ResultType = CheckerResultType.InvalidNumberOfLines;
                details.AppendLine(string.Format("Answer #{0}: Empty line (-2). Expected: {1}", i + 1, exp));
                continue;
            }

            var rec = received[i];

            if (rec == "Unknown".ToLower())
            {
                details.AppendLine(string.Format("Answer #{0}: Unknown (0). Expected: {1}", i + 1, exp));
                continue;
            }

            if (rec == exp)
            {
                points++;
                details.AppendLine(string.Format("Answer #{0}: OK (+1). Expected: {1}", i + 1, exp));
            }
            else
            {
                points -= 2;
                details.AppendLine(string.Format("Answer #{0}: Wrong (-2). Expected: {1}, Received: {2}", i + 1, exp, rec.Substring(0, Math.Min(100, rec.Length))));
            }
        }

        var coefficient = points / expected.Count;
        details.AppendLine(string.Format("Total {0}/{1} = {2}", points, expected.Count, coefficient));
        if (coefficient >= 0.35M)
        {
            result.IsCorrect = true;
            result.ResultType = CheckerResultType.Ok;
        }
        else
        {
            result.IsCorrect = false;
            result.ResultType = CheckerResultType.WrongAnswer;
        }

        result.CheckerDetails = new CheckerDetails() { Comment = details.ToString() };

        return result;
    }

    public void SetParameter(string parameter)
    {
        throw new NotImplementedException();
    }
}
