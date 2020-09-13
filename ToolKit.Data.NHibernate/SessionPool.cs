﻿using System;
using System.Collections.Generic;
using NHibernate;
using ToolKit.Validation;

namespace ToolKit.Data.NHibernate
{
    /// <summary>
    /// Session Pool caches/store session manager objects.
    /// </summary>
    public static class SessionPool
    {
        private static readonly Dictionary<string, SessionManager> pool
            = new Dictionary<string, SessionManager>();

        /// <summary>
        /// Gets the number of session manager object in this pool.
        /// </summary>
        public static int Count
        {
            get
            {
                return pool.Count;
            }
        }

        /// <summary>
        /// Adds the specified session manager with the key being the name.
        /// </summary>
        /// <param name="sessionManagerName">Name of the session manager.</param>
        /// <param name="sessionManager">The session manager object.</param>
        public static void Add(string sessionManagerName, SessionManager sessionManager)
        {
            sessionManager = Check.NotNull(sessionManager, nameof(sessionManager));

            if (pool.ContainsKey(sessionManagerName))
            {
                throw new ArgumentException("Session Name is already Used");
            }

            pool.Add(sessionManagerName, sessionManager);
        }

        /// <summary>
        /// Clear all of the session manager stored in the pool.
        /// </summary>
        public static void Clear()
        {
            pool.Each(s => s.Value.Dispose());

            pool.Clear();
        }

        /// <summary>
        /// Determines whether the pool contains the specified session manager.
        /// </summary>
        /// <param name="sessionManagerName">Name of the session manager.</param>
        /// <returns>
        ///   <c>true</c> if the pool contains the specified session manager; otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains(string sessionManagerName)
        {
            return pool.ContainsKey(sessionManagerName);
        }

        /// <summary>
        /// Returns the specified session manager.
        /// </summary>
        /// <param name="sessionManagerName">Name of the session manager.</param>
        /// <returns>The specified session manager.</returns>
        public static SessionManager Manager(string sessionManagerName)
        {
            if (!pool.ContainsKey(sessionManagerName))
            {
                throw new ArgumentException("Session Manager does not exist in the pool!");
            }

            return pool[sessionManagerName];
        }

        /// <summary>
        /// Removes the specified session manager.
        /// </summary>
        /// <param name="sessionManagerName">Name of the session manager.</param>
        public static void Remove(string sessionManagerName)
        {
            sessionManagerName = Check.NotEmpty(sessionManagerName, nameof(sessionManagerName));

            if (pool.ContainsKey(sessionManagerName))
            {
                pool[sessionManagerName].Dispose();
                pool.Remove(sessionManagerName);
            }
        }

        /// <summary>
        /// Returns the session of the specified session manager.
        /// </summary>
        /// <param name="sessionManagerName">Name of the session manager.</param>
        /// <returns>The session of the specified session manager.</returns>
        public static ISession Session(string sessionManagerName)
        {
            if (!pool.ContainsKey(sessionManagerName))
            {
                throw new ArgumentException("Session Manager does not exist in the pool!");
            }

            return pool[sessionManagerName].Session;
        }
    }
}
