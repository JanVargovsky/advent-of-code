def path(graph, orbit):
    c = []
    while orbit in graph:
        orbit = graph[orbit]
        c.append(orbit)
    return c


def solve(graph):
    you = path(graph, "YOU")
    san = path(graph, "SAN")

    while you[-1] == san[-1]:
        you.pop(-1)
        san.pop(-1)
    return len(you) + len(san)


def load(lines):
    orbits = [line.strip().split(')') for line in lines]
    g = {o2: o1 for o1, o2 in orbits}
    return g


assert(solve(load(["COM)B",
                   "B)C",
                   "C)D",
                   "D)E",
                   "E)F",
                   "B)G",
                   "G)H",
                   "D)I",
                   "E)J",
                   "J)K",
                   "K)L",
                   "K)YOU",
                   "I)SAN"])) == 4)


with open('input.txt') as f:
    g = load(f.readlines())

print(solve(g))
