// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.Events;
// using System.Collections.Generic;

// public class BuyNewItemDetector : MonoBehaviour
// {
//     Items<GameObject>.Item Previous;

//     /// <summary>
//     /// Start is called on the frame when a script is enabled just before
//     /// any of the Update methods is called the first time.
//     /// </summary>
//     void Start()
//     {
//         Previous = GetItem();
//         WorldEther.CoinsChange.Subscribe(Handler);
//     }

//     public void PushMessage()
//     {
//         if (Previous != null)
//             MessageManager.Manager.Show("Можно купить новый предмет\nНажмите чтобы просмотреть", 3, delegate { });
//     }

//     private void Handler(Ethers.Channel.Info info)
//     {
//         DeferredAction.Manager.AddDeferredAction(UpdateItems, DeferredAction.Manager.GetUpdateInterval());
//     }

//     public void UpdateItems()
//     {
//         var buff = GetItem();
//         if (Previous != buff && buff != null)
//         {
//             Previous = buff;
//             PushMessage();
//         }
//     }

//     private Items<GameObject>.Item GetItem()
//     {
//         return ItemsBase.Base.GetNotHaveAndBuy(Coins.Manager.CoinsCount);
//     }
// }