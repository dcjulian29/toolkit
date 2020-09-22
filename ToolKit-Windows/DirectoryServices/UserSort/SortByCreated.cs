using System.Collections.Generic;

namespace ToolKit.DirectoryServices.UserSort
{
    /// <summary>
    ///   Defines a method to compare two user objects by Created.
    /// </summary>
    public class SortByCreated : IComparer<IUser>
    {
        private readonly bool _ascending = true;

        /// <summary>
        ///   Initializes a new instance of the <see cref="SortByCreated" /> class.
        /// </summary>
        /// <param name="ascending">if set to <c>true</c> User are sorted in ascending order.</param>
        public SortByCreated(bool ascending)
        {
            _ascending = ascending;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="SortByCreated" /> class.
        /// </summary>
        protected SortByCreated()
        {
        }

        /// <summary>
        ///   Compares the specified Created date.
        /// </summary>
        /// <param name="x">The first Created date.</param>
        /// <param name="y">The second Created date.</param>
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

            if (x.Created > y.Created)
            {
                return _ascending ? 1 : -1;
            }

            if (x.Created < y.Created)
            {
                return _ascending ? -1 : 1;
            }

            return 0;
        }
    }
}
