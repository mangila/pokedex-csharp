namespace pokedex_shared.Common.Attributes;

/**
 * <summary>
 *  Mark as a Sad path
 * </summary>
 */
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class SadPathAttribute : Attribute;