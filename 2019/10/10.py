from collections import namedtuple
from math import sqrt, atan2, degrees

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


def find_location(map):
    best = -1
    p = None
    for x in range(len(map[0])):
        for y in range(len(map)):
            if map[y][x] != '.':
                a = detect(map, x, y)
                if a > best:
                    best = a
                    p = Point(x, y)
    return p


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

assert(find_location([".#..#",
                      ".....",
                      "#####",
                      "....#",
                      "...##"]) == Point(3, 4))


AsteroidPoint = namedtuple("AsteroidPoint", ["x", "y", "angle", "distance"])


def solve(map, p):
    asteroids = []
    for y in range(len(map)):
        for x in range(len(map[0])):
            if p.x == x and p.y == y:
                continue
            if map[y][x] != '.':
                angle = degrees(atan2(p.y - y, p.x - x)) - 90
                angle = (angle + 360) % 360
                distance = sqrt((x - p.x) ** 2 + (y - p.y) ** 2)
                asteroids.append(AsteroidPoint(x, y, angle, distance))

    asteroids.sort(key=lambda a: (a.angle, a.distance))

    angle = asteroids[0].angle - 0.0001
    i = 0
    while len(asteroids) > 0:
        while i < len(asteroids) and asteroids[i].angle == angle:
            i += 1

        if not i < len(asteroids):
            angle = asteroids[0].angle - 0.0001
            i = 0

        a = asteroids.pop(i)
        # print(map[a.y][a.x], a)
        yield a
        angle = a.angle

        if a.angle >= 360:
            angle = asteroids[0].angle - 0.0001
            i = 0


with open('input.txt') as f:
    map = [line.rstrip() for line in f.readlines()]

p = find_location(map)
asteroids = list(solve(map, p))
print(asteroids[199].x * 100 + asteroids[199].y)
