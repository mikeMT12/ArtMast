using UnityEngine;

namespace Mixaill3d.Lamps.Scripts.Core
{
    public class MonoBehaviourSingletone<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject lampsControllerGO = new GameObject("LampsController");
                    DontDestroyOnLoad(lampsControllerGO);
                    _instance = lampsControllerGO.AddComponent<T>();
                }

                return _instance;
            }
            set => _instance = value;
        }
    }
}
