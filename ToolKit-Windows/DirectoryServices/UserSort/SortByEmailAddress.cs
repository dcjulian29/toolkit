using System;
using System.Collections.Generic;

namespace ToolKit.DirectoryServices.UserSort
{
    /// <summary>
    ///   Defines a method to compare two user objects by Email Address.
    /// </summary>
    public class SortByEmailAddress : IComparer<IUser>
    {
        private readonly bool _ascending = true;

        /// <summary>
        ///   Initializes a new instance of the <see cref="SortByEmailAddress" /> class.
        /// </summary>
        /// <param name="ascending">if set to <c>true</c> User are sorted in ascending order.</param>
        public SortByEmailAddress(bool ascending)
        {
            _ascending = ascending;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="SortByEmailAddress" /> class.
        /// </summary>
        protected SortByEmailAddress()
        {
        }

        /// <summary>
        ///   Compares the specified Email Address.
        /// </summary>
        /// <param name="x">The first Email Address.</param>
        /// <param name="y">The second Email Address.</param>
        /// <returns>
        ///   A integer that indicates the relative values of <paramref name="x" /> and <paramref
        ///   name="y" />.
        /// </returns>
        public int Compare(IUser x, IUser y)
        {
            var compare = String.CompareOrdinal(x.EmailAddress, y.EmailAddress);

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
