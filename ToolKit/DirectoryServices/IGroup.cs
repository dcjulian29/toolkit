using System;
using System.Collections.Generic;

namespace ToolKit.DirectoryServices
{
    /// <summary>
    /// This interface represents a generic group
    /// </summary>
    public interface IGroup
    {
        /// <summary>
        /// Gets the category of the group.
        /// </summary>
        /// <value>The category of the group.</value>
        string Category { get; }

        /// <summary>
        /// Gets the date and time that group was last changed.
        /// </summary>
        /// <value>The date and time that the group was last changed.</value>
        DateTime Changed { get; }

        /// <summary>
        /// Gets the CN of the group.
        /// </summary>
        /// <value>The common name of the group</value>
        string Cn { get; }

        /// <summary>
        /// Gets when the account was created.
        /// </summary>
        /// <value>When the account was created.</value>
        DateTime Created { get; }

        /// <summary>
        /// Gets the description of the group.
        /// </summary>
        /// <value>The description of the group.</value>
        string Description { get; }

        /// <summary>
        /// Gets the the distinguished name of the group.
        /// </summary>
        /// <value>The distinguished name of the group.</value>
        string DistinguishedName { get; }

        /// <summary>
        /// Gets the email address of the group.
        /// </summary>
        /// <value>The email address of the group.</value>
        string EmailAddress { get; }

        /// <summary>
        /// Gets the GUID of the group object.
        /// </summary>
        /// <value>The GUID of the group object.</value>
        Guid Guid { get; }

        /// <summary>
        /// Gets the DistinguishedName for the group's manager.
        /// </summary>
        /// <value>The DistinguishedName for the group's manager.</value>
        string ManagedBy { get; }

        /// <summary>
        /// Gets the distinguished name list of groups this group is a member of.
        /// </summary>
        /// <value>The distinguished name list of the groups</value>
        List<string> MemberOf { get; }

        /// <summary>
        /// Gets the distinguished name list of members.
        /// </summary>
        /// <value>The distinguished name list of members.</value>
        List<string> Members { get; }

        /// <summary>
        /// Gets the modified date and time for the group.
        /// </summary>
        /// <value>The modified date and time for the group.</value>
        DateTime Modified { get; }

        /// <summary>
        /// Gets the name of the group.
        /// </summary>
        /// <value>The name of the group.</value>
        string Name { get; }

        /// <summary>
        /// Gets the notes of the group.
        /// </summary>
        /// <value>The notes of the group.</value>
        string Notes { get; }

        /// <summary>
        /// Gets the name of the sam account name of the group
        /// </summary>
        /// <value>The sam account name of the group.</value>
        string SamAccountName { get; }

        /// <summary>
        /// Gets the Security ID of the group.
        /// </summary>
        /// <value>The Security ID of the group.</value>
        string Sid { get; }

        /// <summary>
        /// Gets the update sequence number (USN) of the group when it was created.
        /// </summary>
        /// <value>The update sequence number (USN) of the group when it was created.</value>
        long UpdateSequenceNumberCreated { get; }

        /// <summary>
        /// Gets the current update sequence number (USN) of the group.
        /// </summary>
        /// <value>The current update sequence number (USN) of the group.</value>
        long UpdateSequenceNumberCurrent { get; }

        /// <summary>
        /// Determines whether this group instance is a distribution list.
        /// </summary>
        /// <returns><c>true</c> if this group instance is a distribution list; otherwise, <c>false</c>.</returns>
        bool IsDistributionList();

        /// <summary>
        /// Determines whether this group instance is a domain local group.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this group instance is a domain local group; otherwise, <c>false</c>.
        /// </returns>
        bool IsDomainLocal();

        /// <summary>
        /// Determines whether this group instance is global.
        /// </summary>
        /// <returns><c>true</c> if this group instance is global; otherwise, <c>false</c>.</returns>
        bool IsGlobal();

        /// <summary>
        /// Determines whether this group instance is security enabled.
        /// </summary>
        /// <returns><c>true</c> if this group instance is security enabled; otherwise, <c>false</c>.</returns>
        bool IsSecurity();

        /// <summary>
        /// Determines whether this group instance is a universal group.
        /// </summary>
        /// <returns><c>true</c> if this group instance is a universal group; otherwise, <c>false</c>.</returns>
        bool IsUniversal();
    }
}
