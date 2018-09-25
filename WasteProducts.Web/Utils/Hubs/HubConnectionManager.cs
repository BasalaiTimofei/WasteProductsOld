using System.Collections.Generic;
using System.Linq;

namespace WasteProducts.Web.Utils.Hubs
{
    /// <summary>
    /// Provides storage of connection identifiers by key
    /// </summary>
    /// <typeparam name="T">Type of key</typeparam>
    public class HubConnectionManager<TKey>
    {
        private readonly Dictionary<TKey, HashSet<string>> _connections = new Dictionary<TKey, HashSet<string>>();

        public int Count
        {
            get
            {
                lock (_connections)
                {
                    return _connections.Count;
                }
            }
        }


        /// <summary>
        /// Adds new connection id to connection list by key
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="connectionId">connection id</param>
        public void Add(TKey key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        /// <summary>
        /// Gets list of connection ids by key
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>enumerable of connection ids, if key doesn't exists returns empty enumerable</returns>
        public IEnumerable<string> GetConnections(TKey key)
        {
            HashSet<string> connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Removes connection id by key if it exists
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="connectionId">connection id</param>
        public void Remove(TKey key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }
    }
}