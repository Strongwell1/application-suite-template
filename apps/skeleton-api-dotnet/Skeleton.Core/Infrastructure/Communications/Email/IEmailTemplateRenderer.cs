namespace Skeleton.Core.Infrastructure.Communications.Email;

public interface IEmailTemplateRenderer
{
    string Render(
        string template,
        IReadOnlyDictionary<string, string> values);
}
