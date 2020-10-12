using System;
using System.Collections.Generic;

namespace ToolKit.DirectoryServices.UserSort
{
    /// <summary>
    ///   Defines a method that a type implements to compare two user objects by User Principal Name.
    /// </summary>
    public class SortByUserPrincipalName : IComparer<IUser>
    {
        private readonly bool _ascending = true;

        /// <summary>
        ///   Initializes a new instance of the <see cref="SortByUserPrincipalName" /> class.
        /// </summary>
        /// <param name="ascending">if set to <c>true</c> User are sorted in ascending order.</param>
        public SortByUserPrincipalName(bool ascending)
        {
            _ascending = ascending;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="SortByUserPrincipalName" /> class.
        /// </summary>
        protected SortByUserPrincipalName()
        {
        }

        /// <summary>
        ///   Compares the specified User Principal Names.
        /// </summary>
        /// <param name="x">The first User Principal Name.</param>
        /// <param name="y">The second User Principal Name.</param>
        /// <returns>
        ///   A integer that indicates the relative values of <paramref name="x" /> and <paramref
        ///   name="y" />.
        /// </returns>
        public int Compare(IUser x, IUser y)
        {
            if ((x == null) || (y == null))
            {
                return 0;
            }

            var compare = string.CompareOrdinal(x.UserPrincipalName, y.UserPrincipalName);

            switch (compare)
            {
                case 1:
                    return _ascending ? 1 : -1;

                case -1:
                    return _ascending ? -1 : 1;
            }

            return 0;
        }
    }
}
