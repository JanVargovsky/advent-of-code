wide = 25
tall = 6
size = wide * tall


def parse(input):
    return [input[(size*i):(size*(i+1))]
            for i in range(len(input) // size)]


def solve(layers):
    m = None
    mi = None
    for i, layer in enumerate(layers):
        c = layer.count("0")
        print(i, c)
        if m is None or c < m:
            m = c
            mi = i

    return layers[mi].count("1") * layers[mi].count("2")


with open("input.txt") as f:
    raw = f.readline().rstrip()

layers = parse(raw)
print(solve(layers))
