namespace pokedex_shared.Common.Attributes;

/**
 * <summary>
 *  Mark as a Happy path
 * </summary>
 */
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class HappyPath : Attribute;