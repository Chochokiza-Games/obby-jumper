using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public enum States
    {
        Idle,
        Running,
        Jumping,
        Attack,
        Fly
    }

    [SerializeField] private Animator _animator;

    private States _currentState = States.Idle;

    public void PlayState(States state)
    {
        if (_currentState != state)
        {
            _currentState = state;
            switch(state)
            {
                case States.Idle:
                    _animator.SetBool("Running", false);
                    _animator.SetBool("Jumping", false);
                    break;
                case States.Running:
                    _animator.SetBool("Running", true);
                    _animator.SetBool("Jumping", false);
                    break;
                case States.Jumping:
                    _animator.SetBool("Jumping", true);
                    break;
                case States.Attack:
                    _animator.SetTrigger("Attack");
                    break;
                case States.Fly:
                    _animator.SetTrigger("Fly");
                    break;
            }
        }
    }    
}
