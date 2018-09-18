// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class ObjectGameContenier : MonoBehaviour
// {
//     public GameObject Prefab;

//     public ObjectGame ObjectGame { private set; get; }

//     [SerializeField]
//     public ActionManager ActionManager;

//     public void SetPrefab(GameObject prefab)
//     {
//         Prefab = prefab;
//     }

//     public void CreateObjectGame()
//     {
//         DestroyObjectGame();
//         ObjectGame = Instantiate(Prefab, transform).GetComponent<ObjectGame>();
//     }

//     public void DestroyObjectGame()
//     {
//         if (ObjectGame != null)
//         {
//             ObjectGame.gameObject.SetActive(false);
//             Destroy(ObjectGame, 2);
//         }
//     }

// }
