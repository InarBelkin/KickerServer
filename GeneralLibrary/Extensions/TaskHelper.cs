namespace GeneralLibrary.Extensions;

public static class TaskHelper
{
    public static async Task<List<TResult>> SequenceAwait<TResult>(this IEnumerable<Task<TResult>> collection)
    {
        var ret = new List<TResult>();

        foreach (var task in collection) ret.Add(await task);

        return ret;
    }
}