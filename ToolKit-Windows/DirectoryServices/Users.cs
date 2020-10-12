using System;
using System.Collections.Generic;
using ToolKit.DirectoryServices.ActiveDirectory;
using ToolKit.DirectoryServices.ServiceInterfaces;

namespace ToolKit.DirectoryServices
{
    /// <summary>
    ///   This Interface is an alias for a generic list of User objects.
    /// </summary>
    public partial class Users : List<IUser>
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="Users" /> class.
        /// </summary>
        /// <param name="capacity">Integer representing the.</param>
        public Users(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Users" /> class.
        /// </summary>
        /// <param name="collection">The collection of users.</param>
        public Users(IEnumerable<IUser> collection)
            : base(collection)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Users" /> class.
        /// </summary>
        public Users()
        {
        }

        /// <summary>
        ///   Disable All Users within this collection.
        /// </summary>
        public void DisableAll()
        {
            ForEach(user =>
            {
                var userFlags = (ADS_USER_FLAG)user.UserAccountControl;

                if (!userFlags.Contains(ADS_USER_FLAG.ACCOUNTDISABLE))
                {
                    userFlags.SetFlag(ADS_USER_FLAG.ACCOUNTDISABLE);

                    var entry = (user as User)?.ToDirectoryEntry();

                    if (entry != null)
                    {
                        entry.Properties["userAccessControl"].Value = userFlags;
                        entry.CommitChanges();
                        entry.Close();
                    }
                    else
                    {
                        throw new InvalidCastException($"Unable to cast {user} to a directory entry");
                    }
                }
            });
        }

        /// <summary>
        ///   Enable All Users within this collection.
        /// </summary>
        public void EnableAll()
        {
            ForEach(user =>
            {
                var userFlags = (ADS_USER_FLAG)user.UserAccountControl;

                if (userFlags.Contains(ADS_USER_FLAG.ACCOUNTDISABLE))
                {
                    userFlags.ClearFlag(ADS_USER_FLAG.ACCOUNTDISABLE);

                    var entry = (user as User)?.ToDirectoryEntry();

                    if (entry != null)
                    {
                        entry.Properties["userAccessControl"].Value = userFlags;
                        entry.CommitChanges();
                        entry.Close();
                    }
                    else
                    {
                        throw new InvalidCastException($"Unable to cast {user} to a directory entry");
                    }
                }
            });
        }
    }
}
