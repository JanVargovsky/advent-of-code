def distance(graph, orbit):
    c = 0
    while orbit in graph:
        orbit = graph[orbit]
        c += 1
    return c


def solve(graph):
    c = 0
    for orbit in graph.keys():
        c += distance(graph, orbit)
    return c


def load(lines):
    orbits = [line.strip().split(')') for line in lines]
    g = {o2: o1 for o1, o2 in orbits}
    return g


assert(distance(load(["COM)B",
                      "B)C",
                      "C)D",
                      "D)E",
                      "E)F",
                      "B)G",
                      "G)H",
                      "D)I",
                      "E)J",
                      "J)K",
                      "K)L"]), "L") == 7)

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
                   "K)L"])) == 42)


with open('input.txt') as f:
    g = load(f.readlines())

print(solve(g))
