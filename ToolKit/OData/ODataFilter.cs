using System;
using System.Globalization;
using System.Linq;
using ToolKit.Validation;

namespace ToolKit.OData
{
    /// <summary>
    /// OData supports various kinds of query options for querying data. There are several kinds of
    /// basic predicates and built-in functions for a filter, including logical operators and
    /// arithmetic operators. This class will help you go through the common scenarios for these
    /// query options.
    /// </summary>
    public class ODataFilter
    {
        private string _filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataFilter" /> class.
        /// </summary>
        public ODataFilter() => _filter = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataFilter" /> class.
        /// </summary>
        /// <param name="filter">The OData filter string.</param>
        public ODataFilter(string filter) => _filter = filter;

        /// <summary>
        /// Add to this filter an Logical And expression.
        /// </summary>
        /// <param name="filter">An OData filter.</param>
        /// <returns>This OData filter.</returns>
        public ODataFilter And(ODataFilter filter)
        {
            Check.NotNull(filter, nameof(filter));

            _filter = $"({_filter}) and ({filter})";

            return this;
        }

        /// <summary>
        /// Add to this filter an expression where the value is equal the two field's content.
        /// </summary>
        /// <param name="field1">The name of the first property.</param>
        /// <param name="field2">The name of the second property.</param>
        /// <param name="value">The value.</param>
        /// <returns>This OData filter.</returns>
        public ODataFilter Concat(string field1, string field2, string value)
            => AppendFilter($"concat({field1}, {field2}) eq '{value}'");

        /// <summary>
        /// Add to this filter an expression where the value is contained in the field's content.
        /// </summary>
        /// <param name="field">The name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>This OData filter.</returns>
        public ODataFilter Contains(string field, string value)
            => AppendFilter($"contains({field}, '{value}')");

        /// <summary>
        /// Add to this filter an expression where the value ends the field's content.
        /// </summary>
        /// <param name="field">The name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>This OData filter.</returns>
        public ODataFilter EndsWith(string field, string value)
            => AppendFilter($"endswith({field}, '{value}')");

        /// <summary>
        /// Add to this filter an expression where the field is equal to the value.
        /// </summary>
        /// <param name="field">The name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>This OData filter.</returns>
        public ODataFilter EqualTo(string field, int value)
            => AddCommonExpression(field, "eq", value.ToString(CultureInfo.InvariantCulture));

        /// <summary>
        /// Add to this filter an expression where the field is equal to the value.
        /// </summary>
        /// <param name="field">The name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>This OData filter.</returns>
        public ODataFilter EqualTo(string field, string value)
            => AddCommonExpression(field, "eq", value);

        /// <summary>
        /// Add to this filter an expression where the field is greater than the value.
        /// </summary>
        /// <param name="field">The name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>This OData filter.</returns>
        public ODataFilter GreaterThan(string field, int value)
            => AddCommonExpression(field, "gt", value.ToString(CultureInfo.InvariantCulture));

        /// <summary>
        /// Add to this filter an expression where the field is greater than or equal to the value.
        /// </summary>
        /// <param name="field">The name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>This OData filter.</returns>
        public ODataFilter GreaterThanOrEqual(string field, int value)
            => AddCommonExpression(field, "ge", value.ToString(CultureInfo.InvariantCulture));

        /// <summary>
        /// Add to this filter an expression where the value starts the field's content.
        /// </summary>
        /// <param name="field">The name of the property.</param>
        /// <param name="value">The value.</param>
        /// <param name="index">The index of the value in the contents of the field.</param>
        /// <returns>This OData filter.</returns>
        public ODataFilter IndexOf(string field, string value, int index)
            => AppendFilter($"indexof({field}, '{value}') eq {index}");

        /// <summary>
        /// Add to this filter an expression where the field is less than the value.
        /// </summary>
        /// <param name="field">The name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>This OData filter.</returns>
        public ODataFilter LessThan(string field, int value)
            => AddCommonExpression(field, "lt", value.ToString(CultureInfo.InvariantCulture));

        /// <summary>
        /// Add to this filter an expression where the field is less than or equal to the value.
        /// </summary>
        /// <param name="field">The name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>This OData filter.</returns>
        public ODataFilter LessThanOrEqual(string field, int value)
            => AddCommonExpression(field, "le", value.ToString(CultureInfo.InvariantCulture));

        /// <summary>
        /// Negates this filter.
        /// </summary>
        /// <returns>This OData filter.</returns>
        public ODataFilter Not()
        {
            _filter = $"not ({_filter})";

            return this;
        }

        /// <summary>
        /// Add to this filter an expression where the field is not equal to the value.
        /// </summary>
        /// <param name="field">The name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>This OData filter.</returns>
        public ODataFilter NotEqualTo(string field, int value)
            => AddCommonExpression(field, "ne", value.ToString(CultureInfo.InvariantCulture));

        /// <summary>
        /// Add to this filter an expression where the field is not equal to the value.
        /// </summary>
        /// <param name="field">The name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>This OData filter.</returns>
        public ODataFilter NotEqualTo(string field, string value)
            => AddCommonExpression(field, "ne", value);

        /// <summary>
        /// Add to this filter an Logical Or expression.
        /// </summary>
        /// <param name="filter">An OData filter.</param>
        /// <returns>This OData filter.</returns>
        public ODataFilter Or(ODataFilter filter)
        {
            Check.NotNull(filter, nameof(filter));

            _filter = $"({_filter}) or ({filter})";

            return this;
        }

        /// <summary>
        /// Add to this filter an expression where the value is equal the field's content as lower case.
        /// </summary>
        /// <param name="field">The name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>This OData filter.</returns>
        public ODataFilter StartsWith(string field, string value)
            => AppendFilter($"startswith({field}, '{value}')");

        /// <summary>
        /// Add to this filter an expression where the value is a substring in the field's content.
        /// </summary>
        /// <param name="field">The name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>This OData filter.</returns>
        public ODataFilter Substring(string field, string value)
            => AppendFilter($"substringof('{value}', {field}) eq true");

        /// <summary>
        /// Add to this filter an expression where the value is equal the field's content as lower case.
        /// </summary>
        /// <param name="field">The name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>This OData filter.</returns>
        public ODataFilter ToLower(string field, string value)
            => AppendFilter($"tolower({field}) eq '{value?.ToLower(CultureInfo.CurrentCulture)}'");

        /// <summary>
        /// Returns a string that represents the filter.
        /// </summary>
        /// <returns>A string that represents the filter.</returns>
        public override string ToString() => _filter;

        /// <summary>
        /// Add to this filter an expression where the value is equal the field's content as upper case.
        /// </summary>
        /// <param name="field">The name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>This OData filter.</returns>
        public ODataFilter ToUpper(string field, string value)
            => AppendFilter($"toupper({field}) eq '{value?.ToUpper(CultureInfo.CurrentCulture)}'");

        /// <summary>
        /// Add to this filter an expression where the value is equal the field's content trimmed.
        /// </summary>
        /// <param name="field">The name of the property.</param>
        /// <param name="value">The value.</param>
        /// <returns>This OData filter.</returns>
        public ODataFilter Trim(string field, string value)
            => AppendFilter($"trim({field}) eq '{value}'");

        private ODataFilter AddCommonExpression(string field, string operation, string value)
        {
            if (!value.All(char.IsNumber))
            {
                value = $"'{value}'";
            }

            return AppendFilter($"{field} {operation} {value}");
        }

        private ODataFilter AppendFilter(string filter)
        {
            if (string.IsNullOrWhiteSpace(_filter))
            {
                _filter = filter;
            }
            else
            {
                _filter = $"({_filter}) and ({filter})";
            }

            return this;
        }
    }
}
