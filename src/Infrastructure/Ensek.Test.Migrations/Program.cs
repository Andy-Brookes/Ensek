using DbUp;
using System.Reflection;

var connectionString = @"Data Source=(localdb)\ProjectModels;Initial Catalog=Ensek.Test.Database;Integrated Security=True;Pooling=False;Connect Timeout=30";
var upgradeEngine = DeployChanges
    .To
    .SqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .LogToConsole()
    .Build();

var result = upgradeEngine.PerformUpgrade();
if (result.Successful)
{
    Console.WriteLine("Migrations ran successfully");
    return 0;
}

Console.WriteLine(result.Error);
return -1;