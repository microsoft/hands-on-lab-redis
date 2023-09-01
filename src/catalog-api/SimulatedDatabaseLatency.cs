public static class SimulatedDatabaseLatency
{
    private static readonly int simulatedDBLatencyInSeconds;

    static SimulatedDatabaseLatency() {
        simulatedDBLatencyInSeconds = SimulatedLatencyInSeconds();
    }

    private static int SimulatedLatencyInSeconds()
    {
        try
        {
            string? latencyInSecondsAsString = Environment.GetEnvironmentVariable("SIMULATED_DB_LATENCY_IN_SECONDS");
            return String.IsNullOrEmpty(latencyInSecondsAsString) ? 0 : Int32.Parse(latencyInSecondsAsString);
        }
        catch
        {
            return 0;
        }
    }

    public static async Task Wait()
    {
        if (simulatedDBLatencyInSeconds > 0) {
            Console.WriteLine($"Simulating a latency of {simulatedDBLatencyInSeconds} seconds");
            await Task.Delay(TimeSpan.FromSeconds(simulatedDBLatencyInSeconds));
        }
    }
}