using Common.Logging;
using NHibernate;

namespace ToolKit.Data.NHibernate
{
    /// <summary>
    /// Provides Testing Constraints to NHibernate session object.
    /// </summary>
    public class SessionConstraint
    {
        private static ILog _log = LogManager.GetLogger<SessionConstraint>();
        private readonly ISession _session;
        private bool _nextBooleanValue = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionConstraint"/> class.
        /// </summary>
        /// <param name="session">The NHibernate Session.</param>
        public SessionConstraint(ISession session)
        {
            this._session = session;
        }

        /// <summary>
        /// Gets the next boolean value and inverts it.
        /// </summary>
        public SessionConstraint Not
        {
            get
            {
                this._nextBooleanValue = !this._nextBooleanValue;

                return this;
            }
        }

        /// <summary>
        /// Determines whether this session is closed.
        /// </summary>
        /// <returns><c>true</c> if this session is closed; otherwise, <c>false</c>.</returns>
        public bool Closed()
        {
            var sessionIsClosed = !this._session.IsConnected && !this._session.IsOpen;

            return this._nextBooleanValue ? sessionIsClosed : !sessionIsClosed;
        }

        /// <summary>
        /// Determines whether the session is in an active transaction.
        /// </summary>
        /// <returns><c>true</c> if the session is in an active transaction; otherwise, <c>false</c>.</returns>
        public bool InTransaction()
        {
            var sessionInTransaction = this._session.Transaction != null && this._session.Transaction.IsActive;

            return this._nextBooleanValue ? sessionInTransaction : !sessionInTransaction;
        }

        /// <summary>
        /// Determines whether this session is null.
        /// </summary>
        /// <returns><c>true</c> if this session is null; otherwise, <c>false</c>.</returns>
        public bool Null()
        {
            var sessionIsNull = this._session == null;

            return this._nextBooleanValue ? sessionIsNull : !sessionIsNull;
        }

        /// <summary>
        /// Determines whether the session is null or closed.
        /// </summary>
        /// <returns><c>true</c> if the session is null or closed; otherwise, <c>false</c>.</returns>
        public bool NullOrClosed()
        {
            var sessionIsNullOrClosed = this.Null() || this.Closed();

            return this._nextBooleanValue ? sessionIsNullOrClosed : !sessionIsNullOrClosed;
        }
    }
}
