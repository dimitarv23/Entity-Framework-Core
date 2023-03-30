namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Exercise 2
            //Console.WriteLine(ExportAlbumsInfo(context, 9));

            //Exercise 3
            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
                .Where(a => a.ProducerId == producerId)
                .Include(a => a.Producer)
                .Include(a => a.Songs)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var album in albums
                .OrderByDescending(a => a.Price))
            {
                sb.AppendLine($"-AlbumName: {album.Name}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}");
                sb.AppendLine($"-ProducerName: {album.Producer.Name}");
                sb.AppendLine($"-Songs:");

                int counter = 1;
                foreach (var song in album.Songs
                    .OrderByDescending(s => s.Name)
                    .ThenBy(s => s.Writer.Name))
                {
                    sb.AppendLine($"---#{counter}");
                    sb.AppendLine($"---SongName: {song.Name}");
                    sb.AppendLine($"---Price: {song.Price:f2}");
                    sb.AppendLine($"---Writer: {song.Writer.Name}");
                    counter++;
                }

                sb.AppendLine($"-AlbumPrice: {album.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .Where(s => (s.Duration.Minutes * 60 + s.Duration.Seconds) > duration)
                .Include(s => s.Writer)
                .Include(s => s.Album)
                .Include(s => s.SongPerformers)
                .ToList();

            StringBuilder sb = new StringBuilder();
            int counter = 1;

            foreach (var song in songs
                .OrderBy(s => s.Name)
                .ThenBy(s => s.Writer.Name))
            {
                sb.AppendLine($"-Song #{counter}");
                sb.AppendLine($"---SongName: {song.Name}");
                sb.AppendLine($"---Writer: {song.Writer.Name}");

                foreach (var performer in song.SongPerformers
                    .OrderBy(sp => sp.Performer.FirstName)
                    .ThenBy(sp => sp.Performer.LastName))
                {
                    sb.AppendLine($"---Performer: {performer.Performer.FirstName} {performer.Performer.LastName}");
                }

                sb.AppendLine($"---AlbumProducer: {song.Album.Producer.Name}");
                sb.AppendLine($"---Duration: {song.Duration}");

                counter++;
            }

            return sb.ToString().TrimEnd();
        }
    }
}
