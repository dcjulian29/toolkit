using System.Collections.Generic;

namespace ToolKit.DirectoryServices.UserSort
{
    /// <summary>
    ///   Defines a method to compare two user objects by Last Logon Timestamp.
    /// </summary>
    public class SortByLastLogonTimestamp : IComparer<IUser>
    {
        private readonly bool _ascending = true;

        /// <summary>
        ///   Initializes a new instance of the <see cref="SortByLastLogonTimestamp" /> class.
        /// </summary>
        /// <param name="ascending">if set to <c>true</c> User are sorted in ascending order.</param>
        public SortByLastLogonTimestamp(bool ascending)
        {
            _ascending = ascending;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="SortByLastLogonTimestamp" /> class.
        /// </summary>
        protected SortByLastLogonTimestamp()
        {
        }

        /// <summary>
        ///   Compares the specified Last Logon Timestamp.
        /// </summary>
        /// <param name="x">The first Last Logon Timestamp.</param>
        /// <param name="y">The second Last Logon Timestamp.</param>
        /// <returns>
        ///   A integer that indicates the relative values of <paramref name="x" /> and <paramref
        ///   name="y" />.
        /// </returns>
        public int Compare(IUser x, IUser y)
        {
            if (x.LastLogonTimestamp > y.LastLogonTimestamp)
            {
                return _ascending ? 1 : -1;
            }

            if (x.LastLogonTimestamp < y.LastLogonTimestamp)
            {
                return _ascending ? -1 : 1;
            }

            return 0;
        }
    }
}
