namespace TeamBuilder.Models
{
    /* 
        Added a Data Transfer Object (DTO)!     
        "A DTO may be used to:
        Prevent over-posting.
        Hide properties that clients are not supposed to view.
        Omit some properties in order to reduce payload size.
        Flatten object graphs that contain nested objects. Flattened object graphs can be more convenient for clients.".
        Ref: https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-8.0&tabs=visual-studio

        Will map to a code-first approach using Entity Framework SQL,  using DBContext, and POCO's, eventually. For now let's get it working!
        A DbSet represents the collection of all entities in the context, or that can be queried from the database, of a given type. 
        DbSet objects are created from a DbContext.

        "SRP: The Single Responsibility Principle
        THERE SHOULD NEVER BE MORE THAN ONE REASON FOR A CLASS TO CHANGE.".
        Ref SRP: https://web.archive.org/web/20150202200348/http://www.objectmentor.com/resources/articles/srp.pdf
     */
    public class TeamBuilderEventDto
    {
        public long Id { get; set; }
        public string EventName { get; set; } = "";
        public string EventDescription { get; set; } = "";
        public DateTime EventStart { get; set; }

        public DateTime EventEnd { get; set; }
        public bool EventRecurring { get; set; } = false;

        public double GetEventDurationInMinutes()
        {
            if (EventEnd > EventStart)
            {
                TimeSpan span = EventEnd.Subtract(EventStart);
                return span.TotalMinutes;
            }
            else
            {
                //Todo log an error because the UI or input should not allow such a scenario.
                return 0;
            }
        }

        public bool HasEventEnded()
        {
            DateTime dtEventEnd = EventStart.AddMinutes(GetEventDurationInMinutes());
            return EventEnd >= dtEventEnd;
        }

    }

    public class TeamBuilder : TeamBuilderEventDto
    {
        private string? Secret { get; set; }

        public TeamBuilder()
        {
        }

        public TeamBuilder(TeamBuilderEventDto teamBuilderEventDto)
        {
            _ = Util.Util.CopyProperties(teamBuilderEventDto, this, ["Secret"]);
        }

        public static TeamBuilderEventDto ObjectToDto(object obj)
        {
            TeamBuilderEventDto objDto = new();

            //Secret property has to be ignored here for the purpose of this DTO to work.
            _ = Util.Util.CopyProperties(obj, objDto, ["Secret"]);
            return objDto;
        }

    }
}
