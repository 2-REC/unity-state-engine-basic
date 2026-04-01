using UnityEngine;

namespace StateEngine.Managers.Shared {
    public abstract class IDataManager : MonoBehaviour {
        //protected bool loaded = false;

        // TODO: protected virtual?
        protected virtual void Awake() {
            //LoadData();
            Load();
        }

        public void Load() {
            /*
            if (!loaded) {
                LoadData();
                loaded = true;
            }
            */
            LoadData();
        }

        public virtual void Leave() {
            CommitChanges();
            //loaded = false;
        }

        public abstract void CommitChanges();
        protected abstract void LoadData();
    }
}
