namespace Skeleton.Core.Infrastructure.Communications.Email;

public sealed class EmailTemplateRenderer : IEmailTemplateRenderer
{
    public string Render(
        string template,
        IReadOnlyDictionary<string, string> values)
    {
        ArgumentNullException.ThrowIfNull(template);
        ArgumentNullException.ThrowIfNull(values);

        var rendered = template;

        foreach (var pair in values)
        {
            rendered = rendered.Replace(
                $"{{{{{pair.Key}}}}}",
                pair.Value ?? string.Empty,
                StringComparison.Ordinal);
        }

        return rendered;
    }
}
