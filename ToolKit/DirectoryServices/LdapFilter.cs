using System;
using System.Text;
using ToolKit.Validation;

namespace ToolKit.DirectoryServices
{
    /// <summary>
    /// The Lightweight Directory Access Protocol (LDAP) defines a network representation of a
    /// search filter transmitted to an LDAP server. This class attempts to represent these filters
    /// in a human-readable form as defined in RFC 2254 and RFC 1960. No attempt to parse the filter
    /// is done in this class and should support both LDAP V2 and LDAP V3 filters.
    /// </summary>
    public class LdapFilter
    {
        private readonly string _filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="LdapFilter"/> class.
        /// </summary>
        /// <param name="attributeType">The attribute of the filter.</param>
        /// <param name="filterType">The type of the filter.</param>
        /// <param name="attributeValue">The value of the filter.</param>
        public LdapFilter(string attributeType, string filterType, string attributeValue)
        {
            _filter = $"{attributeType}{filterType}{attributeValue}";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LdapFilter"/> class.
        /// </summary>
        /// <param name="ldapFilter">A LDAP Filter.</param>
        public LdapFilter(string ldapFilter)
        {
            _filter = Strip(Check.NotEmpty(ldapFilter, nameof(ldapFilter)));
        }

        private LdapFilter()
        {
        }

        /// <summary>
        /// Gets the value that cn be used in the attributeValue as a wild card.
        /// </summary>
        public static string Any => "*";

        /// <summary>
        /// Gets the value that can be used in the filterType to express approximation.
        /// </summary>
        public static string Approx => "~=";

        /// <summary>
        /// Gets the value that can be used in the filterType to express equality.
        /// </summary>
        public static string Equal => "=";

        /// <summary>
        /// Gets the value that can be used in the filterType to express Greater Than.
        /// </summary>
        public static string Greater => ">=";

        /// <summary>
        /// Gets the value that can be used in the filterType to express Less Than.
        /// </summary>
        public static string Less => "<=";

        /// <summary>
        /// Combines the provided LDAP filter(s) to create a new And LDAP Filter.
        /// </summary>
        /// <param name="filters">A LDAP Filter.</param>
        /// <returns>a filter that contains an And LDAP Filter.</returns>
        public static LdapFilter And(params LdapFilter[] filters)
        {
            var newFilter = new StringBuilder();

            newFilter.Append("(&");

            foreach (var filter in filters)
            {
                newFilter.Append(filter.ToString());
            }

            newFilter.Append(')');

            return new LdapFilter(newFilter.ToString());
        }

        /// <summary>
        /// Combines the provided LDAP filter(s) to create a new Or LDAP Filter.
        /// </summary>
        /// <param name="filters">A LDAP Filter.</param>
        /// <returns>a filter that contains an Or LDAP Filter.</returns>
        public static LdapFilter Or(params LdapFilter[] filters)
        {
            var newFilter = new StringBuilder();

            newFilter.Append("(|");

            foreach (var filter in filters)
            {
                newFilter.Append(filter.ToString());
            }

            newFilter.Append(')');

            return new LdapFilter(newFilter.ToString());
        }

        /// <summary>
        /// Combine the existing LDAP filter with the specified LDAP filter to create a new And LDAP Filter.
        /// </summary>
        /// <param name="attributeType">The attribute of the filter.</param>
        /// <param name="filterType">The type of the filter.</param>
        /// <param name="attributeValue">The value of the filter.</param>
        /// <returns>a filter that contains an And LDAP Filter.</returns>
        public LdapFilter And(string attributeType, string filterType, string attributeValue)
        {
            return And(new LdapFilter(attributeType, filterType, attributeValue), false);
        }

        /// <summary>
        /// Combine the existing LDAP filter with the specified LDAP filter to create a new And LDAP Filter.
        /// </summary>
        /// <param name="attributeType">The attribute of the filter.</param>
        /// <param name="filterType">The type of the filter.</param>
        /// <param name="attributeValue">The value of the filter.</param>
        /// <param name="appendFilter">if set to <c>true</c> append the filter.</param>
        /// <returns>a filter that contains an And LDAP Filter.</returns>
        public LdapFilter And(
            string attributeType,
            string filterType,
            string attributeValue,
            bool appendFilter)
        {
            return And(new LdapFilter(attributeType, filterType, attributeValue), appendFilter);
        }

        /// <summary>
        /// Combine the existing LDAP filter with the specified LDAP filter to create a new And LDAP Filter.
        /// </summary>
        /// <param name="ldapFilter">A LDAP Filter.</param>
        /// <returns>a filter that contains an And LDAP Filter.</returns>
        public LdapFilter And(string ldapFilter)
        {
            return And(ldapFilter, false);
        }

        /// <summary>
        /// Combine the existing LDAP filter with the specified LDAP filter to create a new And LDAP Filter.
        /// </summary>
        /// <param name="ldapFilter">A LDAP Filter.</param>
        /// <param name="appendFilter">if set to <c>true</c> append the filter.</param>
        /// <returns>a filter that contains an And LDAP Filter.</returns>
        public LdapFilter And(string ldapFilter, bool appendFilter)
        {
            Check.NotEmpty(ldapFilter, nameof(ldapFilter));

            return appendFilter
                ? new LdapFilter($"(&({_filter})({Strip(ldapFilter)}))")
                : new LdapFilter($"(&({Strip(ldapFilter)})({_filter}))");
        }

        /// <summary>
        /// Combine the existing LDAP filter with the specified LDAP filter to create a new And LDAP Filter.
        /// </summary>
        /// <param name="ldapFilter">A LDAP Filter.</param>
        /// <returns>a filter that contains an And LDAP Filter.</returns>
        public LdapFilter And(LdapFilter ldapFilter)
        {
            return And(Check.NotNull(ldapFilter, nameof(ldapFilter)).ToString(), false);
        }

        /// <summary>
        /// Combine the existing LDAP filter with the specified LDAP filter to create a new And LDAP Filter.
        /// </summary>
        /// <param name="ldapFilter">A LDAP Filter.</param>
        /// <param name="appendFilter">if set to <c>true</c> append the filter.</param>
        /// <returns>a filter that contains an And LDAP Filter.</returns>
        public LdapFilter And(LdapFilter ldapFilter, bool appendFilter)
        {
            return And(Check.NotNull(ldapFilter, nameof(ldapFilter)).ToString(), appendFilter);
        }

        /// <summary>
        /// Inverts the meaning of the LDAP filter.
        /// </summary>
        /// <returns>a filter that contains a Not LDAP filter.</returns>
        public LdapFilter Not()
        {
            if (_filter.Contains("("))
            {
                // This is a complex filter, enclose with parentheses
                return new LdapFilter($"(!({_filter}))");
            }

            // This is a simple filter, no need to use parentheses unless it is a bitwise filter
            // Bitwise filter: attributename:ruleOID:=value
            return _filter.Contains(":=")
                ? new LdapFilter($"(!({_filter}))")
                : new LdapFilter($"(!{_filter})");
        }

        /// <summary>
        /// Combine the existing LDAP filter with the specified LDAP filter to create a new Or LDAP Filter.
        /// </summary>
        /// <param name="attributeType">The attribute of the filter.</param>
        /// <param name="filterType">The type of the filter.</param>
        /// <param name="attributeValue">The value of the filter.</param>
        /// <returns>a filter that contains an Or LDAP Filter.</returns>
        public LdapFilter Or(string attributeType, string filterType, string attributeValue)
        {
            return Or(new LdapFilter(attributeType, filterType, attributeValue), false);
        }

        /// <summary>
        /// Combine the existing LDAP filter with the specified LDAP filter to create a new Or LDAP Filter.
        /// </summary>
        /// <param name="attributeType">The attribute of the filter.</param>
        /// <param name="filterType">The type of the filter.</param>
        /// <param name="attributeValue">The value of the filter.</param>
        /// <param name="appendFilter">if set to <c>true</c> append the filter.</param>
        /// <returns>a filter that contains an Or LDAP Filter.</returns>
        public LdapFilter Or(
            string attributeType,
            string filterType,
            string attributeValue,
            bool appendFilter)
        {
            return Or(new LdapFilter(attributeType, filterType, attributeValue), appendFilter);
        }

        /// <summary>
        /// Combine the existing LDAP filter with the specified LDAP filter to create a new Or LDAP Filter.
        /// </summary>
        /// <param name="ldapFilter">A LDAP Filter.</param>
        /// <returns>a filter that contains an Or LDAP Filter.</returns>
        public LdapFilter Or(string ldapFilter)
        {
            return Or(ldapFilter, false);
        }

        /// <summary>
        /// Combine the existing LDAP filter with the specified LDAP filter to create a new Or LDAP Filter.
        /// </summary>
        /// <param name="ldapFilter">A LDAP Filter.</param>
        /// <param name="appendFilter">if set to <c>true</c> append the filter.</param>
        /// <returns>a filter that contains an Or LDAP Filter.</returns>
        public LdapFilter Or(string ldapFilter, bool appendFilter)
        {
            Check.NotEmpty(ldapFilter, nameof(ldapFilter));

            return appendFilter
                ? new LdapFilter($"(|({_filter})({Strip(ldapFilter)}))")
                : new LdapFilter($"(|({Strip(ldapFilter)})({_filter}))");
        }

        /// <summary>
        /// Combine the existing LDAP filter with the specified LDAP filter to create a new Or LDAP Filter.
        /// </summary>
        /// <param name="ldapFilter">A LDAP Filter.</param>
        /// <returns>a filter that contains an Or LDAP Filter.</returns>
        public LdapFilter Or(LdapFilter ldapFilter)
        {
            return Or(Check.NotNull(ldapFilter, nameof(ldapFilter)).ToString(), false);
        }

        /// <summary>
        /// Combine the existing LDAP filter with the specified LDAP filter to create a new Or LDAP Filter.
        /// </summary>
        /// <param name="ldapFilter">A LDAP Filter.</param>
        /// <param name="appendFilter">if set to <c>true</c> append the filter.</param>
        /// <returns>a filter that contains an Or LDAP Filter.</returns>
        public LdapFilter Or(LdapFilter ldapFilter, bool appendFilter)
        {
            return Or(Check.NotNull(ldapFilter, nameof(ldapFilter)).ToString(), appendFilter);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="object"/>.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the current <see cref="object"/>.</returns>
        public override string ToString()
        {
            return $"({_filter})";
        }

        private static string Strip(string ldapFilter)
        {
            var returnFilter = ldapFilter;

            if (returnFilter.StartsWith("(", StringComparison.InvariantCulture))
            {
                returnFilter = returnFilter.Substring(1);
            }

            if (returnFilter.EndsWith(")", StringComparison.InvariantCulture))
            {
                returnFilter = returnFilter.Substring(0, returnFilter.Length - 1);
            }

            return returnFilter;
        }
    }
}
