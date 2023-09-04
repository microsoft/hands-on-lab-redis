public interface ISimulatedDatabaseLatency 
{
    Task Wait();
}

// This class is used to simulate a database latency. 
// The latency is configured via the environment variable SIMULATED_DB_LATENCY_IN_SECONDS.
public class SimulatedDatabaseLatency : ISimulatedDatabaseLatency
{
    private readonly int _simulatedDBLatencyInSeconds;
    private readonly IConfiguration _configuration;

    public SimulatedDatabaseLatency(IConfiguration configuration)
    {
        _simulatedDBLatencyInSeconds = SimulatedLatencyInSeconds();
        _configuration = configuration;
    }

    private int SimulatedLatencyInSeconds()
    {
        try
        {
            string? latencyInSecondsAsString = _configuration["SIMULATED_DB_LATENCY_IN_SECONDS"];
            return String.IsNullOrEmpty(latencyInSecondsAsString) ? 0 : Int32.Parse(latencyInSecondsAsString);
        }
        catch
        {
            return 0;
        }
    }

    public async Task Wait()
    {
        if (_simulatedDBLatencyInSeconds > 0) {
            Console.WriteLine($"Simulating a latency of {_simulatedDBLatencyInSeconds} seconds");
            await Task.Delay(TimeSpan.FromSeconds(_simulatedDBLatencyInSeconds));
        }
    }
}