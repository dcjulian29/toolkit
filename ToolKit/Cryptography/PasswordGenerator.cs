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

        private readonly Random _randomGenerator;
        private char[] _passwordCharacterArray;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordGenerator"/> class.
        /// </summary>
        /// <param name="seed">The seed to use when generating the password.</param>
        public PasswordGenerator(int seed)
        {
            _randomGenerator = new Random(seed);
            PasswordOptions = PasswordComplexities.UseNumbers |
                              PasswordComplexities.UseSymbols |
                              PasswordComplexities.UseLowerCharacters |
                              PasswordComplexities.UseUpperCharacters |
                              PasswordComplexities.NoConsecutiveCharacters;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ToolKit.Cryptography.PasswordGenerator"/> class.
        /// </summary>
        /// <param name="seed">The seed to use when generating the password.</param>
        /// <param name="options">PasswordGenerator Generation Options</param>
        public PasswordGenerator(int seed, PasswordComplexities options)
            : this(seed)
        {
            PasswordOptions = options;
        }

        /// <summary>
        /// Gets or sets the exclusions to use when generating the password.
        /// </summary>
        public string Exclusions { get; set; } = String.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether inclusion of extended symbols is used.
        /// </summary>
        public bool IncludeExtended
        {
            get => PasswordOptions.HasFlag(PasswordComplexities.UseExtendedCharacters);

            set => SetPasswordOption(PasswordComplexities.UseExtendedCharacters, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether inclusion of lower-case characters is used.
        /// </summary>
        public bool IncludeLowerCase
        {
            get => PasswordOptions.HasFlag(PasswordComplexities.UseLowerCharacters);

            set => SetPasswordOption(PasswordComplexities.UseLowerCharacters, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether inclusion of numbers is used.
        /// </summary>
        public bool IncludeNumbers
        {
            get => PasswordOptions.HasFlag(PasswordComplexities.UseNumbers);

            set => SetPasswordOption(PasswordComplexities.UseNumbers, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether inclusion of symbols is used.
        /// </summary>
        public bool IncludeSymbols
        {
            get => PasswordOptions.HasFlag(PasswordComplexities.UseSymbols);

            set => SetPasswordOption(PasswordComplexities.UseSymbols, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether inclusion of upper-case characters is used.
        /// </summary>
        public bool IncludeUpperCase
        {
            get => PasswordOptions.HasFlag(PasswordComplexities.UseUpperCharacters);

            set => SetPasswordOption(PasswordComplexities.UseUpperCharacters, value);
        }

        /// <summary>
        /// Gets or sets the password options
        /// </summary>
        public PasswordComplexities PasswordOptions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow consecutive characters
        /// </summary>
        public bool ProhibitConsecutiveCharacters
        {
            get => PasswordOptions.HasFlag(PasswordComplexities.NoConsecutiveCharacters);

            set => SetPasswordOption(PasswordComplexities.NoConsecutiveCharacters, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to allow any character to be used more than once
        /// </summary>
        public bool ProhibitRepeatingCharacters
        {
            get => PasswordOptions.HasFlag(PasswordComplexities.NoRepeatingCharacters);

            set => SetPasswordOption(PasswordComplexities.NoRepeatingCharacters, value);
        }

        /// <summary>
        /// Disable All PasswordGenerator Option Flags
        /// </summary>
        public void DisableAll()
        {
            foreach (PasswordComplexities value in Enum.GetValues(typeof(PasswordComplexities)))
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
                }
                while (!charResult);

                newPassword.Append(nextCharacter);
                usedCharacters.Add(nextCharacter);
                lastCharacter = nextCharacter;
            }

            _log.Debug($"Total time was: {DateTime.UtcNow.Subtract(startTime).TotalSeconds} Seconds");
            _log.Debug($"It took {iterations} iterations to generate the password");
            _log.Debug($"New PasswordGenerator (Length of {length}) is: {newPassword.Length}");
            _log.Debug($"The PasswordGenerator has {usedCharacters.Count} unique characters");

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

            unsafe
            {
                fixed (char* c = password)
                {
                    for (var i = 0; i < password.Length; i++)
                    {
                        c[i] = '\0';
                    }
                }
            }

            return securePassword;
        }

        /// <summary>
        /// Enable All PasswordGenerator Option Flags
        /// </summary>
        public void IncludeAll()
        {
            foreach (PasswordComplexities value in Enum.GetValues(typeof(PasswordComplexities)))
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

#pragma warning disable SG0005 // Weak random generator
            _randomGenerator.NextBytes(randomNumberArray);
#pragma warning restore SG0005 // Weak random generator

            var unsignedRandom = BitConverter.ToUInt32(randomNumberArray, 0);

            return Convert.ToInt32(unsignedRandom % charArrayLength);
        }

        private void SetPasswordOption(PasswordComplexities option, Boolean enabled)
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
