using System;
using System.DirectoryServices;
using Common.Logging;

namespace ToolKit.DirectoryServices
{
    /// <summary>
    /// Performs queries against Lightweight Directory Access Protocol (LDAP) Services.
    /// </summary>
    public class LdapQuery : DisposableObject
    {
        private DirectoryEntry _directoryEntry;
        private DirectorySearcher _directorySearcher;
        private string _ldapFilter;
        private int _pageSize = 500;

        /// <summary>
        /// Initializes a new instance of the <see cref="LdapQuery"/> class.
        /// </summary>
        /// <param name="path">The LDAP path of the root of the query.</param>
        public LdapQuery(string path)
            : this(path, String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LdapQuery"/> class.
        /// </summary>
        /// <param name="path">The LDAP path of the root of the query.</param>
        /// <param name="filter">The search filter string in LDAP format.</param>
        public LdapQuery(string path, string filter)
        {
            Path = path;
            Filter = filter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LdapQuery"/> class.
        /// </summary>
        /// <param name="searchRoot">
        /// The node in the LDAP Services hierarchy where the query search will start.
        /// </param>
        public LdapQuery(DirectoryEntry searchRoot)
            : this(searchRoot, String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LdapQuery"/> class.
        /// </summary>
        /// <param name="searchRoot">
        /// The node in the LDAP Services hierarchy where the query search will start.
        /// </param>
        /// <param name="filter">The search filter string in LDAP format.</param>
        public LdapQuery(DirectoryEntry searchRoot, string filter)
        {
            _directoryEntry = searchRoot;
            Path = _directoryEntry.Path;
            Filter = filter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LdapQuery"/> class.
        /// </summary>
        /// <param name="dn">The DistinguishedName of the root.</param>
        /// <param name="globalCatalog">if set to <c>true</c>, use the global catalog.</param>
        public LdapQuery(DistinguishedName dn, bool globalCatalog)
            : this(dn, String.Empty, globalCatalog)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LdapQuery"/> class.
        /// </summary>
        /// <param name="dn">The DistinguishedName of the root.</param>
        /// <param name="filter">The search filter string in LDAP format.</param>
        /// <param name="globalCatalog">if set to <c>true</c>, use the global catalog.</param>
        public LdapQuery(DistinguishedName dn, string filter, bool globalCatalog)
            : this(globalCatalog ? dn?.GcPath : dn?.LdapPath, filter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LdapQuery"/> class.
        /// </summary>
        /// <param name="dn">The DistinguishedName of the root.</param>
        /// <param name="filter">The search filter string in LDAP format.</param>
        /// <param name="globalCatalog">if set to <c>true</c>, use the global catalog.</param>
        public LdapQuery(DistinguishedName dn, LdapFilter filter, bool globalCatalog)
            : this(dn, filter?.ToString(), globalCatalog)
        {
        }

        /// <summary>
        /// Gets or sets a value indicating the Lightweight Directory Access Protocol (LDAP) format
        /// filter string.
        /// </summary>
        /// <value>
        /// The search filter string in LDAP format, such as "(objectClass=user)". The default is
        /// "(objectClass=*)", which retrieves all objects.
        /// </value>
        public string Filter
        {
            get
            {
                return String.IsNullOrEmpty(_ldapFilter) ? "(objectClass=*)" : _ldapFilter;
            }

            set
            {
                _ldapFilter = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the page size in a paged search.
        /// </summary>
        /// <value>
        /// The maximum number of objects the server can return in a paged search. The default is
        /// 500, which means do a paged search returning 500 records at a time.
        /// </value>
        /// <exception cref="System.ArgumentException">The new value is less than zero.</exception>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("The value cannot be less than zero!");
                }

                _pageSize = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the node in the Lightweight Directory Access Protocol
        /// (LDAP) hierarchy where the query search starts.
        /// </summary>
        /// <value>The path of the LDAP query.</value>
        public string Path { get; set; }

        /// <summary>
        /// Executes the search and returns a collection of the results that are found.
        /// </summary>
        /// <returns>A SearchResultCollection object that contains the results of the search.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// The specified LDAP path is not a container.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Searching is not supported by the provider that is being used.
        /// </exception>
        public SearchResultCollection FindAll()
        {
            PrepareInternalObjects();

            return _directorySearcher.FindAll();
        }

        /// <summary>
        /// Executes the search and returns only the first result that is found.
        /// </summary>
        /// <returns>
        /// A SearchResult object that contains the first result that is found during the search.
        /// </returns>
        public SearchResult FindOne()
        {
            PrepareInternalObjects();

            return _directorySearcher.FindOne();
        }

        /// <summary>
        /// Releases the resources (other than memory) that are used by the
        /// System.DirectoryServices.DirectorySearcher object, the
        /// System.DirectoryServices.DirectoryEntry, and any unmanaged resources used by the
        /// internal objects of this class.
        /// </summary>
        /// <param name="disposing">
        /// if set to <c>false</c>, this method has been called by the runtime.
        /// </param>
        protected override void DisposeResources(bool disposing)
        {
            if (_directorySearcher != null)
            {
                _directorySearcher.Dispose();
                _directorySearcher = null;
            }

            if (_directoryEntry != null)
            {
                _directoryEntry.Close();
                _directoryEntry.Dispose();
                _directoryEntry = null;
            }
        }

        private void PrepareInternalObjects()
        {
            if (String.IsNullOrEmpty(Path))
            {
                throw new InvalidOperationException("You must specify a LDAP path!");
            }

            if (_directoryEntry == null)
            {
                _directoryEntry = new DirectoryEntry(Path);
            }

            _directorySearcher = new DirectorySearcher(_directoryEntry)
            {
                Filter = Filter,
                PageSize = PageSize
            };
        }
    }
}
