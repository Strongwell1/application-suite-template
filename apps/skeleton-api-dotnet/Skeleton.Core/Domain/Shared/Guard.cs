namespace Skeleton.Core.Domain.Shared;

/// <summary>
/// Provides common guard clause helpers for validating and normalizing inputs.
/// </summary>
public static class Guard
{
    // -------------------------------------------------------------------------
    // Null / default guards
    // -------------------------------------------------------------------------
    /// <summary>
    /// Ensures the provided reference value is not null.
    /// </summary>
    public static T NotNull<T>(T? value, string paramName) where T : class
        => value ?? throw new ArgumentNullException(paramName);

    /// <summary>
    /// Ensures the provided Guid is not empty.
    /// </summary>
    public static Guid NotEmpty(Guid value, string paramName)
        => value == Guid.Empty
            ? throw new ArgumentException("Guid cannot be empty.", paramName)
            : value;

    /// <summary>
    /// Ensures the provided struct value is not its default value.
    /// </summary>
    public static T NotDefault<T>(T value, string paramName) where T : struct
        => EqualityComparer<T>.Default.Equals(value, default)
            ? throw new ArgumentException("Value cannot be the default value.", paramName)
            : value;

    // -------------------------------------------------------------------------
    // String normalization and validation
    // -------------------------------------------------------------------------

    /// <summary>
    /// Trims a required string and ensures it is not null, empty, or whitespace.
    /// </summary>
    public static string TrimmedRequired(string? value, string paramName, int maxLength)
    {
        if (value is null)
            throw new ArgumentNullException(paramName);

        var trimmed = value.Trim();

        return trimmed.Length == 0 ? throw new ArgumentException("Value is required.", paramName) : MaxLength(trimmed, paramName, maxLength);
    }

    /// <summary>
    /// Trims an optional string and returns null when the value is null, empty, or whitespace.
    /// </summary>
    public static string? TrimmedOrNull(string? value, string paramName, int maxLength)
    {
        if (value is null)
            return null;

        var trimmed = value.Trim();

        if (trimmed.Length == 0)
            return null;

        return MaxLength(trimmed, paramName, maxLength);
    }

    /// <summary>
    /// Ensures the provided string is not null or empty.
    /// </summary>
    public static string NotNullOrEmpty(string? value, string paramName, int maxLength)
    {
        if (value is null)
            throw new ArgumentNullException(paramName);

        if (value.Length == 0)
            throw new ArgumentException("Value is required.", paramName);

        return MaxLength(value, paramName, maxLength);
    }

    /// <summary>
    /// Ensures the provided string does not exceed the specified maximum length.
    /// </summary>
    public static string MaxLength(string value, string paramName, int maxLength)
    {
        if (maxLength < 0)
            throw new ArgumentOutOfRangeException(nameof(maxLength), "Maximum length cannot be negative.");

        return value.Length > maxLength
            ? throw new ArgumentException($"Value cannot exceed {maxLength} characters.", paramName)
            : value;
    }

    /// <summary>
    /// Ensures the provided string meets the specified minimum length.
    /// </summary>
    public static string MinLength(string value, string paramName, int minLength)
    {
        if (minLength < 0)
            throw new ArgumentOutOfRangeException(nameof(minLength), "Minimum length cannot be negative.");

        return value.Length < minLength
            ? throw new ArgumentException($"Value must be at least {minLength} characters.", paramName)
            : value;
    }

    // -------------------------------------------------------------------------
    // Numeric guards
    // -------------------------------------------------------------------------

    /// <summary>
    /// Ensures the provided integer is greater than zero.
    /// </summary>
    public static int Positive(int value, string paramName)
        => value <= 0
            ? throw new ArgumentOutOfRangeException(paramName, "Value must be greater than zero.")
            : value;

    /// <summary>
    /// Ensures the provided decimal is greater than zero.
    /// </summary>
    public static decimal Positive(decimal value, string paramName)
        => value <= 0
            ? throw new ArgumentOutOfRangeException(paramName, "Value must be greater than zero.")
            : value;

    /// <summary>
    /// Ensures the provided integer is zero or greater.
    /// </summary>
    public static int NonNegative(int value, string paramName)
        => value < 0
            ? throw new ArgumentOutOfRangeException(paramName, "Value cannot be negative.")
            : value;

    /// <summary>
    /// Ensures the provided decimal is zero or greater.
    /// </summary>
    public static decimal NonNegative(decimal value, string paramName)
        => value < 0
            ? throw new ArgumentOutOfRangeException(paramName, "Value cannot be negative.")
            : value;

    /// <summary>
    /// Ensures the provided integer is within the inclusive range.
    /// </summary>
    public static int InRange(int value, string paramName, int min, int max)
    {
        if (min > max)
            throw new ArgumentException("Minimum value cannot be greater than maximum value.", nameof(min));

        return value < min || value > max
            ? throw new ArgumentOutOfRangeException(paramName, $"Value must be between {min} and {max}.")
            : value;
    }

    /// <summary>
    /// Ensures the provided decimal is within the inclusive range.
    /// </summary>
    public static decimal InRange(decimal value, string paramName, decimal min, decimal max)
    {
        if (min > max)
            throw new ArgumentException("Minimum value cannot be greater than maximum value.", nameof(min));

        return value < min || value > max
            ? throw new ArgumentOutOfRangeException(paramName, $"Value must be between {min} and {max}.")
            : value;
    }

    // -------------------------------------------------------------------------
    // Collection guards
    // -------------------------------------------------------------------------

    /// <summary>
    /// Ensures the provided collection is not null and contains at least one item.
    /// </summary>
    public static IReadOnlyCollection<T> NotEmpty<T>(IReadOnlyCollection<T>? value, string paramName)
    {
        if (value is null)
            throw new ArgumentNullException(paramName);

        if (value.Count == 0)
            throw new ArgumentException("Collection cannot be empty.", paramName);

        return value;
    }

    /// <summary>
    /// Ensures the provided sequence is not null and contains no null elements.
    /// </summary>
    public static IEnumerable<T> NoNullElements<T>(IEnumerable<T?>? value, string paramName) where T : class
    {
        if (value is null)
            throw new ArgumentNullException(paramName);

        foreach (var item in value)
        {
            if (item is null)
                throw new ArgumentException("Collection cannot contain null elements.", paramName);
        }

        return value!;
    }

    // -------------------------------------------------------------------------
    // Enum guards
    // -------------------------------------------------------------------------

    /// <summary>
    /// Ensures the provided enum value is a defined enum member.
    /// </summary>
    public static TEnum DefinedEnum<TEnum>(TEnum value, string paramName)
        where TEnum : struct, Enum
        => Enum.IsDefined(value)
            ? value
            : throw new ArgumentException("Value is not a defined enum member.", paramName);

    // -------------------------------------------------------------------------
    // Format-specific guards
    // -------------------------------------------------------------------------

    /// <summary>
    /// Validates and normalizes a required email address.
    /// </summary>
    public static string Email(string? value, string paramName, int MaxEmailLength)
    {
        var trimmed = TrimmedRequired(value, paramName, MaxEmailLength);

        if (trimmed.Contains(' '))
            throw new ArgumentException("Email address is not valid.", paramName);

        var at = trimmed.IndexOf('@');
        if (at <= 0 || at != trimmed.LastIndexOf('@') || at == trimmed.Length - 1)
            throw new ArgumentException("Email address is not valid.", paramName);

        var domain = trimmed[(at + 1)..];
        var dot = domain.IndexOf('.');
        if (dot <= 0 || dot == domain.Length - 1 || domain.StartsWith('.') || domain.EndsWith('.'))
            throw new ArgumentException("Email address is not valid.", paramName);

        return trimmed;
    }

    /// <summary>
    /// Validates and normalizes an optional email address.
    /// </summary>
    public static string? EmailOrNull(string? value, string paramName, int MaxEmailLength)
    {
        var trimmed = TrimmedOrNull(value, paramName, MaxEmailLength);
        return trimmed is null ? null : Email(trimmed, paramName, MaxEmailLength);
    }
}
