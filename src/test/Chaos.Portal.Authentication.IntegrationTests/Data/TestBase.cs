namespace Chaos.Portal.Authentication.IntegrationTests.Data
{
    using System.Configuration;

    using Chaos.Deployment.UI.Console.Action.Database.Import;
    using Chaos.Portal.Authentication.Data;

    using NUnit.Framework;

    public class TestBase
    {
        protected AuthenticationRepository AuthenticationRepository { get; set; }

        [SetUp]
        public void SetUp()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["authentication"].ConnectionString;
            var importer = new ImportDeployment();
            
            importer.Parameters.ConnectionString = connectionString;
            importer.Parameters.Path = @"..\..\..\..\..\sql\6.data\initial.sql";

            importer.Run();

            importer.Parameters.Path = "integraion_tests_base_data.sql";

            importer.Run();

            AuthenticationRepository = new AuthenticationRepository(connectionString);
        }
    }
}
