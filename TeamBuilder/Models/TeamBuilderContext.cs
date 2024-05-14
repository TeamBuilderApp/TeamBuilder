using Microsoft.EntityFrameworkCore;

namespace TeamBuilder.Models
{
    /*
        Todo - Design a way for locally stored rosters AKA where a roster comes from does not need to be known. Form a common roster language for any Team Builder to POST.
        Ideas JSON or YAML or even XML.
        This Team Builder can utilize an Entity Framework POCO database.
        As any Team Builder app posts to our Team Builder API using the common roster language, a bulk operations Entity can certainly be copied...
        on all rosters being POSTED by any app that calls this API.

        Feature 1) Common roster language to POST.
        Feature 2) A local storage for rosters that connect to this API to POST.
        Feature 3) A central or local Entity Framework to store rosters per app. Could even be a local text file per app...
        As long as this API is connected to, it will accept POST using common roster language.
        Feature 4) An external centralized Entity Framework Service that uses Bulk Operations async to read in all POSTS from any app connected to this API...
        Which can be the center of operations.
    */
    public partial class TeamBuilderContext : DbContext
    {
        public TeamBuilderContext(DbContextOptions<TeamBuilderContext> options)
            : base(options)
        {
        }

        public DbSet<TeamBuilder> TeamBuilder { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamBuilder>(entity =>
            {
                entity.HasKey(k => k.Id);
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
