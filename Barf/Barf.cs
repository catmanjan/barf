using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Barf
{
    class Barf
    {
        public static string Extension = "barf";

        public static Random Random = new Random();

        public static void Compress(string sourcePath, string destinationPath = null)
        {
            if (destinationPath == null)
            {
                destinationPath = Path.ChangeExtension(sourcePath, Extension);
            }

            Chunk chunk = null;

            var extension = Path.GetExtension(sourcePath);
            var size = new FileInfo(sourcePath).Length;

            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(sourcePath))
                {
                    chunk = new Chunk
                    {
                        Extension = extension,
                        Size = size,
                        Hash = BitConverter.ToString(md5.ComputeHash(stream))
                    };
                }
            }

            if (chunk != null)
            {
                using (var sw = File.CreateText(destinationPath))
                {
                    sw.WriteLine(chunk.Extension);
                    sw.WriteLine(chunk.Size);
                    sw.WriteLine(chunk.Hash);
                }
            }
        }

        public static void Extract(string sourcePath, string destinationPath = null)
        {
            var chunk = new Chunk();

            using (var sr = File.OpenText(sourcePath))
            {
                chunk.Extension = sr.ReadLine();
                chunk.Size = long.Parse(sr.ReadLine());
                chunk.Hash = sr.ReadLine();
            }

            if (destinationPath == null)
            {
                destinationPath = Path.ChangeExtension(sourcePath, chunk.Extension);
            }

            var bytes = new byte[chunk.Size];

            using (var md5 = MD5.Create())
            {
                var hash = string.Empty;

                Stream stream = null;

                while (hash != chunk.Hash)
                {
                    Random.NextBytes(bytes);

                    stream = new MemoryStream(bytes);

                    hash = BitConverter.ToString(md5.ComputeHash(stream));
                }

                if (stream != null)
                {
                    using (var fileStream = File.Create(destinationPath))
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        stream.CopyTo(fileStream);
                    }

                    stream.Dispose();
                }
            }
        }

        private byte[] Permute(int length)
        {
            return new byte[length];
        }
    }
}
