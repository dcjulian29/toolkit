using NHibernate;

namespace ToolKit.Data.NHibernate
{
    /// <summary>
    /// Provides Extensions to NHibernate Session.
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// Extends a NHibernate Session to enable testing constraints to be added to the session instance.
        /// </summary>
        /// <param name="session">An NHibernate session.</param>
        /// <returns>A new Session Constraint instance.</returns>
        public static SessionConstraints Is(this ISession session)
        {
            return new SessionConstraints(session);
        }
    }
}
