﻿namespace NecManager.Server.DataAccessLayer.EntityLayer;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using NecManager.Server.DataAccessLayer.Model;

/// <summary>
///     Represents the Nec Database.
/// </summary>
public class NecLiteDbContext : IdentityDbContext<Student>
{
    public NecLiteDbContext(DbContextOptions<NecLiteDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    ///     Gets the student set.
    /// </summary>
    public DbSet<Student> Students => this.Set<Student>();

    /// <summary>
    ///     Gets the group set.
    /// </summary>
    public DbSet<Group> Groups => this.Set<Group>();

    /// <summary>
    ///     Gets the lesson set.
    /// </summary>
    public DbSet<Lesson> Lessons => this.Set<Lesson>();

    /// <summary>
    ///     Gets the training set.
    /// </summary>
    public DbSet<Training> Trainings => this.Set<Training>();

    /// <summary>
    ///     Gets the person training set.
    /// </summary>
    public DbSet<PersonTraining> PersonTrainings => this.Set<PersonTraining>();

    /// <summary>
    ///     Gets the student group set.
    /// </summary>
    public DbSet<StudentGroup> StudentGroups => this.Set<StudentGroup>();
}
