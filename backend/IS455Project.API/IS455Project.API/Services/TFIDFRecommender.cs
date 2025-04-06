

namespace IS455Project.API.Services
{
    public class TFIDFRecommender
    {
        public static List<(string ContentId, double Score)> GetTopSimilarContent(string csvPath, string targetContentId, int topN)
        {
            var tfidfMatrix = LoadTFIDFMatrix(csvPath);

            if (!tfidfMatrix.ContainsKey(targetContentId))
                throw new ArgumentException($"ContentId '{targetContentId}' not found in the matrix.");

            return tfidfMatrix
                .Select(row => new { ContentId = row.Key, Score = row.Value[targetContentId] })
                .Where(x => x.ContentId != targetContentId)
                .OrderByDescending(x => x.Score)
                .Take(topN)
                .Select(x => (x.ContentId, x.Score))
                .ToList();
        }

        private static Dictionary<string, Dictionary<string, double>> LoadTFIDFMatrix(string filePath)
        {
            var matrix = new Dictionary<string, Dictionary<string, double>>();
            var lines = File.ReadAllLines(filePath);
            var headers = lines[0].Split(',').Skip(1).ToList();

            foreach (var line in lines.Skip(1))
            {
                var parts = line.Split(',');
                var rowId = parts[0];
                var values = parts.Skip(1).Select(double.Parse).ToList();

                var innerDict = new Dictionary<string, double>();
                for (int i = 0; i < headers.Count; i++)
                    innerDict[headers[i]] = values[i];

                matrix[rowId] = innerDict;
            }

            return matrix;
        }
    }

