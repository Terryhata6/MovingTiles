namespace Core.Entities
{
    public class Skill
    {
        public bool OnCooldown = false;

        public void Execute()
        {
            OnCooldown = true;
        }
    }
}