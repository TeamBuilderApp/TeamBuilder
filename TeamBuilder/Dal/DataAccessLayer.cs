namespace TeamBuilder.Dal
{

    public abstract class TeamBuilderAPI
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

    }
    public static class DAL
    {
        static DAL()
        {
            //If there's a need for a Dal!
        }
    }
}
