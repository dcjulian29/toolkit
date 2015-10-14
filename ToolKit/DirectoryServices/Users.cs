using System;
using System.Collections.Generic;
using Common.Logging;
using ToolKit.DirectoryServices.ServiceInterfaces;

namespace ToolKit.DirectoryServices
{
    /// <summary>
    /// This Interface is an alias for a generic list of User objects
    /// </summary>
    public class Users : List<IUser>
    {
        private static ILog _log = LogManager.GetLogger<Users>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Users"/> class.
        /// </summary>
        /// <param name="capacity">Integer representing the</param>
        public Users(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Users"/> class.
        /// </summary>
        /// <param name="collection">The collection of users.</param>
        public Users(IEnumerable<IUser> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Users"/> class.
        /// </summary>
        public Users()
        {
        }

        /// <summary>
        /// Disable All Users within this collection
        /// </summary>
        public void DisableAll()
        {
            ForEach(user =>
            {
                var userFlags = (ADS_USER_FLAG)user.UserAccountControl;

                if (!userFlags.Contains(ADS_USER_FLAG.ACCOUNTDISABLE))
                {
                    userFlags.SetFlag(ADS_USER_FLAG.ACCOUNTDISABLE);

                    var entry = user.ToDirectoryEntry();
                    entry.Properties["userAccessControl"].Value = userFlags;
                    entry.CommitChanges();
                    entry.Close();
                }
            });
        }

        /// <summary>
        /// Enable All Users within this collection
        /// </summary>
        public void EnableAll()
        {
            ForEach(user =>
            {
                var userFlags = (ADS_USER_FLAG)user.UserAccountControl;

                if (userFlags.Contains(ADS_USER_FLAG.ACCOUNTDISABLE))
                {
                    userFlags.ClearFlag(ADS_USER_FLAG.ACCOUNTDISABLE);

                    var entry = user.ToDirectoryEntry();
                    entry.Properties["userAccessControl"].Value = userFlags;
                    entry.CommitChanges();
                    entry.Close();
                }
            });
        }

        /// <summary>
        /// Defines a method to compare two user objects by Changed.
        /// </summary>
        public class SortByChanged : IComparer<IUser>
        {
            private bool _ascending = true;

            /// <summary>
            /// Initializes a new instance of the <see cref="SortByChanged"/> class.
            /// </summary>
            /// <param name="ascending">if set to <c>true</c> User are sorted in ascending order.</param>
            public SortByChanged(bool ascending)
            {
                _ascending = ascending;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SortByChanged"/> class.
            /// </summary>
            protected SortByChanged()
            {
            }

            /// <summary>
            /// Compares the specified Changed date.
            /// </summary>
            /// <param name="x">The first Changed date.</param>
            /// <param name="y">The second Changed date.</param>
            /// <returns>
            /// A integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>.
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

        /// <summary>
        /// Defines a method to compare two user objects by Created.
        /// </summary>
        public class SortByCreated : IComparer<IUser>
        {
            private bool _ascending = true;

            /// <summary>
            /// Initializes a new instance of the <see cref="SortByCreated"/> class.
            /// </summary>
            /// <param name="ascending">if set to <c>true</c> User are sorted in ascending order.</param>
            public SortByCreated(bool ascending)
            {
                _ascending = ascending;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SortByCreated"/> class.
            /// </summary>
            protected SortByCreated()
            {
            }

            /// <summary>
            /// Compares the specified Created date.
            /// </summary>
            /// <param name="x">The first Created date.</param>
            /// <param name="y">The second Created date.</param>
            /// <returns>
            /// A integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>.
            /// </returns>
            public int Compare(IUser x, IUser y)
            {
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

        /// <summary>
        /// Defines a method to compare two user objects by Display Name.
        /// </summary>
        public class SortByDisplayName : IComparer<IUser>
        {
            private bool _ascending = true;

            /// <summary>
            /// Initializes a new instance of the <see cref="SortByDisplayName"/> class.
            /// </summary>
            /// <param name="ascending">if set to <c>true</c> User are sorted in ascending order.</param>
            public SortByDisplayName(bool ascending)
            {
                _ascending = ascending;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SortByDisplayName"/> class.
            /// </summary>
            protected SortByDisplayName()
            {
            }

            /// <summary>
            /// Compares the specified Display Name.
            /// </summary>
            /// <param name="x">The first Display Name.</param>
            /// <param name="y">The second Display Name.</param>
            /// <returns>
            /// A integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>.
            /// </returns>
            public int Compare(IUser x, IUser y)
            {
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

        /// <summary>
        /// Defines a method to compare two user objects by Email Address.
        /// </summary>
        public class SortByEmailAddress : IComparer<IUser>
        {
            private bool _ascending = true;

            /// <summary>
            /// Initializes a new instance of the <see cref="SortByEmailAddress"/> class.
            /// </summary>
            /// <param name="ascending">if set to <c>true</c> User are sorted in ascending order.</param>
            public SortByEmailAddress(bool ascending)
            {
                _ascending = ascending;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SortByEmailAddress"/> class.
            /// </summary>
            protected SortByEmailAddress()
            {
            }

            /// <summary>
            /// Compares the specified Email Address.
            /// </summary>
            /// <param name="x">The first Email Address.</param>
            /// <param name="y">The second Email Address.</param>
            /// <returns>
            /// A integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>.
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

        /// <summary>
        /// Defines a method to compare two user objects by Last Logon Timestamp.
        /// </summary>
        public class SortByLastLogonTimestamp : IComparer<IUser>
        {
            private bool _ascending = true;

            /// <summary>
            /// Initializes a new instance of the <see cref="SortByLastLogonTimestamp"/> class.
            /// </summary>
            /// <param name="ascending">if set to <c>true</c> User are sorted in ascending order.</param>
            public SortByLastLogonTimestamp(bool ascending)
            {
                _ascending = ascending;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SortByLastLogonTimestamp"/> class.
            /// </summary>
            protected SortByLastLogonTimestamp()
            {
            }

            /// <summary>
            /// Compares the specified Last Logon Timestamp.
            /// </summary>
            /// <param name="x">The first Last Logon Timestamp.</param>
            /// <param name="y">The second Last Logon Timestamp.</param>
            /// <returns>
            /// A integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>.
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

        /// <summary>
        /// Defines a method to compare two user objects by Password Last Set.
        /// </summary>
        public class SortByPasswordLastSet : IComparer<IUser>
        {
            private bool _ascending = true;

            /// <summary>
            /// Initializes a new instance of the <see cref="SortByPasswordLastSet"/> class.
            /// </summary>
            /// <param name="ascending">if set to <c>true</c> User are sorted in ascending order.</param>
            public SortByPasswordLastSet(bool ascending)
            {
                _ascending = ascending;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SortByPasswordLastSet"/> class.
            /// </summary>
            protected SortByPasswordLastSet()
            {
            }

            /// <summary>
            /// Compares the specified Password Last Set.
            /// </summary>
            /// <param name="x">The first Password Last Set.</param>
            /// <param name="y">The second Password Last Set.</param>
            /// <returns>
            /// A integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>.
            /// </returns>
            public int Compare(IUser x, IUser y)
            {
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

        /// <summary>
        /// Defines a method that a type implements to compare two user objects by SAM Account Name.
        /// </summary>
        public class SortBySamAccountName : IComparer<IUser>
        {
            private bool _ascending = true;

            /// <summary>
            /// Initializes a new instance of the <see cref="SortBySamAccountName"/> class.
            /// </summary>
            /// <param name="ascending">if set to <c>true</c> User are sorted in ascending order.</param>
            public SortBySamAccountName(bool ascending)
            {
                _ascending = ascending;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SortBySamAccountName"/> class.
            /// </summary>
            protected SortBySamAccountName()
            {
            }

            /// <summary>
            /// Compares the specified SAM Account Name.
            /// </summary>
            /// <param name="x">The first SAM Account Name.</param>
            /// <param name="y">The second SAM Account Name.</param>
            /// <returns>
            /// A integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>.
            /// </returns>
            public int Compare(IUser x, IUser y)
            {
                var compare = String.CompareOrdinal(x.SamAccountName, y.SamAccountName);

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

        /// <summary>
        /// Defines a method that a type implements to compare two user objects by User Principal Name.
        /// </summary>
        public class SortByUserPrincipalName : IComparer<IUser>
        {
            private bool _ascending = true;

            /// <summary>
            /// Initializes a new instance of the <see cref="SortByUserPrincipalName"/> class.
            /// </summary>
            /// <param name="ascending">if set to <c>true</c> User are sorted in ascending order.</param>
            public SortByUserPrincipalName(bool ascending)
            {
                _ascending = ascending;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SortByUserPrincipalName"/> class.
            /// </summary>
            protected SortByUserPrincipalName()
            {
            }

            /// <summary>
            /// Compares the specified User Principal Names.
            /// </summary>
            /// <param name="x">The first User Principal Name.</param>
            /// <param name="y">The second User Principal Name.</param>
            /// <returns>
            /// A integer that indicates the relative values of <paramref name="x"/> and <paramref name="y"/>.
            /// </returns>
            public int Compare(IUser x, IUser y)
            {
                var compare = String.CompareOrdinal(x.UserPrincipalName, y.UserPrincipalName);

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
}
