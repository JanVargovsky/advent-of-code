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

        Debug.Assert(Solve(@"8A004A801A8002F478") == "16");
        Debug.Assert(Solve(@"620080001611562C8802118E34") == "12");
        Debug.Assert(Solve(@"C0015000016115A2E0802F182340") == "23");
        Debug.Assert(Solve(@"A0016C880162017C3686B18A3D4780") == "31");
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

    long Evaluate(LiteralPacket packet) => packet.Version;

    long Evaluate(OperatorPacket packet) => packet.Version + packet.SubPackets.Sum(Evaluate);

    Packet Decode(string bits)
    {
        int offset = 0;
        var packet = Decode(bits, ref offset);
        return packet;
    }

    Packet Decode(string bits, ref int offset)
    {
        var version = Convert.ToByte(bits[offset..(offset + 3)], 2);
        offset += 3;
        var typeID = Convert.ToByte(bits[offset..(offset + 3)], 2);
        offset += 3;

        if (typeID is 4)
        {
            var literal = DecodeLiteral(bits, ref offset);
            var number = Convert.ToInt64(literal, 2);
            return new LiteralPacket(version, typeID, number);
        }
        else
        {
            var subPackets = DecodeOperator(bits, ref offset);
            return new OperatorPacket(version, typeID, subPackets.ToArray());
        }
    }

    IEnumerable<Packet> DecodeOperator(string bits, ref int offset)
    {
        var lengthTypeID = bits[offset];
        offset += 1;

        if (lengthTypeID == '0')
        {
            var totalLength = Convert.ToInt32(bits[offset..(offset + 15)], 2);
            offset += 15;

            var start = offset;
            var subPackets = new List<Packet>();
            while (offset - start != totalLength)
            {
                subPackets.Add(Decode(bits, ref offset));
            }
            return subPackets;
        }
        else
        {
            var numberOfSubPackets = Convert.ToInt32(bits[offset..(offset + 11)], 2);
            offset += 11;

            var subPackets = new Packet[numberOfSubPackets];
            for (int i = 0; i < numberOfSubPackets; i++)
            {
                subPackets[i] = Decode(bits, ref offset);
            }
            return subPackets;
        }
    }

    string DecodeLiteral(string bits)
    {
        var offset = 0;
        return DecodeLiteral(bits, ref offset);
    }

    string DecodeLiteral(string bits, ref int offset)
    {
        var sb = new StringBuilder();
        while (bits[offset] == '1')
        {
            sb.Append(bits[(offset + 1)..(offset + 5)]);
            offset += 5;
        }
        sb.Append(bits[(offset + 1)..(offset + 5)]);
        offset += 5;

        return sb.ToString();
    }

    string ConvertToBits(string packet)
    {
        var bits = new StringBuilder();
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
    record OperatorPacket(byte Version, byte TypeId, Packet[] SubPackets) : Packet(Version, TypeId);
}
