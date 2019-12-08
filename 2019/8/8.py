wide = 25
tall = 6
size = wide * tall


def parse(input, size=size):
    return [input[(size*i):(size*(i+1))]
            for i in range(len(input) // size)]


def solve(layers, size=size):
    image = list("2" * size)

    for layer in layers:
        for i, pixel in enumerate(layer):
            if image[i] == '2':
                image[i] = pixel

    return "".join(image)


assert(solve(parse("0222112222120000", 2*2), 2*2) == "0110")


with open("input.txt") as f:
    raw = f.readline().rstrip()

layers = parse(raw)
decoded = solve(layers)

for i in range(tall):
    print(decoded[wide * i:(wide * (i+1))].replace("1", ".").replace("0", " "))
