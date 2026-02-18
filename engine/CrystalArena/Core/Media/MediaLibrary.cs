namespace CrystalArena.Media
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
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

        private static readonly Dictionary<string, object?> Clipart =
            new Dictionary<string, object?>();
        private static readonly Dictionary<string, object?> CardImages =
            new Dictionary<string, object?>();
        private static readonly Dictionary<string, MagicSet> Sets =
            new Dictionary<string, MagicSet>();
        private static readonly List<object?> Avatars = new List<object?>();
        private static readonly List<string> PlayerNames = new List<string>();

        public static object? MissingImage
        {
            get { return null; }
        }

        public static object? SingleRedPixelBitmap()
        {
            // Stub - images are served by image_proxy
            return null;
        }

        public static void LoadAll(ProgressIndicator? showProgress = null)
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
            var path = Path.Combine(BasePath, "player-names.txt");
            if (!File.Exists(path))
                return;

            var rows = File.ReadAllLines(path);

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

        public static void LoadSets(Action<long>? showProgress = null)
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
                    // Stub - images are served by image_proxy
                    Avatars.Add(null);
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
                    // Stub - images are served by image_proxy
                    Clipart.Add(r.Name.ToLowerInvariant(), null);
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
                    // Stub - images are served by image_proxy
                    CardImages.Add(r.Name.ToLowerInvariant(), null);
                    showProgress(r.Content.Length);
                }
            );
        }

        public static List<string> GetPlayerUnitNames()
        {
            return PlayerNames;
        }

        public static object? GetCardImage(string name)
        {
            var filename = name.ToLowerInvariant() + ".jpg";

            if (CardImages.ContainsKey(filename))
                return CardImages[filename];

            return MissingImage;
        }

        public static object? GetAvatar(int id)
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

        public static object? GetImage(string filename)
        {
            // Stub - images are served by image_proxy
            return null;
        }

        public static IEnumerable<object?> GetImages(Func<string, bool> filter)
        {
            foreach (var keyValuePair in Clipart)
            {
                if (filter(keyValuePair.Key))
                    yield return keyValuePair.Value;
            }
        }

        public static object? GetSetImage(string set, Rarity? rarity)
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
