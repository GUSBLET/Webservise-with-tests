namespace DataAccessLayer.EntityFramework;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Answer> Answers { get; set; }
    public DbSet<ImprovingData> ImprovingDatas { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<ProfileTest> ProfileTest { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Result> Results { get; set; }
    public DbSet<ResultTest> ResultTest { get; set; }
    public DbSet<Test> Tests { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProfileImprovingData>(buildAction =>
        {
            buildAction
                .ToTable("ProfileImprovingDataTable");

            buildAction
                .HasKey(pi => new { pi.ProfileId, pi.ImprovingDataId });

            buildAction
                .HasOne(pi => pi.Profile)
                .WithMany(p => p.ProfileImprovingDatas)
                .HasForeignKey(pi => pi.ProfileId)
                .OnDelete(DeleteBehavior.NoAction);

            buildAction
                .HasOne(pi => pi.ImprovingData)
                .WithMany(i => i.ProfileImprovingDatas)
                .HasForeignKey(pi => pi.ImprovingDataId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProfileTest>(buildAction =>
        {
            buildAction
                .ToTable("ProfileTestTable");

            buildAction
                .HasKey(x => new { x.ProfileId, x.TestId });

            buildAction
                .HasOne(x => x.Profile)
                .WithMany(x => x.ProfileTests)
                .HasForeignKey(x => x.ProfileId)
                .OnDelete(DeleteBehavior.NoAction);

            buildAction
                .HasOne(x => x.Test)
                .WithMany(xs => xs.ProfileTests)
                .HasForeignKey(x => x.TestId)
                .OnDelete(DeleteBehavior.Cascade);

            buildAction
                .Property(x => x.TestResult)
                .HasColumnName("TestResult")
                .HasColumnType("VARCHAR(50)")
                .IsRequired();
        });

        modelBuilder.Entity<ResultTest>(buildAction =>
        {
            buildAction
                .HasKey(x => new { x.ResultId, x.TestId });

            buildAction
                .HasOne(x => x.Test)
                .WithMany(x => x.ResultTests)
                .HasForeignKey(x => x.TestId);

            buildAction
                .HasOne(x => x.Result)
                .WithMany(x => x.ResultTests)
                .HasForeignKey(x => x.ResultId);

            buildAction
                .Property(x => x.MinimumOfDiapason)
                .HasColumnName("MinimumOfDiapason")
                .HasColumnType("SMALLINT");

            buildAction
                .Property(x => x.MaximumOfDiapason)
                .HasColumnName("MaximumOfDiapason")
                .HasColumnType("SMALLINT");
        });

        modelBuilder.Entity<Result>(buildAction =>
        {
            buildAction
                .ToTable("ResultTable");

            buildAction
                .HasKey(x => x.Id)
                .HasName("ResultId");

            buildAction
                .Property(x => x.Name)
                .HasColumnName("Name")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();
        });

        modelBuilder.Entity<Answer>(buildAction =>
        {
            buildAction
                .ToTable("AnswerTable");

            buildAction
                .HasKey(x => x.Id)
                .HasName("AnswerId");

            buildAction
                .Property(x => x.Name)
                .HasColumnName("Name")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            buildAction
                .Property(x => x.PowerOfAnswer)
                .HasColumnName("PowerOfAnswer")
                .HasColumnType("BIT");
        });

        modelBuilder.Entity<ImprovingData>(buildAction =>
        {
            buildAction
                .ToTable("ImprovingDataTable");

            buildAction
                .HasKey(x => x.Id)
                .HasName("ImprovingDataId");

            buildAction
                .Property(x => x.Title)
                .HasColumnName("Title")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            buildAction
                .Property(x => x.Description)
                .HasColumnName("Description")
                .HasColumnType("TEXT")
                .IsRequired(false);

            buildAction
                .Property(x => x.Information)
                .HasColumnName("Information")
                .HasColumnType("TEXT")
                .IsRequired();
        });

        modelBuilder.Entity<Question>(buildAction =>
        {
            buildAction
                .ToTable("QuestionTable");

            buildAction
                .HasKey(x => x.QuestionId)
                .HasName("QuestionId");

            buildAction
                .Property(x => x.QuestionText)
                .HasColumnName("QuestionText")
                .HasColumnType("VARCHAR(200)")
                .IsRequired();

            buildAction
                .HasMany(x => x.Answers)
                .WithMany(x => x.Questions)
                .UsingEntity(x => x.ToTable("QuestionAnswer"));

            buildAction
                .HasOne(x => x.Test)
                .WithMany(x => x.Questions);
        });

        modelBuilder.Entity<Test>(buildAction =>
        {
            buildAction
                .ToTable("TestTable");

            buildAction
                .HasKey(x => x.Id)
                .HasName("TestId");

            buildAction
                .Property(x => x.Name)
                .HasColumnName("Name")
                .HasColumnType("VARCHAR(50)")
                .IsRequired();

            buildAction
                .Property(x => x.Description)
                .HasColumnName("Description")
                .HasColumnType("TEXT")
                .IsRequired(false);

            buildAction
                .Property(x => x.TimePassing)
                .HasConversion<TimeOnlyConverter, TimeOnlyComparer>()
                .HasColumnName("TimePassing");

            buildAction
                .HasOne(x => x.Author)
                .WithMany(x => x.Tests);

            buildAction
                .HasMany(x => x.Questions)
                .WithOne(x => x.Test);
        });

        modelBuilder.Entity<Profile>(buildAction =>
        {
            buildAction
                .ToTable("ProfileTable");

            buildAction
                .HasKey(x => x.Id)
                .HasName("ProfileId");

            buildAction
                .Property(x => x.FullName)
                .HasColumnName("FullName")
                .HasColumnType("VARCHAR(50)")
                .IsRequired();

            buildAction
                .Property(x => x.Avatar)
                .HasColumnName("Avatar")
                .HasColumnType("VARBINARY(MAX)");

            buildAction
                .Property(x => x.Year)
                .HasColumnName("Year")
                .HasConversion<DateOnlyConverter, DateOnlyComparer>();
        });

        modelBuilder.Entity<User>(buildAction =>
        {
            buildAction
                .ToTable("UserTable");

            buildAction
                .HasKey(x => x.Id)
                .HasName("UserId");

            buildAction
                .Property(x => x.Email)
                .HasColumnName("Email")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            buildAction
                .Property(x => x.Password)
                .HasColumnName("Password")
                .HasColumnType("VARCHAR(MAX)")
                .IsRequired();

            buildAction
                .Property(x => x.Role)
                .HasColumnName("Role")
                .HasColumnType("VARCHAR(50)")
                .IsRequired();

            buildAction
                .Property(x => x.EmailConfirmedToken)
                .HasColumnName("EmailConfirmedToken")
                .HasColumnType("VARCHAR(50)")
                .IsRequired();

            buildAction
                .Property(x => x.EmailConfirmed)
                .HasColumnName("EmailConfirmed")
                .HasColumnType("BIT")
                .IsRequired();

            buildAction
                .HasMany(x => x.Tests)
                .WithOne(x => x.Author);

            buildAction
                .HasOne(x => x.Profile)
                .WithOne(x => x.User)
                .HasForeignKey<User>(x => x.ProfileId);

            buildAction
                .HasMany(x => x.ImprovingDatas)
                .WithOne(x => x.Author);
        });
    }
}
