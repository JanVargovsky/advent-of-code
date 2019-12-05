def diff(a, b):
    return list(filter(lambda i: a[i] != b[i], range(len(a))))


assert(diff("abcde", "axcye") == [1, 3])
assert(diff("fghij", "fguij") == [2])

with open('input.txt') as f:
    lines = f.readlines()

for i in range(len(lines)):
    for j in range(i + 1, len(lines)):
        d = diff(lines[i], lines[j])
        if len(d) == 1:
            print(lines[i][:d[0]] + lines[i][d[0]+1:])
