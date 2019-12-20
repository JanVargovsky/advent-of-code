from collections import namedtuple
from queue import Queue

Int2 = namedtuple("Int2", ["x", "y"])


def find(m, c):
    for y in range(len(m)):
        for x in range(len(m[y])):
            if m[y][x] == c:
                return Int2(x, y)


def is_key(c):
    return c not in "@.#" and c.islower()


assert(is_key("a"))
assert(not is_key("A"))
assert(not is_key("."))


def is_door(c):
    return c not in "@.#" and c.isupper()


assert(is_door("A"))
assert(not is_door("a"))
assert(not is_door("."))


def enumerate2d(m):
    for y in range(len(m)):
        for x, v in enumerate(m[y]):
            yield Int2(x, y), v


directions = [
    Int2(1, 0),  # >
    Int2(0, 1),  # v
    Int2(-1, 0),  # <
    Int2(0, -1)  # ^
]


def find_path(m, start, end):
    visited = set()
    q = Queue()
    q.put([start])

    while not q.empty():
        *path, current = q.get()

        if current in visited:
            continue
        visited.add(current)

        if m[current.y][current.x] == "#":
            continue

        if current == end:
            return path + [current]

        for direction in directions:
            q.put(path +
                  [current, Int2(current.x + direction.x, current.y + direction.y)])

    return []


assert(find_path("""#########
#b.A.@.a#
#########""".splitlines(), Int2(1, 1), Int2(7, 1)) == [Int2(i, 1) for i in range(1, 8)])


def solve(m):
    all_keys = {v: p for p, v in enumerate2d(m) if is_key(v)}
    robots = {v: find(m, v) for v in "1234"}
    key_distances = {key: {} for key in ([*all_keys] + [*robots])}
    for a, pa in {**robots, **all_keys}.items():
        for b, pb in all_keys.items():
            if a == b or b in key_distances[a]:
                continue
            path = find_path(m, pa, pb)
            if not path:
                continue
            key_distances[a][b] = key_distances[b][a] = {
                # "path": path,
                "distance": len(path) - 1,
                "doors": frozenset(m[y][x] for x, y in path if is_door(m[y][x])),
                "keys": frozenset(m[y][x] for x, y in path if is_key(m[y][x])),
                "required_keys": frozenset(m[y][x].lower() for x, y in path[1:-1] if is_door(m[y][x]))
            }

    states = {(*robots.keys(), frozenset()): 0}  # positions, keys: distance
    best_distance = 1e10
    while states:
        new_states = {}
        for (*positions, keys), d in states.items():
            if not all_keys.keys() - keys:
                best_distance = min(best_distance, d)
                continue

            for i, position in enumerate(positions):
                for key, key_data in key_distances[position].items():
                    if key in keys:  # already obtained key
                        continue
                    if key_data["required_keys"] - keys:  # missing key
                        continue

                    new_keys = keys \
                        .union(key) \
                        .union(key_data["keys"])

                    new_distance = d + key_data["distance"]

                    new_positions = positions.copy()
                    new_positions[i] = key
                    new_positions = tuple((*new_positions, new_keys))
                    new_states[new_positions] = min(
                        new_states.get(new_positions, 1e10), new_distance)

        states = new_states
    return best_distance


def replace_robots(m):
    x, y = find(m, "@")
    m[y-1] = m[y-1][:x-1] + "1#2" + m[y-1][x+2:]
    m[y] = m[y][:x-1] + "###" + m[y][x+2:]
    m[y+1] = m[y+1][:x-1] + "3#4" + m[y+1][x+2:]
    return m


assert(solve(replace_robots("""#######
#a.#Cd#
##...##
##.@.##
##...##
#cB#Ab#
#######""".splitlines())) == 8)
assert(solve("""###############
#d.ABC.#.....a#
######1#2######
###############
######3#4######
#b.....#.....c#
###############""".splitlines()) == 24)
assert(solve("""#############
#DcBa.#.GhKl#
#.###1#2#I###
#e#d#####j#k#
###C#3#4###J#
#fEbA.#.FgHi#
#############""".splitlines()) == 32)
assert(solve("""#############
#g#f.D#..h#l#
#F###e#E###.#
#dCba1#2BcIJ#
#############
#nK.L3#4G...#
#M###N#H###.#
#o#m..#i#jk.#
#############""".splitlines()) == 72)


with open('input.txt') as f:
    m = [line.rstrip() for line in f.readlines()]

print(solve(replace_robots(m)))
