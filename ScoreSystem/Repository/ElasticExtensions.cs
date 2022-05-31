using Nest;

namespace ScoreSystem.Repository
{
	internal static class ElasticExtensions
	{
		public static QueryContainer FieldContains<T>(this QueryContainerDescriptor<T> descriptor, Field field, params object[] values) where T : class
		{
			QueryContainer q = new QueryContainer();
			foreach (var value in values)
			{
				q |= descriptor.Term(t => t.Field(field).Value(value));
			}
			return q;
		}
	}
}
