using System.Text.Json;
using UfwBulker;

internal class Program
{
    private static readonly JsonSerializerOptions serializerOptions = new() { PropertyNameCaseInsensitive = true };
    static Program()
    {
        serializerOptions.Converters.Add(new IPNetworkJsonConverter());
    }

    public static async Task<int> Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage:\r\n\tufwbulker <app_name> <input_file> [output_dir]");
            return 1;
        }

        string? appName = args.ElementAtOrDefault(0);
        string? inputFilePath = args.ElementAtOrDefault(1);
        string? outputDirectory = args.ElementAtOrDefault(2);

        if (string.IsNullOrWhiteSpace(appName))
        {
            Console.WriteLine("The ufw app name is not specifed.");
            return 1;
        }

        if (File.Exists(inputFilePath) is false)
        {
            Console.WriteLine("The input file not found.");
            return 1;
        }


        outputDirectory = string.IsNullOrWhiteSpace(outputDirectory) ? Directory.GetCurrentDirectory() : outputDirectory;
        if (Directory.Exists(outputDirectory) is false)
        {
            Console.WriteLine("The output directory not found.");
            return 1;
        }

        FileStream readStream = File.OpenRead(inputFilePath);
        SubnetWithComment[]? subnetsWithComment;
        try
        {
            subnetsWithComment = JsonSerializer.Deserialize<SubnetWithComment[]>(readStream, serializerOptions);
        }
        catch (JsonException)
        {
            Console.WriteLine("File can't be deserialized.");
            return 1;
        }
        finally
        {
            readStream.Dispose();
        }


        if (subnetsWithComment is null || subnetsWithComment.Length == 0)
        {
            Console.WriteLine("The input file is empty.");
            return 1;
        }

        string addScript = Helpers.FormatBashScript(subnetsWithComment, appName, Helpers.CreateAddRuleCommand);
        string removeScript = Helpers.FormatBashScript(subnetsWithComment, appName, Helpers.CreateRemoveRuleCommand);

        Task writeAddTask = File.WriteAllTextAsync(Path.Combine(outputDirectory, "add-script.bash"), addScript);
        Task writeRemoveTask = File.WriteAllTextAsync(Path.Combine(outputDirectory, "remove-script.bash"), removeScript);
        await Task.WhenAll(writeAddTask, writeRemoveTask);
        return 0;
    }

}
