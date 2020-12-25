using System;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Year2020.Day25
{
    class Solver
    {
        public Solver()
        {
            Debug.Assert(DetermineLoopSizeFromPublicKey(5764801, 20201227) == 8);
            Debug.Assert(DetermineLoopSizeFromPublicKey(17807724, 20201227) == 11);
            Debug.Assert(Solve(@"5764801
17807724") == "14897079");
        }

        public string Solve(string input)
        {
            var keys = input.Split(Environment.NewLine)
                .Select(int.Parse)
                .ToArray();

            var cardPublicKey = keys[0];
            var doorPublicKey = keys[1];
            const int remainder = 20201227;

            var cardSecretLoopSize = DetermineLoopSizeFromPublicKey(cardPublicKey, remainder);
            var doorSecretLoopSize = DetermineLoopSizeFromPublicKey(doorPublicKey, remainder);

            var encryptionKey1 = Transform(cardPublicKey, doorSecretLoopSize, remainder);
            var encryptionkey2 = Transform(doorPublicKey, cardSecretLoopSize, remainder);
            Debug.Assert(encryptionKey1 == encryptionkey2);

            return encryptionKey1.ToString();
        }

        int Transform(int subjectNumber, int loopSize, int remainder)
        {
            return (int)BigInteger.ModPow(subjectNumber, loopSize, remainder);
        }

        int DetermineLoopSizeFromPublicKey(int publicKey, int remainder)
        {
            const int subjectNumber = 7;

            var value = 1;
            for (int loopSize = 1; loopSize < int.MaxValue; loopSize++)
            {
                value *= subjectNumber;
                value %= remainder;

                if (value == publicKey)
                    return loopSize;
            }

            throw new Exception();
        }
    }
}
