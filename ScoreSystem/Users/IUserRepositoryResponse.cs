namespace ScoreSystem.Users
{
	public interface IUserRepositoryResponse<T>
	{
		bool IsDuplicated { get; }
		bool IsSuccessStatusCode { get; }
		string Message { get; }
		public T Object { get; set; }
	}
}