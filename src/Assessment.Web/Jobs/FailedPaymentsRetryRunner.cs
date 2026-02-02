using Assessment.Application.Jobs;
using Hangfire;

namespace Assessment.Web.Jobs;

public class FailedPaymentsRetryRunner
{
    private readonly FailedPaymentsRetryJob _retryJob;

    public FailedPaymentsRetryRunner(FailedPaymentsRetryJob retryJob)
    {
        _retryJob = retryJob;
    }

    public async Task RunAsync(CancellationToken ct)
    {
        var ids = await _retryJob.GetRetryIdsAsync(ct);

        foreach (var id in ids)
        {
            BackgroundJob.Enqueue<PaymentProcessingJob>(
                "payments",
                j => j.ProcessAsync(id, CancellationToken.None)
            );
        }
    }
}
