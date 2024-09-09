namespace CharacterControllers
{
    public abstract class BaseState
    {
        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
        public abstract void CheckSwitchStates();
        public abstract void InitializeSubState();
        
        void UpdateStates(){}

        void SwitchState(BaseState newState)
        {
            ExitState();
            newState.EnterState();
        }
        
        void SetSuperState(){}
        void SetSubState(){}
    }
}
