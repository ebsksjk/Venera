using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venera.Shell
{
    /// <summary>
    /// These enviroments should always exist in <see cref="Kernel.GlobalEnvironment"/> and are
    /// considered default.
    /// </summary>
    public class DefaultEnvironments
    {
        public static readonly string CurrentWorkingDirectory = "CWD";
    }

    /// <summary>
    /// The environment allows applications to share arbitrary values with each other. In
    /// theory, multiple environments can exist concurrently but it's recommended to use
    /// the global one, see <see cref="Kernel.GlobalEnvironment"/>.
    /// </summary>
    public class Environment<T>
    {
        private Dictionary<T, List<T>> env;

        public Environment()
        {
            env = new Dictionary<T, List<T>>();
        }

        /// <summary>
        /// Add a new item to an enviroment variable. This will be accessible by other applications
        /// sharing this environment.
        /// </summary>
        public void Add(T key, T value)
        {
            if (env.ContainsKey(key))
            {
                env[key].Add(value);
            }
            else
            {
                env.Add(key, new List<T>());
            }
        }

        /// <summary>
        /// Override any existing environment variable with a new value, deleting the previous state.
        /// </summary>
        public void Set(T key, T value)
        {
            if (env.ContainsKey(key))
            {
                env.Remove(key);
            }

            List<T> list = new List<T>
            {
                value
            };

            env.Add(key, list);
        }

        /// <summary>
        /// Override any existing environment variable with a new list, deleting the previous state.
        /// </summary>
        public void Set(T key, List<T> valueList)
        {
            if (env.ContainsKey(key))
            {
                env.Remove(key);
            }

            env.Add(key, valueList);
        }

        /// <summary>
        /// Remove any existing environment variable.
        /// </summary>
        public void Remove(T key)
        {
            if (env.ContainsKey(key))
            {
                env.Remove(key);
            }
        }

        /// <summary>
        /// Get all values stored in an environment variable.
        /// </summary>
        public List<T> Get(T key)
        {
            if (env.TryGetValue(key, out List<T> list))
            {
                return list;
            }

            return default;
        }

        /// <summary>
        /// Sometimes you only need to store one value in a key. In these cases, this method is
        /// useful because it will only return the first item in the list.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetFirst(T key)
        {
            if (env.TryGetValue(key, out List<T> list))
            {
                return list.FirstOrDefault();
            }

            return default;
        }
    }
}