namespace foxer.Core.Game.Entities
{
    public interface IConstruction
    {
        ConstructionLevel ConstructionLevel { get; }
    }

    public enum ConstructionLevel
    {
        Primitive
    }
}
