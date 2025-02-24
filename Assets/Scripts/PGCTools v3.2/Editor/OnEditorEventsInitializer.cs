using PGCTools.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PGCTools
{
    [InitializeOnLoad]
    public class OnEditorEventsInitializer
    {
        static OnEditorEventsInitializer()
        {
            ObjectFactory.componentWasAdded -= componentWasAdded;
            ObjectFactory.componentWasAdded += componentWasAdded;
            EditorApplication.quitting -= OnEnterPlayMode;
            EditorApplication.quitting += OnEnterPlayMode;
        }
        /// <summary>
        /// Unsubscribe to callbacks.
        /// </summary>
        private static void OnEnterPlayMode()
        {
            ObjectFactory.componentWasAdded -= componentWasAdded;
            EditorApplication.quitting -= OnEnterPlayMode;
        }
        /// <summary>
        /// Called when a component is added in edit mode.
        /// </summary>
        /// <param name="obj"></param>
        private static void componentWasAdded(Component obj)
        {
            if (obj is IOnComponentAdded component)
            {
                component.OnComponentAdded();
            }
        }
    }
}