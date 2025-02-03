using System.ComponentModel.DataAnnotations;

namespace pokedex_shared.Common.Option;

public class WorkerOption
{
    [Required] public required Interval Interval { get; set; }
    [Required] public required MediaHandler MediaHandler { get; set; }
}

public abstract class MediaHandler
{
    [Required] public required Interval Interval { get; set; }
}

public abstract class Interval
{
    [Required] public required int Min { get; init; }
    [Required] public required int Max { get; init; }
}