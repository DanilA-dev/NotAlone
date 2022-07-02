using System;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerObjectInteraction : IState, IInteractor
{
    private const float ROTATION_SPEED = 700;

    private PlayerController _playerController;
    private Rigidbody _body;
    private Animator _playerAnimator;

    public Transform Interactor => _playerController.transform;

    public PlayerObjectInteraction(PlayerController playerController, Rigidbody body, Animator playerAnimator)
    {
        _playerController = playerController;
        _body = body;
        _playerAnimator = playerAnimator;
    }

    public void Enter()
    {
        _playerController.ToogleInput(false);
    }

    public void ExecuteFixedUpdate() { }
    public void ExecuteUpdate() { }

    public void Exit()
    {
        _playerController.ToogleInput(true);
    }

    public async void FocusToInteractable(Transform objectToFocus, Action onInteractionEnd)
    {
        var pickAbleItem = objectToFocus.GetComponent<PickableItem>();
        if (pickAbleItem)
        {
            Vector3 dirToItem = (objectToFocus.position - Interactor.position).normalized;
            for (float i = 0; i < 3; i+= Time.deltaTime)
            {
                Vector3 rotateDir = new Vector3(dirToItem.x, Interactor.position.y, dirToItem.z);
                RotateTowards(rotateDir);
                MoveToObject(dirToItem);

                if (Vector3.Distance(_playerController.transform.position, pickAbleItem.transform.position) < pickAbleItem.PickUpDistance)
                     break;

                await Task.Yield();
            }
            _playerAnimator.SetTrigger("Take");
            onInteractionEnd?.Invoke();
            pickAbleItem.DisableItemOnScene();
        }
    }

    private void RotateTowards(Vector3 dir)
    {
        Quaternion r = Quaternion.LookRotation(dir, Vector3.up);
        Interactor.rotation = Quaternion.RotateTowards(Interactor.rotation, r, ROTATION_SPEED * Time.deltaTime);
    }

    private void MoveToObject(Vector3 dir)
    {
        Vector3 target = _body.transform.InverseTransformDirection(_body.velocity);
        float velocityZ = target.z;
        float velocityX = target.x;
        _playerAnimator.SetFloat("InputY", velocityZ, 0.1f, Time.deltaTime);
        _playerAnimator.SetFloat("InputX", velocityX, 0.1f, Time.deltaTime);
        _body.AddForce(dir.normalized * _playerController.CurrentMoveType.Acceleration * Time.deltaTime);

        if (_body.velocity.magnitude > 0)
        {
            var oppositeDir = -_body.velocity * _playerController.CurrentMoveType.Friction;
            _body.AddForce(oppositeDir * Time.deltaTime);
        }
    }
}
