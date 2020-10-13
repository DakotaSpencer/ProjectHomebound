using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
  //what level game is currently in
  //load and unload scenes
  //keep track of game states
  //generate peristance systems

  // Based on the code Akshat provided. Thanks dude :)
  public GameObject[] systemPrefabs;
  private List<GameObject> instancedSystemPrefabs;
  
  private string currentLevelName = string.Empty;
  List<AsyncOperation> loadOperation;
  
   private string activeScene;
   
   // Peter Says: Please... Unity... Love me :(
   private void Start ()
   {
      activeScene = SceneManager.GetActiveScene().name;
      DontDestroyOnLoad(gameObject);
      loadOperation = new List<AsyncOperation>();
      instancedSystemPrefabs = new List<GameObject>();
   }

   void OnloadAsyncOperationComplete(AsyncOperation ao)
   {
      if(loadOperation.Contains(ao))
      {
          loadOperation.Remove(ao);
      }
      Debug.Log("Load Complete");
   }

   void OnUnloadAsyncOperationComplete(AsyncOperation ao)
   { 
      Debug.Log("Unload Complete");
   }
   
   void InstantiateSystemPrefabs()
   {
      GameObject prefabInstance;

       foreach (GameObject i in systemPrefabs)
       {
          prefabInstance = Instantiate(i);
          instancedSystemPrefabs.Add(prefabInstance);
       }
   }
  
   public void LoadLevel(string LevelName)
   {
      currentLevelName = LevelName;
      AsyncOperation ao = SceneManager.LoadSceneAsync(LevelName,LoadSceneMode.Additive);
      if(ao == null)
      {
         Debug.LogError("[Game Manager]Enable to load level " + LevelName);
         return;
      }
      ao.completed +=  OnloadAsyncOperationComplete;
      loadOperation.Add(ao);
   }

   public void UnloadLevel(string LevelName)
   {
       SceneManager.UnloadSceneAsync(LevelName);
       AsyncOperation ao = SceneManager.LoadSceneAsync(LevelName);
       ao.completed +=  OnUnloadAsyncOperationComplete;
   }

    protected void OnDestroy()
    { 
        for (int i = 0; i > instancedSystemPrefabs.Count; i++)
        {
          Destroy(instancedSystemPrefabs[i]);
          instancedSystemPrefabs.Clear();
        }
    }
}
