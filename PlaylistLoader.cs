using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace musicvisualizerWPF
{
    public class PlaylistLoader
    {
        public List<Track> Load(string filePath)
        {
            var lines = File.ReadAllLines(filePath);

            return lines.Select(line =>
            {
                var parts = line.Split('|');
                if (parts.Length == 2)
                {
                    return new Track(parts[0], parts[1]);
                }

                return new Track("DefaultTrack", "DefaultFriendlyName");
            }).ToList();
        }
    }
}
