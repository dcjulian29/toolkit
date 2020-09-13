using System;
using System.Collections.Generic;

namespace ToolKit.DirectoryServices.UserSort
{
    /// <summary>
    ///   Defines a method to compare two user objects by Display Name.
    /// </summary>
    public class SortByDisplayName : IComparer<IUser>
    {
        private readonly bool _ascending = true;

        /// <summary>
        ///   Initializes a new instance of the <see cref="SortByDisplayName" /> class.
        /// </summary>
        /// <param name="ascending">if set to <c>true</c> User are sorted in ascending order.</param>
        public SortByDisplayName(bool ascending)
        {
            _ascending = ascending;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="SortByDisplayName" /> class.
        /// </summary>
        protected SortByDisplayName()
        {
        }

        /// <summary>
        ///   Compares the specified Display Name.
        /// </summary>
        /// <param name="x">The first Display Name.</param>
        /// <param name="y">The second Display Name.</param>
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

            var compare = String.CompareOrdinal(x.DisplayName, y.DisplayName);

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
