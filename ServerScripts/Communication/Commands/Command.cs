using GUC.Server.Scripting.Objects.Character;
using System;

namespace GUC.Server.Scripts.Communication
{
  public delegate bool CommandDelegate(Player player, string[] parameters);

  /// <summary>
  /// This class is used to to encapsule an executable command using delegates
  ///
  /// </summary>
  public class Command
  {
    public string Name { get; private set; }

    public string[] Alias { get; private set; }

    public int NumArgs { get; private set; }

    private CommandDelegate function;

    public Command(string name, string[] alias, int numArgs, CommandDelegate func)
    {
      Name = name.Trim().ToLower();
      NumArgs = numArgs;
      function = func;
      Alias=alias;
    }
    public Command(string name, int numArgs, CommandDelegate func):this(name, new string[]{},numArgs,func)
    {
    }
    public Command(string name, CommandDelegate func)
      : this(name, 0, func)
    {
    }

    /// <summary>
    /// Executes a given command
    /// </summary>
    /// <param name="player"></param>
    /// <param name="parameters"></param>
    /// <returns>false, if there was an error( i.e. not enough arguments) </returns>
    internal bool Execute(Player player, string parameters)
    {
      string[] par = GetParameters(NumArgs, parameters);
      if (par.Length < NumArgs)//not enoug arguments given!
        return false;
      return function(player, par);
    }

    private string[] GetParameters(int numArgs, string parameters)
    {
      if (parameters.Length == 0)//is already trimmed!
        return new string[0];

      if (numArgs <= 1)
        return new string[1] { parameters.Trim() };

      string[] result = parameters.Split(new char[] { ' ' }, numArgs, StringSplitOptions.RemoveEmptyEntries);
      return result;
    }
  }
}