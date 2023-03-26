using Microsoft.Data.SqlClient;
using System.Text;

namespace P05.ChangeTownNamesCasing
{
    internal class StartUp
    {
        static async Task Main(string[] args)
        {
            await using SqlConnection connection =
                new SqlConnection(Config.ConnectionString);
            connection.Open();

            string nameOfCountry = Console.ReadLine();
            string result = await GetOutput(connection, nameOfCountry);

            Console.WriteLine(result);
        }

        static async Task<string> GetOutput(SqlConnection connection, string country)
        {
            int rowsAffected = await UpdateTownNamesOfACountry(connection, country);

            SqlCommand getOutputCmd = new SqlCommand(@"
                SELECT t.Name 
                FROM Towns as t
                    JOIN Countries AS c 
                    ON c.Id = t.CountryCode
                WHERE c.Name = @countryName", connection);
            getOutputCmd.Parameters.AddWithValue("@countryName", country);

            SqlDataReader townsReader = await getOutputCmd.ExecuteReaderAsync();

            StringBuilder sb = new StringBuilder();

            if (!townsReader.HasRows)
            {
                return "No town names were affected.";
            }
            else
            {
                sb.AppendLine($"{rowsAffected} town names were affected.");
                List<string> townsAffected = new List<string>();

                while (townsReader.Read())
                {
                    string townName = (string)townsReader["Name"];
                    townsAffected.Add(townName);
                }

                sb.AppendLine($"[{string.Join(", ", townsAffected)}]");
            }

            return sb.ToString().TrimEnd();
        }

        static async Task<int> UpdateTownNamesOfACountry(SqlConnection connection, string country)
        {
            SqlCommand updateTownNameCmd = new SqlCommand(@"
                UPDATE Towns
                SET Name = UPPER(Name)
                WHERE CountryCode = (
                    SELECT c.Id 
                    FROM Countries AS c 
                    WHERE c.Name = @countryName)", connection);
            updateTownNameCmd.Parameters.AddWithValue("@countryName", country);

            return await updateTownNameCmd.ExecuteNonQueryAsync();
        }
    }
}