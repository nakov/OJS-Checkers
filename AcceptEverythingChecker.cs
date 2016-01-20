using System;

using OJS.Workers.Common;

public class AcceptEverythingChecker : IChecker
{
    public CheckerResult Check(string inputData, string receivedOutput,
        string expectedOutput, bool isTrialTest)
    {
        return new CheckerResult()
        {
            IsCorrect = true,
            ResultType = CheckerResultType.Ok,
            CheckerDetails = new CheckerDetails()
        };
    }

    public void SetParameter(string parameter)
    {
        throw new NotImplementedException();
    }
}
