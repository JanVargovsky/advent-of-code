using System.Text;

namespace AdventOfCode.Year2021.Day16;

class Solver
{
    public Solver()
    {
        Debug.Assert(ConvertToBits("D2FE28") == "110100101111111000101000");
        Debug.Assert(DecodeLiteral("101111111000101000") == "011111100101");
        Debug.Assert(Decode("11010001010") is LiteralPacket { Value: 10 });
        Debug.Assert(Decode("01010010001001000000000") is LiteralPacket { Value: 20 });

        // just to debug flow
        //Debug.Assert(Solve(@"38006F45291200") == "?");
        //Debug.Assert(Solve(@"EE00D40C823060") == "?");

        //Debug.Assert(Solve(@"8A004A801A8002F478") == "16");
        //Debug.Assert(Solve(@"620080001611562C8802118E34") == "12");
        //Debug.Assert(Solve(@"C0015000016115A2E0802F182340") == "23");
        //Debug.Assert(Solve(@"A0016C880162017C3686B18A3D4780") == "31");

        Debug.Assert(Solve("C200B40A82") == "3");
        Debug.Assert(Solve("04005AC33890") == "54");
        Debug.Assert(Solve("880086C3E88112") == "7");
        Debug.Assert(Solve("CE00C43D881120") == "9");
        Debug.Assert(Solve("D8005AC2A8F0") == "1");
        Debug.Assert(Solve("F600BC2D8F") == "0");
        Debug.Assert(Solve("9C005AC2F8F0") == "0");
        Debug.Assert(Solve("9C0141080250320F1802104A08") == "1");
    }

    public string Solve(string input)
    {
        var bits = ConvertToBits(input);
        var packet = Decode(bits);
        var result = Evaluate(packet);
        return result.ToString();
    }

    long Evaluate(Packet packet) => packet switch
    {
        LiteralPacket literalPacket => Evaluate(literalPacket),
        OperatorPacket operatorPacket => Evaluate(operatorPacket),
        _ => throw new NotSupportedException()
    };

    long Evaluate(LiteralPacket packet) => packet.Value;

    long Evaluate(OperatorPacket packet) => packet.TypeId switch
    {
        0 => packet.SubPackets.Sum(Evaluate),
        1 => packet.SubPackets.Aggregate(1L, (a, p) => a * Evaluate(p)),
        2 => packet.SubPackets.Min(Evaluate),
        3 => packet.SubPackets.Max(Evaluate),
        5 => Evaluate(packet.SubPackets[0]) > Evaluate(packet.SubPackets[1]) ? 1L : 0L,
        6 => Evaluate(packet.SubPackets[0]) < Evaluate(packet.SubPackets[1]) ? 1L : 0L,
        7 => Evaluate(packet.SubPackets[0]) == Evaluate(packet.SubPackets[1]) ? 1L : 0L,
        _ => throw new NotSupportedException()
    };

    Packet Decode(StringStream bits)
    {
        var version = Convert.ToByte(bits.Read(3).ToString(), 2);
        var typeID = Convert.ToByte(bits.Read(3).ToString(), 2);

        if (typeID is 4)
        {
            var literal = DecodeLiteral(bits);
            var number = Convert.ToInt64(literal, 2);
            return new LiteralPacket(version, typeID, number);
        }
        else
        {
            var subPackets = DecodeOperator(bits);
            return new OperatorPacket(version, typeID, subPackets);
        }
    }

    IList<Packet> DecodeOperator(StringStream bits)
    {
        var lengthTypeID = bits.Read();

        if (lengthTypeID == '0')
        {
            var totalLength = Convert.ToInt32(bits.Read(15).ToString(), 2);

            var start = bits.Offset;
            var subPackets = new List<Packet>();
            while (bits.Offset - start != totalLength)
            {
                subPackets.Add(Decode(bits));
            }
            return subPackets;
        }
        else
        {
            var numberOfSubPackets = Convert.ToInt32(bits.Read(11).ToString(), 2);

            var subPackets = new Packet[numberOfSubPackets];
            for (int i = 0; i < numberOfSubPackets; i++)
            {
                subPackets[i] = Decode(bits);
            }
            return subPackets;
        }
    }

    string DecodeLiteral(StringStream bits)
    {
        var sb = new StringBuilder();
        while (bits.Read() == '1')
        {
            sb.Append(bits.Read(4));
        }
        sb.Append(bits.Read(4));

        return sb.ToString();
    }

    string ConvertToBits(string packet)
    {
        var bits = new StringBuilder(packet.Length * 4);
        foreach (var base16 in packet)
        {
            //var base10 = Convert.ToInt16(base16.ToString(), 16);
            var base10 = base16 switch
            {
                >= '0' and <= '9' => base16 - '0',
                _ => base16 - 'A' + 10
            };
            var base2 = Convert.ToString(base10, 2);
            bits.Append(base2.PadLeft(4, '0'));
        }
        return bits.ToString();
    }

    record Packet(byte Version, byte TypeId);
    record LiteralPacket(byte Version, byte TypeId, long Value) : Packet(Version, TypeId);
    record OperatorPacket(byte Version, byte TypeId, IList<Packet> SubPackets) : Packet(Version, TypeId);

    record StringStream(string Data)
    {
        public int Offset { get; private set; }

        public char Read()
        {
            var c = Data[Offset];
            Offset++;
            return c;
        }

        public ReadOnlySpan<char> Read(int length)
        {
            var c = Data.AsSpan().Slice(Offset, length);
            Offset += length;
            return c;
        }

        public static implicit operator StringStream(string data) => new(data);
    }
}
