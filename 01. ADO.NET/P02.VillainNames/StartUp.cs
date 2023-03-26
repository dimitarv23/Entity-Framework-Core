using Microsoft.Data.SqlClient;
using System.Text;

namespace P02.VillainNames
{
    public class StartUp
    {
        static async Task Main(string[] args)
        {
            await using SqlConnection connection = 
                new SqlConnection(Config.ConnectionString);
            await connection.OpenAsync();

            string result = await GetVillainNamesAndTheirMinions(connection);
            Console.WriteLine(result);
        }

        static async Task<string> GetVillainNamesAndTheirMinions(SqlConnection connection)
        {
            StringBuilder sb = new StringBuilder();

            SqlCommand command = new SqlCommand(@"
                    SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount
                    FROM Villains AS v
                        JOIN MinionsVillains AS mv 
                        ON v.Id = mv.VillainId
                    GROUP BY v.Id, v.Name
                    HAVING COUNT(mv.VillainId) > 3
                    ORDER BY COUNT(mv.VillainId)", connection);

            SqlDataReader reader = await command.ExecuteReaderAsync();

            while (reader.Read())
            {
                string? villainName = reader["Name"].ToString();
                int countMinions = (int)reader["MinionsCount"];

                sb.AppendLine($"{villainName} - {countMinions}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}