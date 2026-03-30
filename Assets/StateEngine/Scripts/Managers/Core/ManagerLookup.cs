using UnityEngine;

namespace StateEngine.Managers.Core {
    internal static class ManagerLookup {
        public static T FindSingletonInLoadedWorld<T>() where T : Object {
            T[] found = Object.FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            return found.Length > 0 ? found[0] : null;
        }
    }
}
