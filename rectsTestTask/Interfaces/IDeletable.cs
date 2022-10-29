namespace rectsTestTask.Interfaces
{
    public interface IDeletable
    {
        bool ToDelete { get; set; }
        int IterationsLeft { get; set; }
    }
}
