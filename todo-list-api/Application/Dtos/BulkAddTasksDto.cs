public class BulkAddTasksDto
{
    public required int NumberOfTasks { get; set; }
    public required bool RandomizeTags { get; set; } = false;
    public required bool RandomizeDueDates { get; set; } = true;
    public required bool RandomizeStates { get; set; } = true;
}