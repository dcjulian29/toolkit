using System;
using System.ComponentModel;

namespace ToolKit.Cryptography
{
    /// <summary>
    /// An Enumeration of the features of a password to use when generating a random password.
    /// </summary>
    [Flags]
    public enum PasswordComplexities
    {
        /// <summary>
        /// No complexities should be included.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Should the password allow characters to be repeated
        /// </summary>
        NoRepeatingCharacters = 0x1,

        /// <summary>
        /// Should the password allow consecutive characters
        /// </summary>
        NoConsecutiveCharacters = 0x2,

        /// <summary>
        /// Should the password use extended Characters
        /// </summary>
        [Description("Extended, {}\\;:/,<.>?")]
        UseExtendedCharacters = 0x4,

        /// <summary>
        /// Should the password use lower-case characters
        /// </summary>
        [Description("Lower Case, a..z")]
        UseLowerCharacters = 0x10,

        /// <summary>
        /// Should the password use numbers
        /// </summary>
        [Description("Numbers, 0123456789")]
        UseNumbers = 0x20,

        /// <summary>
        /// Should the password use symbols
        /// </summary>
        [Description("Symbols,~!@#$%^&*-=+")]
        UseSymbols = 0x40,

        /// <summary>
        /// Should the password use upper-case characters
        /// </summary>
        [Description("Upper Case, A..Z")]
        UseUpperCharacters = 0x80,
    }
}
