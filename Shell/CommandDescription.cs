using System;

namespace Venera.Shell;

public class CommandArgument
{
    /// <summary>
    /// Short form of the argumnt like -l or -a.
    /// </summary>
    public char? ShortForm { get; set; }
    /// <summary>
    /// Long form of the argument like --list or --all.
    /// </summary>
    public string LongForm { get; set; }
    /// <summary>
    /// If no short or long form exists then this argument position is enforced.
    /// <b>This value starts at zero</b> and all arguments passed with "-" are skipped.
    /// </summary>
    public int? ArgsPosition { get; set; }
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
    /// If this argument takes a value, give it a name like `bind_address`.
    /// </summary>
    public string ValueName { get; set; }
    /// <summary>
    /// If this argument isn't required and has a default, put it here.
    /// </summary>
    public string Default { get; set; }
    /// <summary>
    /// What kind of type does this argument accept?
    /// </summary>
    public Type Type { get; set; }

    public CommandArgument()
    {
        if (ShortForm == null && LongForm == null || ArgsPosition == null)
        {
            throw new ArgumentNullException("Command arguments are required to have at least one long form, short form or argument position. None were provided.");
        }
    }
}

public class CommandDescription
{
    /// <summary>
    /// Custom usage text can be set to override the automatically generated text.
    /// </summary>
    public string UsageText { get; set; }

    public CommandArgument[] Arguments { get; set; }
}
