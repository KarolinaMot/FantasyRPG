using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public GameObject pickMsg;
    public float radius = 1.5f;               // How close do we need to be to interact?
    public Transform interactionTransform;  // The transform from where we interact in case you want to offset it
    public Transform player;       // Reference to the player transform
    public Item item;

    public virtual void PickUp()
    {

        pickMsg.SetActive(true);

        if (Input.GetKey(KeyCode.F))
        {
            bool wasPickedUp = Inventory.instance.Add(item);

            if (wasPickedUp)
            {
                Destroy(gameObject);
                pickMsg.SetActive(false);
            }
           
        }
    }

    void Update()
    {

        float distance = Vector3.Distance(player.position, interactionTransform.position);
        if (distance <= radius)
        {
                PickUp();
        }
        if (distance > radius)
        {
            pickMsg.SetActive(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
            interactionTransform = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }

}
