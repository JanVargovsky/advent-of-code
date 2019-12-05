def checksum(lines):
    twos = 0
    threes = 0
    for l in lines:
        counts = {}
        for c in l:
            if c not in counts:
                counts[c] = 1
            else:
                counts[c] += 1
        two = False
        three = False
        for k, v in counts.items():
            if v == 2:
                two = True
            elif v == 3:
                three = True
        if two:
            twos += 1
        if three:
            threes += 1
    return twos * threes

assert(checksum(["abcdef", "bababc", "abbcde", "abcccd", "aabcdd", "abcdee", "ababab"]) == 12)

with open('input.txt') as f:
    lines = f.readlines()

print(checksum(lines))
