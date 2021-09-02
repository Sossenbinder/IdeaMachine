namespace IdeaMachine.Common.Core.Utils
{
	public class CallerInfo
	{
		public string MemberName { get; }
		public string SourceFilePath { get; }
		public int SourceLineNumber { get; }

		public CallerInfo(
			string memberName = "",
			string sourceFilePath = "",
			int sourceLineNumber = 0)
		{
			MemberName = memberName;
			SourceFilePath = sourceFilePath;
			SourceLineNumber = sourceLineNumber;
		}
	}
}