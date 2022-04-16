# PassPass
A simple password manager solution, meant to work on both x64 and ARM64 platforms.

## What's under the hood?

The encryption is based off XChaCha20-Poly1305 algorithm, which is an extended nonce variant of [ChaCha20-Poly1305](https://en.wikipedia.org/wiki/ChaCha20-Poly1305).

Key derivation is made using [Argon2](https://en.wikipedia.org/wiki/Argon2), specifically Argon2id version with 16 byte salts.

Password database is held in `Database` class, which has `DBCollection` with a list of `DBEntry`, which allows for simple, yet effective organization of contents.
Such structure allows for easy serialization to and from JSON, that is encrypted before storage on disk. Each `DBEntry` stores passwords in encrypted form even
during runtime, effectively decrypting them at very last moment that user requests it.

All those functionalities run thanks to following dependencies:
* [Isopoh.Cryptography.Argon2](https://github.com/mheyman/Isopoh.Cryptography.Argon2) by Michael Heyman - fully managed Argon2 implementation in .NET Core.

  Available under CC BY 4.0 License - [Link to license file](https://github.com/mheyman/Isopoh.Cryptography.Argon2/blob/master/LICENSE).
* [NaCl.Core](https://github.com/daviddesmet/NaCl.Core) by David De Smet - fully managed modern cryptographic primitives library in .NET Core.

  Available under MIT License - [Link to license file](https://github.com/daviddesmet/NaCl.Core/blob/master/LICENSE).
* [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) by James Newton-King - very popular, open source JSON framework for .NET.

  Available under MIT License - [Link to license file](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md) 
## Projects contained within this repository (subject to change)
* PassPassLib - Contains entire functionality regarding importing, exporting and modifying passwords. Fully documented.
* PassPassPlayground - Contains test environment for the library above. Does not contain documentation.
