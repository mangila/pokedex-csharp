using System.ComponentModel.DataAnnotations;

namespace pokedex_poller.Config;

public class WorkerOption
{
    [Required]
    public int Interval { get; set; }
}