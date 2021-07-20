using System.Collections.Generic;

namespace IdeaMachine.Common.Core.Utils.Pagination
{
	public class PaginationResult<TTokenType, TDataType>
	{
		public TTokenType? PaginationToken { get; }

		public IEnumerable<TDataType> Data { get; }

		public PaginationResult(TTokenType? paginationToken, IEnumerable<TDataType> data) =>
			(PaginationToken, Data) = (paginationToken, data);

		public PaginationResult<TTokenType, TNewDataType> WithNewPayload<TNewDataType>(IEnumerable<TNewDataType> newData)
			=> new(PaginationToken, newData);
	}
}