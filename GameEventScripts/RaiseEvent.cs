using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RaiseEvent : MonoBehaviour
{
    public enum RaiseType {ON_TRIGGER_ENTER, ON_PICKUP};
    public RaiseType type;
    public bool randomFromList = false;
    public GameEvent eventToRaise = null;
    public List<GameEvent> possibleEventsToRaise = new List<GameEvent>();
    public string triggeringTag = "Player";

    // Implement "trigger only once"?
    public bool triggerOnce;

    #region INTERACTABLE SCRIPT CHECKS
    //// When dependent on items
    //private InteractableObject interactable;
    //bool lastHeldState = false; // Save the last state of the item, so we only raise the event once

    //private void Start()
    //{
    //    // Get the InteractableObject script if we don't have a reference of it yet
    //    if (interactable == null && type == RaiseType.ON_PICKUP)
    //    {
    //        interactable = GetComponentInParent<InteractableObject>();
    //        if (interactable != null)
    //        {
    //            lastHeldState = interactable.bHeld;
    //        }
    //        else
    //        {
    //            enabled = false;
    //            Debug.LogError("Couldn't find InteractableObject from GameObject " + gameObject.name);
    //        }
    //    }
    //}

    //void Update()
    //{

    //    // Check the type if it's one of the ones we need to check in Update
    //    // And proceed accordingly
    //    switch (type)
    //    {
    //        case RaiseType.ON_PICKUP:
    //            // Update when the item has been dropped again
    //            if(lastHeldState)
    //            {
    //                lastHeldState = interactable.bHeld;
    //            }

    //            // Check if the item has been picked up
    //            if(interactable.bHeld != lastHeldState && interactable.bHeld)
    //            {
    //                lastHeldState = interactable.bHeld;
    //                StartRaise();
    //            }
    //            break;
    //    }
    //}
    #endregion

    private void StartRaise()
    {
        if (!randomFromList)
        {
            eventToRaise.Raise();
        }
        else
        {
            if (possibleEventsToRaise.Count > 0)
            {
                int rand = Random.Range(0, possibleEventsToRaise.Count);
                possibleEventsToRaise[rand].Raise();
            }
            else
            {
                Debug.LogError("Tried to raise an event from GameObject '" + gameObject.name + "' but no events were in List of possible events.");
            }
        }

        if(triggerOnce)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Layersmasks are bit flag fields, so we turn gameObject.layer into a bit flag and compare that to the LayerMask
        if (other.gameObject.tag == triggeringTag && !other.isTrigger)
        {
            StartRaise();
        }
    }
}