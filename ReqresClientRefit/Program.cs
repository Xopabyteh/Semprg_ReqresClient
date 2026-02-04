var apiKey = Environment.GetEnvironmentVariable("quick_temporary_api_key", EnvironmentVariableTarget.User);
if (string.IsNullOrEmpty(apiKey))
{
	Console.WriteLine("API key not found in environment variable 'quick_temporary_api_key'. Please set it and try again.");
	return;
}

