﻿using Microsoft.EntityFrameworkCore;

namespace TeamBuilder.Models
{
    public class TeamBuilderContext : DbContext
    {
        public TeamBuilderContext(DbContextOptions<TeamBuilderContext> options)
            : base(options)
        {
        }

        public DbSet<TeamBuilder> TeamBuilder { get; set; } = null!;
    }
}
