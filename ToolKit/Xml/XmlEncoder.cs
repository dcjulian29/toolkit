using System;
using System.Globalization;
using System.Text;
using Common.Logging;
using ToolKit.Validation;

namespace ToolKit.Xml
{
    /// <summary>
    /// This class contains two static methods to encode and decode text to be compatible with being
    /// put into an XML document
    /// </summary>
    public static class XmlEncoder
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(XmlEncoder));

        /// <summary>
        /// Decodes a string to remove XML entity markups
        /// </summary>
        /// <param name="inputText">Input text containing entity markups</param>
        /// <returns>an decoded string</returns>
        public static string Decode(string inputText)
        {
            inputText = Check.NotEmpty(inputText, nameof(inputText));

            // If the string doesn't have a & character, it doesn't have any entities.
            if (inputText.IndexOf('&') < 0)
            {
                return inputText;
            }

            // If the string contains a & but not a ;, it doesn't have any entities.
            if ((inputText.IndexOf('&') > -1) && (inputText.IndexOf(';') < 0))
            {
                return inputText;
            }

            var sb = new StringBuilder();

            for (var i = 0; i < inputText.Length; i++)
            {
                var ch = inputText[i];
                if (ch != '&')
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0}", ch);
                    continue;
                }

                var endOfEntity = inputText.IndexOfAny(new char[] { ';', '&' }, i + 1);

                // If the end marker is not ; then ignore this "entity" and continue on
                if ((endOfEntity > 0) && (inputText[endOfEntity] == '&'))
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "{0}", ch);
                    continue;
                }

                var entity = inputText.Substring(i, endOfEntity + 1 - i);

                if ((entity.Length > 1) && (entity[1] == '#'))
                {
                    entity = entity.Substring(2, entity.Length - 3);
                    try
                    {
                        if ((entity[0] == 'x') || (entity[0] == 'X'))
                        {
                            // It's encoded in hexadecimal
                            ch = (char)int.Parse(
                                entity.Substring(2),
                                NumberStyles.AllowHexSpecifier,
                                CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            // It's encoded in decimal
                            ch = (char)int.Parse(entity, CultureInfo.InvariantCulture);
                        }
                    }
                    catch (FormatException fex)
                    {
                        _log.Warn($"{inputText}: {fex.Message}", fex);
                        continue;
                    }
                    catch (ArgumentException aex)
                    {
                        _log.Warn(inputText + ": " + aex.Message, aex);
                        continue;
                    }
                }
                else
                {
                    // It's not an Entity Number but instead an Entity Name... Keep in mind that this
                    // is not the entire list of named entities. This Decode method is designed to
                    // decode only what was encoded with this class...
                    switch (entity)
                    {
                        case "&amp;":
                            ch = '&';
                            break;

                        case "&lt;":
                            ch = '<';
                            break;

                        case "&gt;":
                            ch = '>';
                            break;

                        case "&apos;":
                            ch = '\'';
                            break;

                        case "&quot;":
                            ch = '"';
                            break;

                        case "&nbsp;":
                            ch = ' ';
                            break;

                        default:

                            // I don't know what it is...
                            sb.Append(entity);
                            continue;
                    }
                }

                i = endOfEntity;
                sb.AppendFormat(CultureInfo.InvariantCulture, "{0}", ch);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Encodes a string to replace certain types of characters that are not "XML-Friendly"
        /// </summary>
        /// <param name="inputText">Input text to be encoded.</param>
        /// <returns>an encoded string</returns>
        public static string Encode(string inputText)
        {
            inputText = Check.NotNull(inputText, nameof(inputText));
            var sb = new StringBuilder();

            foreach (var item in inputText)
            {
                if ((item > 31) && (item < 127))
                {
                    switch (item)
                    {
                        case '&':
                            sb.Append("&amp;");
                            break;

                        case '<':
                            sb.Append("&lt;");
                            break;

                        case '>':
                            sb.Append("&gt;");
                            break;

                        case '\'':
                            sb.Append("&apos;");
                            break;

                        case '"':
                            sb.Append("&quot;");
                            break;

                        default:
                            sb.Append(item.ToString(CultureInfo.InvariantCulture));
                            break;
                    }
                }
                else
                {
                    sb.AppendFormat(CultureInfo.InvariantCulture, "&#{0};", (int)item);
                }
            }

            return sb.ToString().Replace("  ", "&nbsp; ");
        }
    }
}
