using System.ComponentModel.DataAnnotations;

namespace pokedex_shared.Option;

public class WorkerOption
{
    [Required]
    public int Interval { get; set; }
}