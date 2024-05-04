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
     */

    /*
        Will map to Entity SQL eventually using DBContext and POCO, for now let's get it working!
        A DbSet represents the collection of all entities in the context, or that can be queried from the database, of a given type. 
        DbSet objects are created from a DbContext using the DbContext.
     */
    public class TeamBuilderEventDto
    {
        public long Id { get; set; }
        public string EventName { get; set; } = "";
        public DateTime? EventDateTime { get; set; }
        public bool IsEventOpen { get; set; }

        public bool IsEventRecurring { get; set; }

    }

    public class TeamBuilderEvent : TeamBuilderEventDto
    {
        private string? Secret { get; set; }

        public TeamBuilderEvent(TeamBuilderEventDto TeamBuilderEventDto)
        {
            _ = Util.Util.CopyProperties(TeamBuilderEventDto, this);
        }

        public static TeamBuilderEventDto TeamBuilderEventToDto(TeamBuilderEvent teamBuilderEvent)
        {
            TeamBuilderEventDto teamBuilderEventDto = new();

            //Secret property has to be ignored here for the purpose of this DTO to work.
            _ = Util.Util.CopyProperties(teamBuilderEvent, teamBuilderEventDto, ["Secret"]);
            return teamBuilderEventDto;
        }
    }
}
