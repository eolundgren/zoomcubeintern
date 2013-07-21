namespace PulseMates.Infrastructure.Extensions
{
    using System;
    using System.Linq;

    public static class IQueryableExtensions
    {
        public static Paged<T> AsPaged<T>(this IQueryable<T> queryable, int index = 0, int size = 20)
        {
            return new Paged<T>(queryable, index, size);
        }
    }

    public class Paged<T>
    {
        public Paged(IQueryable<T> innerQueryable, int index = 0, int size = 20)
        {
            Records = innerQueryable.Count();
            Pages = (int)Math.Ceiling((double)Records / (double)size);
            Rows = innerQueryable.Skip(index * size).Take(size);
        }

        public IQueryable<T> Rows { get; private set; }
        public int Pages { get; private set; }
        public int Records { get; private set; }
    }
}