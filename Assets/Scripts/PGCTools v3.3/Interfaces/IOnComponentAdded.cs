using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PGCTools.Interfaces
{
    /// <summary>
    /// Inhetice from this when you want to initialized an new added component in edit mode.
    /// </summary>
    public interface IOnComponentAdded
    {
        /// <summary>
        /// Callded when this component is added to a game obejct.
        /// </summary>
        public void OnComponentAdded();
    }
}
