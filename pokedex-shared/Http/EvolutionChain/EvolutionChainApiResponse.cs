namespace pokedex_shared.Http.EvolutionChain;

public class EvolutionChainApiResponse
{
    public Chain chain { get; set; }
    public int? id { get; set; }
}

public class Chain
{
    public Evolves_to[] evolves_to { get; set; }
    public bool is_baby { get; set; }
    public Species species { get; set; }
}

public class Evolves_to
{
    public Evolves_to[] evolves_to { get; set; }
    public bool is_baby { get; set; }
    public Species species { get; set; }
}

public class Species
{
    public string name { get; set; }
    public string url { get; set; }
}