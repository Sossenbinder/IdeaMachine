namespace IdeaMachine.DataTypes.Validation
{
    public class ValidationInfo
    {
	    public IdeaValidationInfo Idea { get; set; } = null!;
    }

    public class IdeaValidationInfo
    {
	    public int MaxUploads { get; init; }

	    public int MaxByteSize { get; init; }
    }
}
