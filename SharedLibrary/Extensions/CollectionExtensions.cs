namespace SharedLibrary.Extensions;

public static class CollectionExtensions
{
    public static bool IsNullOrNotAny<T>(this IEnumerable<T> source)
    {
        return source is null || !source.Any();
    }
}