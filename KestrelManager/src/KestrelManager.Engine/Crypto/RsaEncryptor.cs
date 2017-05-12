using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace KestrelManager.Engine.Crypto
{
    public class RsaEncryptor
    {
        private readonly RsaKeyParameters _keys;

        public RsaEncryptor(TextReader keyReader)
        {
            var pr = new PemReader(keyReader);
            _keys = (RsaKeyParameters)pr.ReadObject();
        }

        public string Encrypt(string text)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(text);

            var eng = new OaepEncoding(new RsaEngine());
            eng.Init(true, _keys);

            var length = plainTextBytes.Length;
            var blockSize = eng.GetInputBlockSize();
            var cipherTextBytes = new List<byte>();
            for (var chunkPosition = 0;
                chunkPosition < length;
                chunkPosition += blockSize)
            {
                var chunkSize = Math.Min(blockSize, length - chunkPosition);
                cipherTextBytes.AddRange(eng.ProcessBlock(
                    plainTextBytes, chunkPosition, chunkSize
                ));
            }
            return Convert.ToBase64String(cipherTextBytes.ToArray());
        }
    }
}