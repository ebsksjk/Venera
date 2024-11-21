using System;

namespace Venera.Shell;

public class CommandArgument
{
    public static readonly char ShortFormDefault = '°';

    /// <summary>
    /// If this argument takes a value, give it a name like `bind_address`.
    /// </summary>
    public string ValueName { get; set; }
    /// <summary>
    /// Short form of the argumnt like -l or -a.
    /// </summary>
    public char ShortForm { get; set; }
    /// <summary>
    /// Long form of the argument like --list or --all.
    /// </summary>
    public string LongForm { get; set; }
    /// <summary>
    /// If no short or long form exists then this argument position is enforced.
    /// <b>This value starts at zero</b> and all non-indexed arguments are skipped.
    /// 
    /// <para>Will be set to -1 if <see cref="Type"/> is set to string[]</para>
    /// </summary>
    public int ArgsPosition { get; set; } = int.MinValue;
    /// <summary>
    /// Declare if this argument must be provided. Failure to do so will result in
    /// ExitCode.InvalidArgument.
    /// </summary>
    public bool Required { get; set; } = false;
    /// <summary>
    /// What does this argument do? Give it a catchy description.
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// If this argument isn't required and has a default, put it here.
    /// </summary>
    public string ValueDefault { get; set; }
    /// <summary>
    /// What kind of type does this argument accept?
    /// </summary>
    public Type Type { get; set; }

    /// <summary>
    /// Create a command argument at a indexed position.
    /// </summary>
    public CommandArgument(string valueName, string description, Type type, int argsPosition, bool required = false, string valueDefault = null)
        : this(valueName, description, type, ShortFormDefault, null, argsPosition, required, valueDefault)
    {
    }

    /// <summary>
    /// Create a command argument with only a short form.
    /// </summary>
    public CommandArgument(string valueName, string description, Type type, char shortForm, bool required = false, string valueDefault = null)
        : this(valueName, description, type, shortForm, null, int.MinValue, required, valueDefault)
    {
    }

    /// <summary>
    /// Create a command argument with only a long form.
    /// </summary>
    public CommandArgument(string valueName, string description, Type type, string longForm, bool required = false, string valueDefault = null)
        : this(valueName, description, type, ShortFormDefault, longForm, int.MinValue, required, valueDefault)
    {
    }

    public CommandArgument(string valueName, string description, Type type, char shortForm, string longForm, int argsPosition = int.MinValue, bool required = false, string valueDefault = null)
    {
        ValueName = valueName;
        Description = description;
        ShortForm = shortForm;
        LongForm = longForm;
        Type = type;
        Required = required;
        ArgsPosition = argsPosition;
        ValueDefault = valueDefault;

        if (ValueName == null)
        {
            Kernel.PrintDebug("Command arguments must have a short value name. None have been provided.");
        }

        if (Type == typeof(string[]))
        {
            LongForm = "";
            ShortForm = 'ü';
            ArgsPosition = -1;
        }

        if (ShortForm == ShortFormDefault && LongForm == string.Empty)
        {
            Kernel.PrintDebug("Command arguments must have at least one long form, short form or argument position. None have been provided.");
        }
    }

    public override string ToString()
    {
        if (LongForm != null)
        {
            return $"--{LongForm} <{ValueName}>";
        }

        if (ShortForm != ShortFormDefault)
        {
            return $"-{ShortForm} <{ValueName}>";
        }

        // If this is a "endless" argument type at the very end.
        if (ArgsPosition == -1 && Type == typeof(string[]))
        {
            return $"<...{ValueName}>";
        }

        return $"<{ValueName}>";
    }
}

public class CommandDescription
{
    /// <summary>
    /// Custom usage text can be set to override the automatically generated text.
    /// </summary>
    public string UsageText { get; set; }

    public CommandArgument[] Arguments { get; set; } = [];

    public bool HasArguments { get { return Arguments != null && Arguments.Length > 0; } }
}
