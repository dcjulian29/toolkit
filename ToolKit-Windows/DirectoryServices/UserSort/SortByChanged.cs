using System.Collections.Generic;

namespace ToolKit.DirectoryServices.UserSort
{
    /// <summary>
    ///   Defines a method to compare two user objects by Changed.
    /// </summary>
    public class SortByChanged : IComparer<IUser>
    {
        private readonly bool _ascending = true;

        /// <summary>
        ///   Initializes a new instance of the <see cref="SortByChanged" /> class.
        /// </summary>
        /// <param name="ascending">if set to <c>true</c> User are sorted in ascending order.</param>
        public SortByChanged(bool ascending)
        {
            _ascending = ascending;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="SortByChanged" /> class.
        /// </summary>
        protected SortByChanged()
        {
        }

        /// <summary>
        ///   Compares the specified Changed date.
        /// </summary>
        /// <param name="x">The first Changed date.</param>
        /// <param name="y">The second Changed date.</param>
        /// <returns>
        ///   A integer that indicates the relative values of <paramref name="x" /> and <paramref
        ///   name="y" />.
        /// </returns>
        public int Compare(IUser x, IUser y)
        {
            if (x.Changed > y.Changed)
            {
                return _ascending ? 1 : -1;
            }

            if (x.Changed < y.Changed)
            {
                return _ascending ? -1 : 1;
            }

            return 0;
        }
    }
}
