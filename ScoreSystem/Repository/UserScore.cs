using Nest;
using System;

namespace ScoreSystem.Repository
{
	[ElasticsearchType(RelationName = "user_score")]
	internal class UserScore
	{
		[Keyword(NullValue = "null", Similarity = "BM25")]
		public string Username { get; set; }

		[Number(DocValues = false, IgnoreMalformed = true, Coerce = true)]
		public int Value { get; set; }

		[Date]
		public DateTimeOffset OccurredOn { get; set; }
	}
}