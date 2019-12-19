from collections import namedtuple
from queue import Queue

Int2 = namedtuple("Int2", ["x", "y"])


def find_entrance(m):
    for y in range(len(m)):
        for x in range(len(m[y])):
            if m[y][x] == "@":
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

    while q:
        *path, current = q.get()

        if current in visited or m[current.y][current.x] == "#":
            continue

        if current == end:
            return path + [current]

        visited.add(current)

        for direction in directions:
            q.put(path +
                  [current, Int2(current.x + direction.x, current.y + direction.y)])

    return []


assert(find_path("""#########
#b.A.@.a#
#########""".splitlines(), Int2(1, 1), Int2(7, 1)) == [Int2(i, 1) for i in range(1, 8)])


def solve(m):
    all_keys = {v: p for p, v in enumerate2d(m) if is_key(v)}
    key_distances = {key: {} for key in all_keys.keys()}
    key_distances["@"] = {}
    for a, pa in {"@": find_entrance(m), **all_keys}.items():
        for b, pb in all_keys.items():
            if a == b or b in key_distances[a]:
                continue
            path = find_path(m, pa, pb)
            key_distances[a][b] = key_distances[b][a] = {
                # "path": path,
                "distance": len(path) - 1,
                "doors": frozenset(m[y][x] for x, y in path if is_door(m[y][x])),
                "keys": frozenset(m[y][x] for x, y in path if is_key(m[y][x])),
                "required_keys": frozenset(m[y][x].lower() for x, y in path if is_door(m[y][x]))
            }

    states = {("@", frozenset("@")): 0}  # position, keys: distance
    best_distance = 1e10
    while states:
        new_states = {}
        for (position, keys), d in states.items():
            if not all_keys.keys() - keys:
                best_distance = min(best_distance, d)
                continue

            for key, key_data in key_distances[position].items():
                if key in keys:  # already obtained key
                    continue
                if key_data["required_keys"] - keys:  # missing key
                    continue

                new_keys = keys \
                    .union(key) \
                    .union(key_data["keys"])
                new_distance = d + key_data["distance"]
                new_states[(key, new_keys)] = min(
                    new_states.get((key, new_keys), 1e10), new_distance)

        states = new_states
    return best_distance


assert(solve("""#########
#b.A.@.a#
#########""".splitlines()) == 8)
assert(solve("""########################
#f.D.E.e.C.b.A.@.a.B.c.#
######################.#
#d.....................#
########################""".splitlines()) == 86)
assert(solve("""########################
#...............b.C.D.f#
#.######################
#.....@.a.B.c.d.A.e.F.g#
########################""".splitlines()) == 132)
assert(solve("""#################
#i.G..c...e..H.p#
########.########
#j.A..b...f..D.o#
########@########
#k.E..a...g..B.n#
########.########
#l.F..d...h..C.m#
#################""".splitlines()) == 136)
assert(solve("""########################
#@..............ac.GI.b#
###d#e#f################
###A#B#C################
###g#h#i################
########################""".splitlines()) == 81)


with open('input.txt') as f:
    m = [line.rstrip() for line in f.readlines()]

print(solve(m))
