using UnityEngine;

namespace Project.Scripts.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public Vector2 Move => move;
        public bool Jump => jump;
        public bool JumpHold => jumpHold;
        public bool Climb => climb;
        public bool Interact => interact;
        
        private Vector2 move;
        private bool jump;
        private bool jumpHold;
        private bool climb;
        private bool interact;

        private void Update()
        {
            move.x = Input.GetAxisRaw("Horizontal");
            move.y = Input.GetAxisRaw("Vertical");

            jumpHold = Input.GetKey(KeyCode.Space);
            jump = Input.GetKeyUp(KeyCode.Space);

            climb = Input.GetKey(KeyCode.Y);

            interact = Input.GetKeyDown(KeyCode.G);
        }
    }
}