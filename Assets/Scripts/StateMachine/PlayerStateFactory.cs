namespace CharacterControllers
{
    public class PlayerStateFactory
    {
        private readonly PlayerStateMachine _context;
        
        public PlayerStateFactory(PlayerStateMachine context)
        {
            _context = context;
        }

        public BaseState Idle()
        {
            return new PlayerIdleState(_context, this);
        }

        public BaseState Grounded()
        {
            return new PlayerGroundedState(_context, this);
        }

        public BaseState Walk()
        {
            return new PlayerWalkState(_context, this);
        }

        public BaseState Run()
        {
            return new PlayerRunState(_context, this);
        }

        public BaseState Jump()
        {
            return new PlayerJumpState(_context, this);
        }
    }
}
