namespace CharacterControllers
{
    public class PlayerIdleState : BaseState
    {
        private PlayerStateMachine _context;
        private PlayerStateFactory _factory;
        
        public PlayerIdleState(PlayerStateMachine context, PlayerStateFactory playerStateFactory)
        {
            _context = context;
            _factory = playerStateFactory;
        }
        
        public override void EnterState()
        {
            // noop
        }

        public override void UpdateState()
        {
            // noop
        }

        public override void ExitState()
        {
            // noop
        }

        public override void CheckSwitchStates()
        {
            // noop
        }

        public override void InitializeSubState()
        {
            // noop
        }
    }
    
    public class PlayerGroundedState : BaseState
    {
        private PlayerStateMachine _context;
        private PlayerStateFactory _factory;
        
        public PlayerGroundedState(PlayerStateMachine context, PlayerStateFactory playerStateFactory)
        {
            _context = context;
            _factory = playerStateFactory;
        }

        public override void EnterState()
        {
            // noop
        }

        public override void UpdateState()
        {
            // noop
        }

        public override void ExitState()
        {
            // noop
        }

        public override void CheckSwitchStates()
        {
            // noop
        }

        public override void InitializeSubState()
        {
            // noop
        }
    }
    
    public class PlayerJumpState : BaseState
    {
        private PlayerStateMachine _context;
        private PlayerStateFactory _factory;
        
        public PlayerJumpState(PlayerStateMachine context, PlayerStateFactory playerStateFactory)
        {
            _context = context;
            _factory = playerStateFactory;
        }

        public override void EnterState()
        {
            // noop
        }

        public override void UpdateState()
        {
            // noop
        }

        public override void ExitState()
        {
            // noop
        }

        public override void CheckSwitchStates()
        {
            // noop
        }

        public override void InitializeSubState()
        {
            // noop
        }
    }
    
    public class PlayerWalkState : BaseState
    {
        private PlayerStateMachine _context;
        private PlayerStateFactory _factory;

        public PlayerWalkState(PlayerStateMachine context, PlayerStateFactory playerStateFactory)
        {
            _context = context;
            _factory = playerStateFactory;
        }

        public override void EnterState()
        {
            // noop
        }

        public override void UpdateState()
        {
            // noop
        }

        public override void ExitState()
        {
            // noop
        }

        public override void CheckSwitchStates()
        {
            // noop
        }

        public override void InitializeSubState()
        {
            // noop
        }
    }
    
    public class PlayerRunState : BaseState
    {
        private PlayerStateMachine _context;
        private PlayerStateFactory _factory;

        public PlayerRunState(PlayerStateMachine context, PlayerStateFactory playerStateFactory)
        {
            _context = context;
            _factory = playerStateFactory;
        }

        public override void EnterState()
        {
            // noop
        }

        public override void UpdateState()
        {
            // noop
        }

        public override void ExitState()
        {
            // noop
        }

        public override void CheckSwitchStates()
        {
            // noop
        }

        public override void InitializeSubState()
        {
            // noop
        }
    }
}
