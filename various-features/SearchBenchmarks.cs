using System.Buffers;
using BenchmarkDotNet.Attributes;
public class SearchBenchmarks
{
    public static readonly string[] WordsArray = new[]
    {
        "apple", "banana", "cherry", "date", "elderberry",
        "fig", "grape", "honeydew", "kiwi", "lemon"
    };

    public readonly SearchValues<string> SearchWords = SearchValues.Create(WordsArray, StringComparison.OrdinalIgnoreCase);

    [Params("banana", "something", "berry", "elderberry")]
    public required string SearchTerm { get; set; }

    [Benchmark]
    public bool Array_Contains()
    {
        return WordsArray.Contains(SearchTerm, StringComparer.OrdinalIgnoreCase);
    }

    [Benchmark]
    public bool List_Contains()
    {
        return SearchWords.Contains(SearchTerm);
    }
}