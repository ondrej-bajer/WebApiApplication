namespace WebApiApplication.Configuration
{
    public sealed class DemoUsersOptions
    {
        public const string SectionName = "DemoUsers";

        public List<DemoUser> Users { get; init; } = new();
    }

    public sealed class DemoUser
    {
        public string Username { get; init; } = default!;
        public string Password { get; init; } = default!;
        public string Role { get; init; } = "User";
    }
}
