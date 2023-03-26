using Microsoft.Data.SqlClient;
using System.Text;

namespace P03.MinionNames
{
    public class StartUp
    {
        static async Task Main(string[] args)
        {
            await using SqlConnection connection =
                new SqlConnection(Config.ConnectionString);

            await connection.OpenAsync();

            int villainId = int.Parse(Console.ReadLine());

            string result = await
                GetMinionNamesAndAgesByVillainId(connection, villainId);

            Console.WriteLine(result);
        }

        static async Task<string> GetMinionNamesAndAgesByVillainId(SqlConnection connection, int villainId)
        {
            StringBuilder sb = new StringBuilder();

            SqlCommand getVillainNameByIdCmd =
                new SqlCommand(@"
                    SELECT Name FROM Villains 
                    WHERE Id = @VillainId", connection);

            getVillainNameByIdCmd.Parameters.AddWithValue("@VillainId", villainId);
            object? villainNameObj = await getVillainNameByIdCmd.ExecuteScalarAsync();

            if (villainNameObj == null)
            {
                return $"No villain with ID {villainId} exists in the database.";
            }

            string villainName = (string)villainNameObj;

            SqlCommand getMinionNamesOfVillainCmd =
                new SqlCommand(@"
                    SELECT ROW_NUMBER() OVER (ORDER BY m.Name) 
                        AS RowNum, m.Name, m.Age
                    FROM MinionsVillains AS mv
                        JOIN Minions As m 
                        ON mv.MinionId = m.Id
                    WHERE mv.VillainId = @VillainId
                    ORDER BY m.Name", connection);

            getMinionNamesOfVillainCmd.Parameters.AddWithValue("@VillainId", villainId);

            SqlDataReader minionsReader = await getMinionNamesOfVillainCmd.ExecuteReaderAsync();

            sb.AppendLine($"Villain: {villainName}");

            if (!minionsReader.HasRows)
            {
                sb.AppendLine("(no minions)");
            }
            else
            {
                while (minionsReader.Read())
                {
                    long rowNum = (long)minionsReader["RowNum"];
                    string minionName = (string)minionsReader["Name"];
                    int minionAge = (int)minionsReader["Age"];

                    sb.AppendLine($"{rowNum}. {minionName} {minionAge}");
                }
            }

            return sb.ToString().TrimEnd();
        }
    }
}