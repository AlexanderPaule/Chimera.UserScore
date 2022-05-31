using Nest;

namespace ScoreSystem.Repository
{
	[ElasticsearchType(RelationName = "users")]
	internal class User
	{
		[Keyword(NullValue = "null", Similarity = "BM25")]
		public string Username { get; set; }
	}
}
