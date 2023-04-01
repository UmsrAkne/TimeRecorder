using Prism.Mvvm;

namespace TimeRecorder.Models;

public class ApplicationSetting : BindableBase
{
    private string defaultComment = string.Empty;
    private string defaultAutoComment = string.Empty;

    public string DefaultComment
    {
        get => defaultComment;
        set => SetProperty(ref defaultComment, value);
    }

    public string DefaultAutoComment
    {
        get => defaultAutoComment;
        set => SetProperty(ref defaultAutoComment, value);
    }
}