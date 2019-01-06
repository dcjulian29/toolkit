using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using Common.Logging;

namespace ToolKit.Cryptography
{
    /// <summary>
    /// A class to generate secure passwords
    /// </summary>
    /// <remarks>
    /// This code was originally based of some code that a colleague of mine wrote but has been
    /// changed to remove proprietary information from the algorithm.
    /// </remarks>
    public class PasswordGenerator
    {
        private const string ExtendedCharacters = "{}\\;:/,<.>?";
        private const string LowerCaseCharacters = "abcdefghijklmnopqrstuvwxyz";
        private const string Numbers = "0123456789";
        private const string Symbols = "~!@#$%^&*-=+";
        private const string UpperCaseCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private static ILog _log = LogManager.GetLogger<PasswordGenerator>();

        private char[] _passwordCharacterArray;
        private Random _randomGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordGenerator"/> class.
        /// </summary>
        /// <param name="seed">The seed to use when generating the password.</param>
        public PasswordGenerator(int seed)
        {
            _randomGenerator = new Random(seed);
            PasswordOptions = PasswordComplexity.UseNumbers |
                              PasswordComplexity.UseSymbols |
                              PasswordComplexity.UseLowerCharacters |
                              PasswordComplexity.UseUpperCharacters |
                              PasswordComplexity.NoConsecutiveCharacters;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordGenerator"/> class.
        /// </summary>
        /// <param name="seed">The seed to use when generating the password.</param>
        /// <param name="options">PasswordGenerator Generation Options</param>
        public PasswordGenerator(int seed, PasswordComplexity options)
            : this(seed)
        {
            PasswordOptions = options;
        }

        /// <summary>
        /// Gets or sets the exclusions to use when generating the password.
        /// </summary>
        public string Exclusions { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets the inclusion of extended symbols
        /// </summary>
        public bool IncludeExtended
        {
            get => PasswordOptions.HasFlag(PasswordComplexity.UseExtendedCharacters);

            set => SetPasswordOption(PasswordComplexity.UseExtendedCharacters, value);
        }

        /// <summary>
        /// Gets or sets the inclusion of lower-case characters
        /// </summary>
        public bool IncludeLowerCase
        {
            get => PasswordOptions.HasFlag(PasswordComplexity.UseLowerCharacters);

            set => SetPasswordOption(PasswordComplexity.UseLowerCharacters, value);
        }

        /// <summary>
        /// Gets or sets the inclusion of numbers in the password
        /// </summary>
        public bool IncludeNumbers
        {
            get => PasswordOptions.HasFlag(PasswordComplexity.UseNumbers);

            set => SetPasswordOption(PasswordComplexity.UseNumbers, value);
        }

        /// <summary>
        /// Gets or sets the inclusion of symbols in the password
        /// </summary>
        public bool IncludeSymbols
        {
            get => PasswordOptions.HasFlag(PasswordComplexity.UseSymbols);

            set => SetPasswordOption(PasswordComplexity.UseSymbols, value);
        }

        /// <summary>
        /// Gets or sets the inclusion of upper-case characters in the password
        /// </summary>
        public bool IncludeUpperCase
        {
            get => PasswordOptions.HasFlag(PasswordComplexity.UseUpperCharacters);

            set => SetPasswordOption(PasswordComplexity.UseUpperCharacters, value);
        }

        /// <summary>
        /// Gets or sets the password options
        /// </summary>
        public PasswordComplexity PasswordOptions { get; set; }

        /// <summary>
        /// Gets or sets whether to allow consecutive characters
        /// </summary>
        public bool ProhibitConsecutiveCharacters
        {
            get => PasswordOptions.HasFlag(PasswordComplexity.NoConsecutiveCharacters);

            set => SetPasswordOption(PasswordComplexity.NoConsecutiveCharacters, value);
        }

        /// <summary>
        /// Gets or sets whether to allow any character to be used more than once
        /// </summary>
        public bool ProhibitRepeatingCharacters
        {
            get => PasswordOptions.HasFlag(PasswordComplexity.NoRepeatingCharacters);

            set => SetPasswordOption(PasswordComplexity.NoRepeatingCharacters, value);
        }

        /// <summary>
        /// Disable All PasswordGenerator Option Flags
        /// </summary>
        public void DisableAll()
        {
            foreach (PasswordComplexity value in Enum.GetValues(typeof(PasswordComplexity)))
            {
                SetPasswordOption(value, false);
            }
        }

        /// <summary>
        /// Generate a Password
        /// </summary>
        /// <param name="length">Length of Password</param>
        /// <returns>String representing the new password</returns>
        public string Generate(int length)
        {
            _passwordCharacterArray = BuildCharacterSet();

            if (ProhibitRepeatingCharacters && length > _passwordCharacterArray.Length)
            {
                throw new InvalidOperationException("Cannot build password; Not Enough Unique Characters Available");
            }

            var usedCharacters = new List<char>();
            var newPassword = new StringBuilder();
            var iterations = 0;
            var startTime = DateTime.UtcNow;
            char nextCharacter;

            var lastCharacter = '\n';

            for (var i = 0; i < length; i++)
            {
                bool charResult;
                do
                {
                    var randomCharPosition = GetRandomNumber();
                    nextCharacter = _passwordCharacterArray[randomCharPosition];

                    charResult = true;

                    if (ProhibitConsecutiveCharacters)
                    {
                        charResult &= lastCharacter != nextCharacter;
                    }

                    if (ProhibitRepeatingCharacters)
                    {
                        charResult &= !usedCharacters.Contains(nextCharacter);
                    }

                    if (Exclusions.Any())
                    {
                        charResult &= Exclusions.IndexOf(nextCharacter) == -1;
                    }

                    if (!charResult)
                    {
                        iterations++;
                    }
                } while (charResult == false);

                newPassword.Append(nextCharacter);
                usedCharacters.Add(nextCharacter);
                lastCharacter = nextCharacter;
            }

            _log.Debug(m => m("Total time was: {0} Seconds", DateTime.UtcNow.Subtract(startTime).TotalSeconds));
            _log.Debug(m => m("It took {0} iterations to generate the password", iterations));
            _log.Debug(m => m("New PasswordGenerator (Length of {0}) is: {1}", length, newPassword.Length));
            _log.Debug(m => m("The PasswordGenerator has {0} unique characters", usedCharacters.Count));

            return newPassword.ToString();
        }

        /// <summary>
        /// Generate a Password
        /// </summary>
        /// <param name="length">Length of Password</param>
        /// <returns>SecureString representing the new password</returns>
        public SecureString GenerateSecureString(int length)
        {
            var password = Generate(length);
            var securePassword = new SecureString();

            password.Each(c => securePassword.AppendChar(c));

            securePassword.MakeReadOnly();

            // ReSharper disable once RedundantAssignment
            password = null;

            return securePassword;
        }

        /// <summary>
        /// Enable All PasswordGenerator Option Flags
        /// </summary>
        public void IncludeAll()
        {
            foreach (PasswordComplexity value in Enum.GetValues(typeof(PasswordComplexity)))
            {
                SetPasswordOption(value, true);
            }
        }

        private char[] BuildCharacterSet()
        {
            var charSet = new StringBuilder();
            if (IncludeLowerCase)
            {
                charSet.Append(LowerCaseCharacters);
            }

            if (IncludeUpperCase)
            {
                charSet.Append(UpperCaseCharacters);
            }

            if (IncludeNumbers)
            {
                charSet.Append(Numbers);
            }

            if (IncludeSymbols)
            {
                charSet.Append(Symbols);
            }

            if (IncludeExtended)
            {
                charSet.Append(ExtendedCharacters);
            }

            return charSet.ToString().ToCharArray();
        }

        private int GetRandomNumber()
        {
            var charArrayLength = (uint)_passwordCharacterArray.Length;
            var randomNumberArray = new byte[4];

            _randomGenerator.NextBytes(randomNumberArray);

            var unsignedRandom = BitConverter.ToUInt32(randomNumberArray, 0);

            return Convert.ToInt32(unsignedRandom % charArrayLength);
        }

        private void SetPasswordOption(PasswordComplexity option, Boolean enabled)
        {
            if (enabled)
            {
                PasswordOptions |= option;
            }
            else
            {
                PasswordOptions &= ~option;
            }
        }
    }
}
