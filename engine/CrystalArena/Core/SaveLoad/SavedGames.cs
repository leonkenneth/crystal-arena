namespace CrystalArena
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters;
    using System.Runtime.Serialization.Formatters.Binary;

    public static class SavedGames
    {
        public static void WriteToStream(SaveFileHeader header, object data, Stream stream)
        {
            var formatter = CreateFormatter();
            formatter.Serialize(stream, header);
            formatter.Serialize(stream, data);
        }

        public static SaveGameFile ReadFromStream(Stream stream, DateTime? modifiedAt = null)
        {
            var formatter = CreateFormatter();
            var header = (SaveFileHeader)formatter.Deserialize(stream);
            var data = formatter.Deserialize(stream);

            return new SaveGameFile(header, data, modifiedAt);
        }

        private static BinaryFormatter CreateFormatter()
        {
            return new BinaryFormatter
            {
                AssemblyFormat = FormatterAssemblyStyle.Simple,
                Binder = new RenameBinder(),
            };
        }
    }
}
