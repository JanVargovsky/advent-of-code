from itertools import chain
from collections import defaultdict

size = 5


def index(x, y):
    return y * size + x


def index2xy(index):
    return index % size, index // size


def is_valid(x, y):
    return x >= 0 and y >= 0 and x < size and y < size


adjacent_tiles = [(0, -1), (-1, 0), (1, 0), (0, 1)]
inner_adjacent_tiles = {
    (size // 2, size // 2 - 1): [(x, 0) for x in range(size)],
    (size // 2, size // 2 + 1): [(x, size - 1) for x in range(size)],
    (size // 2 - 1, size // 2): [(0, y) for y in range(size)],
    (size // 2 + 1, size // 2): [(size - 1, y) for y in range(size)],
}
outer_adjacent_tiles = {}
for key, values in inner_adjacent_tiles.items():
    for value in values:
        if value in outer_adjacent_tiles:
            outer_adjacent_tiles[value] += [key]
        else:
            outer_adjacent_tiles[value] = [key]


def get_adjacent_tiles(grids, g, x, y):
    p = (x, y)
    result = {}
    for adjacent in adjacent_tiles:
        pi = x + adjacent[0], y + adjacent[1]
        if pi == (2, 2) or not is_valid(*pi):
            if p in inner_adjacent_tiles:
                gi = g + 1
                points = inner_adjacent_tiles[p]
            elif p in outer_adjacent_tiles:
                gi = g - 1
                points = outer_adjacent_tiles[p]

            for pi in points:
                result[(gi, pi)] = grids[gi][index(*pi)]
        else:
            result[(g, pi)] = grids[g][index(*pi)]

    return result.values()


outer = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, None,
         14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25]
inner = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", None,
         "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y"]

assert set(get_adjacent_tiles(
    [outer], 0, *index2xy(outer.index(19)))) == set([14, 18, 20, 24])
assert set(get_adjacent_tiles(
    [inner], 0, *index2xy(inner.index("G")))) == set(["B", "F", "H", "L"])
assert set(get_adjacent_tiles(
    [outer, inner], 1, *index2xy(inner.index("D")))) == set([8, "C", "E", "I"])
assert set(get_adjacent_tiles(
    [outer, inner], 1, *index2xy(inner.index("E")))) == set([8, "D", 14, "J"])
assert set(get_adjacent_tiles(
    [outer, inner], 0, *index2xy(outer.index(14)))) == set([9, "E", "J", "O", "T", "Y", 15, 19])
assert set(get_adjacent_tiles(
    [inner, outer], 0, *index2xy(inner.index("N")))) == set(["I", "O", "S", 5, 10, 15, 20, 25])


def adjacent_bugs(grids, g, x, y):
    return sum(1 for t in get_adjacent_tiles(grids, g, x, y) if t == "#")


def tick(grids, g):
    new_grid = []
    for y in range(size):
        for x in range(size):
            if x == 2 and y == 2:
                new_grid.append("?")
                continue

            bugs = adjacent_bugs(grids, g, x, y)
            if grids[g][index(x, y)] == "#" and bugs != 1:
                new_grid.append(".")  # dies
            elif grids[g][index(x, y)] == "." and bugs in [1, 2]:
                new_grid.append("#")  # infested
            else:
                new_grid.append(grids[g][index(x, y)])  # remains
    return new_grid


def calculate_biodiversity_rating(grid):
    return sum([2 ** i for i, value in enumerate(grid) if value == "#"])


grid = []
with open('input.txt') as f:
    for l in f.readlines():
        grid += list(l.rstrip())

empty_grid = ["."] * size ** 2
grids = defaultdict(lambda: empty_grid)
grids[0] = grid

for _ in range(200):
    new_grids = defaultdict(lambda: empty_grid)
    min_depth = min(grids.keys())
    max_depth = max(grids.keys())

    for depth in range(min_depth, max_depth + 1):
        new_grids[depth] = tick(grids, depth)

    for depth in (min_depth - 1, max_depth + 1):
        if depth in grids:
            new_grid = tick(grids, depth)
            if "#" in new_grid:
                new_grids[depth] = new_grid
    grids = new_grids

print(sum(1 for i in chain(*grids.values()) if i == "#"))
