using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class SimpleHingeInteractable : XRSimpleInteractable
{
    [SerializeField] Vector3 positionLimits;
    private Transform grabHand;
    private Collider hingeCollider;
    private Vector3 hingePositions;
    private const string Default_Layer = "Default";
    private const string Grab_Layer = "Grab";
    [SerializeField] bool isLocked;

    protected virtual void Start()
    {
        hingeCollider = GetComponent<Collider>();
    }

    public void UnlockHinge()
    {
        isLocked = false;
    }
    public void LockHinge()
    {
        isLocked = true;
    }

    protected virtual void Update()
    {
        if (grabHand != null)
        {
            TrackHand();
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!isLocked)
        {
            base.OnSelectEntered(args);
            grabHand = args.interactorObject.transform;
        }
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        grabHand = null;
        ChangeLayerMask(Grab_Layer);
    }

    private void TrackHand()
    {
        transform.LookAt(grabHand, transform.forward);
        hingePositions = hingeCollider.bounds.center;
        if (grabHand.position.x >= hingePositions.x + positionLimits.x ||
        grabHand.position.x<=hingePositions.x-positionLimits.x)
        {
            ReleaseHinge();
        }
        else if (grabHand.position.y >= hingePositions.y + positionLimits.y ||
        grabHand.position.y<=hingePositions.y-positionLimits.y)
        {
            ReleaseHinge();
        }
        else if (grabHand.position.z >= hingePositions.z + positionLimits.z ||
        grabHand.position.z <= hingePositions.z - positionLimits.z)
        {
            ReleaseHinge();
        }
    }

    public void ReleaseHinge()
    {
        ChangeLayerMask(Default_Layer);
    }
    
    private void ChangeLayerMask(string mask)
    {
        interactionLayers = InteractionLayerMask.GetMask(mask);
    }
}
