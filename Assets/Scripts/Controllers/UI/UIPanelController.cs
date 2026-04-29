using UnityEngine;

namespace Controllers.UI
{
    public class UIPanelController : MonoBehaviour
    {
        protected virtual void OnEnable() => SubscribeEvents();
        
        protected virtual void SubscribeEvents() { }
        
        protected virtual void UnsubscribeEvents() { }
        
        protected virtual void OnDisable() => UnsubscribeEvents();
    }
}