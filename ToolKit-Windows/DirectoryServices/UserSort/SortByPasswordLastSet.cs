using System.Collections.Generic;

namespace ToolKit.DirectoryServices.UserSort
{
    /// <summary>
    ///   Defines a method to compare two user objects by Password Last Set.
    /// </summary>
    public class SortByPasswordLastSet : IComparer<IUser>
    {
        private readonly bool _ascending = true;

        /// <summary>
        ///   Initializes a new instance of the <see cref="SortByPasswordLastSet" /> class.
        /// </summary>
        /// <param name="ascending">if set to <c>true</c> User are sorted in ascending order.</param>
        public SortByPasswordLastSet(bool ascending)
        {
            _ascending = ascending;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="SortByPasswordLastSet" /> class.
        /// </summary>
        protected SortByPasswordLastSet()
        {
        }

        /// <summary>
        ///   Compares the specified Password Last Set.
        /// </summary>
        /// <param name="x">The first Password Last Set.</param>
        /// <param name="y">The second Password Last Set.</param>
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

            if (x.PasswordLastSet > y.PasswordLastSet)
            {
                return _ascending ? 1 : -1;
            }

            if (x.PasswordLastSet < y.PasswordLastSet)
            {
                return _ascending ? -1 : 1;
            }

            return 0;
        }
    }
}
