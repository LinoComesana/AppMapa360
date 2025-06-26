using UnityEngine;
using Firebase;
using Firebase.Extensions;

public class FirebaseInitializer : MonoBehaviour
{
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var status = task.Result;
            if (status == DependencyStatus.Available)
            {
                Debug.Log("✅ Firebase se ha inicializado correctamente.");
            }
            else
            {
                Debug.LogError("❌ Firebase no está disponible: " + status.ToString());
            }
        });
    }
}
