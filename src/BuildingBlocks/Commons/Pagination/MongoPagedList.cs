using MongoDB.Driver;

namespace Commons.Pagination
{
    public class MongoPagedList<T> : List<T>
    {
        public MongoPagedList(IEnumerable<T> items, long totalItems, int pageIndex, int pageSize)
        {
            MetaData = new MetaDataPageList
            {
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = pageIndex,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };
            AddRange(items);
        }

        private MetaDataPageList MetaData { get; }

        public MetaDataPageList GetMetaData()
        {
            return MetaData;
        }

        public static async Task<MongoPagedList<T>> ToPagedList(IMongoCollection<T> source, FilterDefinition<T> filter, int pageIndex, int pageSize, SortDefinition<T>? sort = null)
        {
            long count = await source.Find(filter).CountDocumentsAsync();
            IFindFluent<T, T> query = source.Find(filter);

            if (sort != null)
            {
                query = query.Sort(sort);
            }

            List<T> items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return new MongoPagedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
