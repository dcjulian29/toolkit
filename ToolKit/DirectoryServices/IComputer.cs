using System;

namespace ToolKit.DirectoryServices
{
    /// <summary>
    /// This class represents an Computer Interface.
    /// </summary>
    public interface IComputer
    {
        /// <summary>
        /// Gets when the account was created.
        /// </summary>
        /// <value>When the account was created.</value>
        DateTime Created { get; }

        /// <summary>
        /// Gets the description of the computer account.
        /// </summary>
        /// <value>The description of the computer account.</value>
        string Description { get; }

        /// <summary>
        /// Gets the distinguished name of the computer account.
        /// </summary>
        /// <value>The distinguished name of the computer account.</value>
        string DistinguishedName { get; }

        /// <summary>
        /// Gets the DNS address of the computer account.
        /// </summary>
        /// <value>The DNS address of the computer account.</value>
        string DnsHostName { get; }

        /// <summary>
        /// Gets the NetBIOS Domain name of the computer.
        /// </summary>
        /// <value>The domain.</value>
        string DomainName { get; }

        /// <summary>
        /// Gets the name of the computer account.
        /// </summary>
        /// <value>The name of the computer account.</value>
        string Name { get; }

        /// <summary>
        /// Gets the operating system of the computer.
        /// </summary>
        /// <value>The operating system of the computer.</value>
        string OperatingSystem { get; }

        /// <summary>
        /// Gets the operating system service pack of the computer.
        /// </summary>
        /// <value>The operating system service pack of the computer.</value>
        string OperatingSystemServicePack { get; }

        /// <summary>
        /// Gets the operating system version of the computer.
        /// </summary>
        /// <value>The operating system version of the computer.</value>
        string OperatingSystemVersion { get; }

        /// <summary>
        /// Gets the SAM Account name of the computer account.
        /// </summary>
        /// <value>The SAM Account name of the computer account.</value>
        string SamAccountName { get; }

        /// <summary>
        /// Gets the Security ID of the computer account.
        /// </summary>
        /// <value>The Security ID of the computer account.</value>
        string Sid { get; }

        /// <summary>
        /// Determines whether this computer is a domain controller.
        /// </summary>
        /// <returns><c>true</c> if this computer is a domain controller; otherwise, <c>false</c>.</returns>
        bool IsDomainController();

        /// <summary>
        /// Determines whether this computer is a server as determined by the OS.
        /// </summary>
        /// <returns><c>true</c> if this computer is a server; otherwise, <c>false</c>.</returns>
        bool IsServer();

        /// <summary>
        /// Determines whether this computer is a workstation as determined by the OS.
        /// </summary>
        /// <returns><c>true</c> if this computer is a workstation; otherwise, <c>false</c>.</returns>
        bool IsWorkstation();
    }
}
