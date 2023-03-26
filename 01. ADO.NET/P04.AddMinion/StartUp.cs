using Azure.Core;
using Microsoft.Data.SqlClient;
using System.Text;

namespace P04.AddMinion
{
    public class StartUp
    {
        static async Task Main(string[] args)
        {
            await using SqlConnection connection =
                new SqlConnection(Config.ConnectionString);
            connection.Open();

            string? minionInfo = Console.ReadLine();
            string? villainInfo = Console.ReadLine();

            string[]? minionArgs = minionInfo?
                .Split(": ", StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
            string[]? villainName = villainInfo?
                .Split(": ", StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            string result = await AddNewMinion(connection, minionArgs[1], villainName[1]);

            Console.WriteLine(result);
        }

        static async Task<string> AddNewMinion(SqlConnection connection, string minionInfo, string villainName)
        {
            string[] minionArgs = minionInfo
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            string minionName = minionArgs[0];
            int minionAge = int.Parse(minionArgs[1]);
            string townName = minionArgs[2];

            StringBuilder sb = new StringBuilder();

            try
            {
                int townId = await GetTownIdOrAddTown(connection, sb, townName);
                int villainId = await GetVillainIdOrAddVillain(connection, sb, villainName);
                int minionId = await AddNewMinion(connection, minionName, minionAge, townId);

                await SetMinionToVillain(connection, minionId, villainId);

                sb.AppendLine($"Successfully added {minionName} to be minion of {villainName}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return sb.ToString().TrimEnd();
        }

        private static async Task<int> GetTownIdOrAddTown(SqlConnection connection, StringBuilder sb, string townName)
        {
            SqlCommand getTownIdCmd =
                    new SqlCommand(@"
                        SELECT Id 
                        FROM Towns 
                        WHERE Name = @townName", connection);
            getTownIdCmd.Parameters.AddWithValue("@townName", townName);

            object? townIdObj = await getTownIdCmd.ExecuteScalarAsync();

            if (townIdObj == null)
            {
                SqlCommand addNewTownCmd =
                    new SqlCommand(@"
                            INSERT INTO Towns (Name) 
                            VALUES (@townName)", connection);
                addNewTownCmd.Parameters.AddWithValue("@townName", townName);

                await addNewTownCmd.ExecuteNonQueryAsync();
                townIdObj = await getTownIdCmd.ExecuteScalarAsync();

                sb.AppendLine($"Town {townName} was added to the database.");
            }

            return (int)townIdObj;
        }

        private static async Task<int> GetVillainIdOrAddVillain(
            SqlConnection connection, StringBuilder sb, string villainName)
        {
            SqlCommand getVillainIdCmd =
                    new SqlCommand(@"
                        SELECT Id 
                        FROM Villains 
                        WHERE Name = @Name", connection);
            getVillainIdCmd.Parameters.AddWithValue("@Name", villainName);

            object? villainIdObj = await getVillainIdCmd.ExecuteScalarAsync();

            if (villainIdObj == null)
            {
                SqlCommand addNewVillainCmd =
                    new SqlCommand(@"
                            INSERT INTO Villains (Name, EvilnessFactorId) 
                            VALUES (@villainName, 4)", connection);
                addNewVillainCmd.Parameters.AddWithValue("@villainName", villainName);

                await addNewVillainCmd.ExecuteNonQueryAsync();
                villainIdObj = await getVillainIdCmd.ExecuteScalarAsync();

                sb.AppendLine($"Villain {villainName} was added to the database.");
            }

            return (int)villainIdObj;
        }

        private static async Task<int> AddNewMinion(SqlConnection connection,
            string minionName, int minionAge, int townId)
        {
            SqlCommand addNewMinionCmd =
                new SqlCommand(@"
                    INSERT INTO Minions (Name, Age, TownId) 
                    VALUES (@name, @age, @townId)", connection);
            addNewMinionCmd.Parameters.AddWithValue("@name", minionName);
            addNewMinionCmd.Parameters.AddWithValue("@age", minionAge);
            addNewMinionCmd.Parameters.AddWithValue("@townId", townId);

            await addNewMinionCmd.ExecuteNonQueryAsync();

            SqlCommand getMinionIdCmd =
                new SqlCommand(@"
                    SELECT Id 
                    FROM Minions 
                    WHERE Name = @Name", connection);
            getMinionIdCmd.Parameters.AddWithValue("@Name", minionName);

            object? minionIdObj = await getMinionIdCmd.ExecuteScalarAsync();
            return (int)minionIdObj;
        }

        private static async Task SetMinionToVillain(SqlConnection connection,
            int minionId, int villainId)
        {
            SqlCommand setMinionCmd =
                new SqlCommand(@"
                    INSERT INTO MinionsVillains (MinionId, VillainId) 
                    VALUES (@minionId, @villainId)", connection);
            setMinionCmd.Parameters.AddWithValue("@minionId", minionId);
            setMinionCmd.Parameters.AddWithValue("@villainId", villainId);

            await setMinionCmd.ExecuteNonQueryAsync();
        }
    }
}