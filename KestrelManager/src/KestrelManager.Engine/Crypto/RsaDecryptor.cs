using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;
using System;
using System.Collections.Generic;
using System.Text;

namespace KestrelManager.Engine.Crypto
{
    public class RsaDecryptor
    {

        private readonly AsymmetricCipherKeyPair _keys;

        public RsaDecryptor(TextReader keyReader)
        {
             var pr = new PemReader(keyReader);
             _keys = (AsymmetricCipherKeyPair)pr.ReadObject();
        }

        public string Decrypt(string text)
        {
            var cipherTextBytes = Convert.FromBase64String(text);
            var eng = new OaepEncoding(new RsaEngine());
            eng.Init(false, _keys.Private);

            var length = cipherTextBytes.Length;
            var blockSize = eng.GetInputBlockSize();
            var plainTextBytes = new List<byte>();
            for (var chunkPosition = 0;
                chunkPosition < length;
                chunkPosition += blockSize)
            {
                var chunkSize = Math.Min(blockSize, length - chunkPosition);
                plainTextBytes.AddRange(eng.ProcessBlock(
                    cipherTextBytes, chunkPosition, chunkSize
                ));
            }
            return Encoding.UTF8.GetString(plainTextBytes.ToArray());
        }
    }
}