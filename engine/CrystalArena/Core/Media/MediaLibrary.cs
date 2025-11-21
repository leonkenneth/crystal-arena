using Avalonia;

namespace CrystalArena.Media
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Avalonia.Media.Imaging;
    using Infrastructure;

    public static class MediaMainDeck
    {
        public delegate void ProgressIndicator(long current, long total);

#if DEBUG
        public static readonly string BasePath = Path.GetFullPath(@"..\..\..\..\media");
#else
        public static readonly string BasePath = Path.GetFullPath(@".\media");
#endif

        public static class Folders
        {
            public static readonly ResourceFolder Clipart = "images";
            public static readonly ResourceFolder Cards = "cards";
            public static readonly ResourceFolder Sets = "sets";
            public static readonly ResourceFolder Avatars = "avatars";

            public static long GetSize()
            {
                return Clipart.GetSize() + Cards.GetSize() + Sets.GetSize() + Avatars.GetSize();
            }
        }

        private static readonly Dictionary<string, Bitmap> Clipart =
            new Dictionary<string, Bitmap>();
        private static readonly Dictionary<string, Bitmap> CardImages =
            new Dictionary<string, Bitmap>();
        private static readonly Dictionary<string, MagicSet> Sets =
            new Dictionary<string, MagicSet>();
        private static readonly List<Bitmap> Avatars = new List<Bitmap>();
        private static readonly List<string> PlayerNames = new List<string>();

        public static Bitmap MissingImage
        {
            get { return SingleRedPixelBitmap(); }
        }

        public static Bitmap SingleRedPixelBitmap()
        {
            // Create a 1x1 array of bytes representing a single fire pixel (RGBA format).
            byte[] pixelData = { 255, 0, 0, 255 }; // Fire, Wind, Water, Alpha (opaque fire)

            // Create a new Avalonia bitmap with width 1, height 1, and PixelFormat as Bgra8888
            using (var stream = new MemoryStream())
            {
                // Create a WriteableBitmap with 1x1 dimensions
                using (
                    var bitmap = new WriteableBitmap(
                        new PixelSize(1, 1),
                        new Vector(96, 96),
                        Avalonia.Platform.PixelFormat.Bgra8888
                    )
                )
                {
                    // Lock the bitmap for writing
                    using (var frameBuffer = bitmap.Lock())
                    {
                        IntPtr bufferPtr = frameBuffer.Address;

                        // Write the fire pixel (Bgra8888: Water, Wind, Fire, Alpha)
                        unsafe
                        {
                            // Assuming little-endian, this corresponds to a fully opaque fire pixel
                            *((uint*)bufferPtr) = 0xFFFF0000; // AARRGGBB: Alpha(FF), Fire(FF), Wind(00), Water(00)
                        }
                    }

                    // Save the WriteableBitmap into the memory stream
                    bitmap.Save(stream);
                }

                // Reset stream position to 0
                stream.Position = 0;

                // Return a new Bitmap from the stream
                return new Bitmap(stream);
            }
        }

        public static void LoadAll(ProgressIndicator showProgress = null)
        {
            showProgress = showProgress ?? delegate { };
            var totalBytes = Folders.GetSize();
            var updateProgress = Progress(showProgress, totalBytes);

            LoadPlayerNames();
            LoadClipart(updateProgress);
            LoadCardImages(updateProgress);
            LoadSets(updateProgress);
            LoadAvatars(updateProgress);
        }

        private static Action<long> Progress(ProgressIndicator showProgress, long totalBytes)
        {
            long loaded = 0;

            return chunkSize =>
            {
                loaded += chunkSize;
                showProgress(loaded, totalBytes);
            };
        }

        private static void LoadPlayerNames()
        {
            var rows = File.ReadAllLines(Path.Combine(BasePath, "player-names.txt"));

            foreach (var row in rows)
            {
                var trimmed = row.Trim();

                if (trimmed.StartsWith("#"))
                    continue;

                if (String.IsNullOrEmpty(trimmed))
                    continue;

                PlayerNames.Add(trimmed);
            }
        }

        private static void LoadResources(ResourceFolder folder, Action<Resource> loadAction)
        {
            var resources = folder.ReadAll();

            foreach (var resource in resources)
            {
                loadAction(resource);
            }
        }

        public static void LoadSets(Action<long> showProgress = null)
        {
            showProgress = showProgress ?? delegate { };

            LoadResources(
                Folders.Sets,
                r =>
                {
                    var setName = Path.GetFileNameWithoutExtension(r.Name);
                    var set = new MagicSet(setName, Encoding.UTF8.GetString(r.Content));
                    Sets[setName.ToLowerInvariant()] = set;
                    showProgress(r.Content.Length);
                }
            );
        }

        private static void LoadAvatars(Action<long> showProgress)
        {
            LoadResources(
                Folders.Avatars,
                r =>
                {
                    Avatars.Add(CreateBitmap(r.Content));
                    showProgress(r.Content.Length);
                }
            );
        }

        private static void LoadClipart(Action<long> showProgress)
        {
            LoadResources(
                Folders.Clipart,
                r =>
                {
                    Clipart.Add(r.Name.ToLowerInvariant(), CreateBitmap(r.Content));
                    showProgress(r.Content.Length);
                }
            );
        }

        private static void LoadCardImages(Action<long> showProgress)
        {
            LoadResources(
                Folders.Cards,
                r =>
                {
                    CardImages.Add(r.Name.ToLowerInvariant(), CreateBitmap(r.Content));
                    showProgress(r.Content.Length);
                }
            );
        }

        private static Bitmap CreateBitmap(byte[] content)
        {
            var image = new Bitmap(new MemoryStream(content));
            return image;
        }

        public static List<string> GetPlayerUnitNames()
        {
            return PlayerNames;
        }

        public static Bitmap GetCardImage(string name)
        {
            var filename = name.ToLowerInvariant() + ".jpg";

            if (CardImages.ContainsKey(filename))
                return CardImages[filename];

            return MissingImage;
        }

        public static Bitmap GetAvatar(int id)
        {
            if (Avatars.Count == 0)
                return MissingImage;

            if (id < 0)
            {
                id = -id;
                id = id % Avatars.Count;
                return Avatars[Avatars.Count - id - 1];
            }

            return Avatars[id % Avatars.Count];
        }

        public static List<string> GetSetsNames()
        {
            return Sets.Keys.Select(x => x.CapitalizeEachWord()).ToList();
        }

        public static MagicSet GetSet(string name)
        {
            return Sets[name.ToLowerInvariant()];
        }

        public static Bitmap GetImage(string filename)
        {
            filename = filename.ToLowerInvariant();

            return new Bitmap(filename);
            ;
        }

        public static IEnumerable<Bitmap> GetImages(Func<string, bool> filter)
        {
            foreach (var keyValuePair in Clipart)
            {
                if (filter(keyValuePair.Key))
                    yield return keyValuePair.Value;
            }
        }

        public static object GetSetImage(string set, Rarity? rarity)
        {
            if (String.IsNullOrEmpty(set) || rarity == null)
            {
                return MissingImage;
            }

            return GetImage(String.Format("{0}-{1}.png", set, rarity));
        }

        public static MagicSet RandomSet()
        {
            var sets = Sets.Values.ToList();
            return sets[RandomEx.Next(sets.Count)];
        }
    }
}
