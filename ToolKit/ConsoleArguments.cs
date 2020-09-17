using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ToolKit
{
    /// <summary>
    /// This Class will parse the command line arguments and provide a dictionary that can be used
    /// to determine the arguments that were passed to the program.
    /// </summary>
    public class ConsoleArguments : StringDictionary, IEnumerable<ConsoleArguments>
    {
        private Regex _removeQuotes;

        private Regex _spliter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleArguments" /> class.
        /// </summary>
        /// <param name="args">The arguments to parse.</param>
        public ConsoleArguments(string[] args)
        {
            Initialize();
            Parse(args);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleArguments" /> class.
        /// </summary>
        public ConsoleArguments()
        {
            Initialize();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        IEnumerator<ConsoleArguments> IEnumerable<ConsoleArguments>.GetEnumerator()
        {
            return (IEnumerator<ConsoleArguments>)GetEnumerator();
        }

        /// <summary>
        /// Checks if the parameter exist.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns><c>true</c> is the parameter exist; otherwise, <c>false</c>.</returns>
        public bool IsPresent(string parameterName)
        {
            return ContainsKey(parameterName);
        }

        /// <summary>
        /// Parses the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <example>Valid parameters forms: {-,/,--}parameter{ ,=,:}((",')value(",')).</example>
        public void Parse(string[] args)
        {
            if ((args == null) || (args.Length == 0))
            {
                return;
            }

            var parameterName = string.Empty;

            foreach (var argument in args)
            {
                // Look for new parameters (-,/ or --) and a possible enclosed value (=,:)
                var parameterParts = _spliter.Split(argument, 3);

                switch (parameterParts.Length)
                {
                    case 1: // Found a value (for the last parameter found (space separator))
                        if (!string.IsNullOrEmpty(parameterName))
                        {
                            if (!ContainsKey(parameterName))
                            {
                                parameterParts[0] = _removeQuotes.Replace(parameterParts[0], "$1");
                                Add(parameterName, parameterParts[0]);
                            }

                            parameterName = string.Empty;
                        }

                        break;

                    case 2: // Found just a parameter. If the last parameter is still waiting, set it to true.
                        SetParameterTrueIfNeeded(parameterName);
                        parameterName = parameterParts[1];
                        break;

                    case 3: // Parameter with enclosed value. If the last parameter is still waiting, set it to true.
                        if (argument.StartsWith("\"", StringComparison.CurrentCulture)
                            || argument.StartsWith("'", StringComparison.CurrentCulture))
                        {
                            // This is an enclosed value for the previous parameter that contains a
                            // parameter character. Example: /Description '--=nice=--'
                            if (!ContainsKey(parameterName))
                            {
                                Add(parameterName, _removeQuotes.Replace(argument, "$1"));
                            }
                        }
                        else
                        {
                            SetParameterTrueIfNeeded(parameterName);
                            parameterName = parameterParts[1];

                            // Remove possible enclosing characters (",')
                            if (!ContainsKey(parameterName))
                            {
                                parameterParts[2] = _removeQuotes.Replace(parameterParts[2], "$1");
                                Add(parameterName, parameterParts[2]);
                            }
                        }

                        parameterName = string.Empty;
                        break;
                }
            }

            // If the last parameter is still waiting, set it to true.
            SetParameterTrueIfNeeded(parameterName);
        }

        /// <summary>
        /// Converts the parameter to Boolean if the parameter exist and does not contain a value.
        /// </summary>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns>
        /// <c>true</c> is the parameter exist and its string representation is true; otherwise, <c>false</c>.
        /// </returns>
        public bool ToBoolean(string parameterName)
        {
            return ContainsKey(parameterName) && Convert.ToBoolean(this[parameterName], CultureInfo.InvariantCulture);
        }

        private void Initialize()
        {
            _spliter = new Regex(
                @"^-{1,2}|^/|=|(?<!^\w):",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            _removeQuotes = new Regex(
                @"^['""]?(.*?)['""]?$",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        private void SetParameterTrueIfNeeded(string parameterName)
        {
            if ((!string.IsNullOrEmpty(parameterName)) && (!ContainsKey(parameterName)))
            {
                Add(parameterName, "True");
            }
        }
    }
}
