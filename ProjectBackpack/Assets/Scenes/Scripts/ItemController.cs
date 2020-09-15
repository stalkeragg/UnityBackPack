using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemController : MonoBehaviour
{
    private Camera playerCamera;

    private Rigidbody rigidBody ;

    private PlayerController playerController;

    private Transform itemAttachedTransform;

    private bool itemIsAttached;

    [SerializeField]
    public float atachPositionLerpSpeed = 0.05f;

    [SerializeField]
    public float atachRotationSlerpSpeed = 0.07f;

    private Vector3 mOffset;

    private float mZCoord;

    [HideInInspector]
    public event Action<GameObject> OnItemDragDetach;

    void Start()
    {
        playerCamera = Camera.main;
        playerController = (PlayerController)Camera.main.GetComponent("PlayerController");
        rigidBody = (Rigidbody)gameObject.GetComponent("Rigidbody");
        itemIsAttached = false;
    }

    private Vector3 GetMouseWorldPossition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPossition();

        playerController.SetPickedItem(gameObject);

        rigidBody.freezeRotation = true;
        rigidBody.Sleep();
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPossition() + mOffset;
        if (itemIsAttached)
        {
            itemIsAttached = false;
            rigidBody.constraints = RigidbodyConstraints.None;

            if (OnItemDragDetach != null) OnItemDragDetach(gameObject);
        }
    }

    public void AttachVisualItem(Transform mewItemTransform)
    {
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;

        itemAttachedTransform = mewItemTransform;
        itemIsAttached = true;
    }

    public void AttachItemHiden()
    {
        gameObject.SetActive(false);
    }

    public void DetachItem()
    {

    }

    public void RenderItemAttachAnimation()
    {
        transform.position = Vector3.Lerp(transform.position, itemAttachedTransform.position, atachPositionLerpSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, itemAttachedTransform.rotation, atachRotationSlerpSpeed);
    }

    void Update()
    {
        // smooth transform if visual attach
        if (itemIsAttached)
        {
            RenderItemAttachAnimation();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray mouseVector = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitObject;

            if (Physics.Raycast(mouseVector, out hitObject, 25, LayerMask.GetMask("Item")))
            {
                if (itemIsAttached && hitObject.collider.name == gameObject.name && playerController.BackPackIsPiked)
                {
                    itemIsAttached = false;
                    rigidBody.constraints = RigidbodyConstraints.None;

                    if (OnItemDragDetach != null) OnItemDragDetach(gameObject);
                }
            }
        }
    }
}
