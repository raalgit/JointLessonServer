using FluentMigrator;

namespace JL_Migrator.Migrations
{
    [Migration(202207172101)]
    public class InitTestUser : Migration
    {
        public override void Down()
        {

        }

        public override void Up()
        {
            Insert.IntoTable("User").InSchema("JL")
                .Row(new
                {
                    FirstName = "Иван",
                    SecondName = "Иванов",
                    ThirdName = "Иванович",
                    AvatarId = (int?)null
                });

            Insert.IntoTable("AuthData").InSchema("JL")
                .Row(new
                {
                    UserId = 1,
                    Login = "test",
                    PasswordHash = "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08"
                });

            Insert.IntoTable("UserRole").InSchema("JL")
                .Row(new
                {
                    RoleId = 1,
                    UserId = 1
                })
                .Row(new
                {
                    RoleId = 2,
                    UserId = 1
                })
                .Row(new
                {
                    RoleId = 3,
                    UserId = 1
                })
                .Row(new
                {
                    RoleId = 4,
                    UserId = 1
                });

            Insert.IntoTable("Group").InSchema("JL")
                .Row(new
                {
                    Name = "Тестовая группа"
                });

            Insert.IntoTable("UserGroup").InSchema("JL")
                .Row(new
                {
                    UserId = 1,
                    GroupId = 1
                });
        }
    }
}
