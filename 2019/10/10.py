from queue import Queue
from collections import namedtuple
from math import sqrt, atan2

Point = namedtuple("Point", ["x", "y"])


def generate_area_points(center, size):
    for i in range(2 * size):
        yield Point(center.x - size + i, center.y - size)
        yield Point(center.x + size, center.y - size + i)
        yield Point(center.x + size - i, center.y + size)
        yield Point(center.x - size, center.y + size - i)


def is_valid(p, size):
    return p.x >= 0 and p.y >= 0 and p.x < size and p.y < size


def detect(map, x, y):
    s = len(map)
    center = Point(x, y)
    visited = set()

    for size in range(1, s):
        for p in generate_area_points(center, size):
            if not is_valid(p, s):
                continue
            pn = atan2(p.x - x, p.y - y)
            if not pn in visited and map[p.y][p.x] != ".":
                # print(map[p.y][p.x])
                visited.add(pn)

    return len(visited)


def solve(map):
    best = 0
    for x in range(len(map)):
        for y in range(len(map)):
            if map[y][x] != '.':
                best = max(best, detect(map, x, y))
    return best


assert(detect([".#..#",
               ".....",
               "#####",
               "....#",
               "...##"], 1, 0) == 7)

assert(detect([".#..#",
               ".....",
               "#####",
               "....#",
               "...##"], 3, 4) == 8)

assert(solve([".#..#",
              ".....",
              "#####",
              "....#",
              "...##"]) == 8)

with open('input.txt') as f:
    map = [line.rstrip() for line in f.readlines()]

print(solve(map))
