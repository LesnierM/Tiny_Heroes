using System;
using UnityEngine;

namespace PGCTools
{
    /// <summary>
    /// Creates a singleton object that derives from MonoBehaviour and is not destroyed.
    /// </summary>
    /// <typeparam name="T">The object.</typeparam>
    public class SingletonPersitent<T> : MonoBehaviour where T : Component
    {
        static T _instance;

        private void Awake()
        {
            if (_instance != null && _instance.gameObject != gameObject)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                _instance = this as T;
                DontDestroyOnLoad(_instance.gameObject);
            }

            AwakeFromSingleton();
        }
        /// <summary>
        /// Used to be able to get awake on derived classes.
        /// </summary>
        protected virtual void AwakeFromSingleton() { }
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance != null)
                        DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
    }

    /// <summary>
    /// Creates a singleston object from monobehaviour that is destroyed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonMono<T> : MonoBehaviour where T : Component
    {
        static T _instance;

        private void Awake()
        {
            if (_instance != null && _instance.gameObject != gameObject)
            {
                Destroy(gameObject);
                return;
            }
            else
                _instance = this as T;

            AwakeFromSingleton();
        }
        /// <summary>
        /// Used to be able to get awake on derived classes.
        /// </summary>
        protected virtual void AwakeFromSingleton() { }
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<T>();
                return _instance;
            }
        }
    }

#if FUSION_WEAVER
    /// <summary>
    /// Creates a singleton object that derives from NetworkBehaviour and is not destroyed.
    /// </summary>
    /// <typeparam name="T">The object.</typeparam>
    public class SingletonFusionNetPersistent<T> : Fusion.NetworkBehaviour where T : Component
    {
        static T _instance;

        private void Awake()
        {
            if (_instance != null && _instance.gameObject != gameObject)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                _instance = this as T;
                DontDestroyOnLoad(_instance.gameObject);
            }

            AwakeFromSingleton();
        }
        /// <summary>
        /// Used to be able to get awake on derived classes.
        /// </summary>
        protected virtual void AwakeFromSingleton() { }
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance != null)
                        DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
    }
    /// <summary>
    /// Creates a singleton object that derives from SimulationBehaviour.
    /// </summary>
    /// <typeparam name="T">The object.</typeparam>
    public class SingletonFusionSimPersistent<T> : Fusion.SimulationBehaviour where T : Component
    {
        static T _instance;

        private void Awake()
        {
            if (_instance != null && _instance.gameObject != gameObject)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                _instance = this as T;
                DontDestroyOnLoad(_instance.gameObject);
            }

            AwakeFromSingleton();
        }
        /// <summary>
        /// Used to be able to get awake on derived classes.
        /// </summary>
        protected virtual void AwakeFromSingleton() { }
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance != null)
                        DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
    }
    /// <summary>
    /// Creates a singleton object that derives from MonoBehaviour and is not destroyed.
    /// </summary>
    /// <typeparam name="T">The object.</typeparam>
    public class SingletonFusion<T> : NetworkBehaviour where T : Component
    {
        static T _instance;

        private void Awake()
        {
            if (_instance != null && _instance.gameObject != gameObject)
            {
                Destroy(gameObject);
                return;
            }
            else
                _instance = this as T;

            AwakeFromSingleton();
        }
        /// <summary>
        /// Used to be able to get awake on derived classes.
        /// </summary>
        protected virtual void AwakeFromSingleton() { }
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<T>();
                return _instance;
            }
        }
    }
#endif
}
