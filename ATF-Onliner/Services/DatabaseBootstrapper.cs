using ATF_Onliner.Context;

namespace ATF_Onliner.Services
{
    public class DatabaseBootstrapper
    {
        private readonly UserContext context;

        public DatabaseBootstrapper(UserContext context)
        {
            this.context = context;
        }

        // public void Configure()
        // {
        //     if (context.Database.Exist())
        //         return;
        //
        //     context.Database.Create();
        //     var seeder = new Seeder(context);
        //     seeder.SeedDatabase();
        // }
    }
}