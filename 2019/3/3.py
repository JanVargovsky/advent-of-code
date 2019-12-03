from collections import namedtuple

Point = namedtuple('Point', ['x', 'y'])
Move = namedtuple('Move', ['direction', 'length'])

def manhattan_distance(p1: Point, p2: Point):
    return abs(p1.x - p2.x) + abs(p1.y - p2.y)

assert(manhattan_distance(Point(0, 0), Point(5, 5)) == 10)
assert(manhattan_distance(Point(-5, 5), Point(5, -5)) == 20)

def parse_input(text):
    return [Move(x[0], int(x[1:])) for x in text.split(',')]

assert(parse_input('R75,D30,R83') == [Move('R', 75), Move('D', 30), Move('R', 83)])

def build(moves):
    points = {}
    move_to_vec = {
        'R': Point(1, 0),
        'U': Point(0, 1),
        'L': Point(-1, 0),
        'D': Point(0, -1)
    }

    p = Point(0, 0)
    d = 0
    for m in moves:
        vec = move_to_vec[m.direction]
        for _ in range(m.length):
            points[p] = d
            p = Point(p.x + vec.x, p.y + vec.y)
            d += 1
    return points

def solve(w1, w2):
    p1 = build(w1)
    p2 = build(w2)
    p12 = p1.keys() & p2.keys()
    center = Point(0, 0)
    p12.remove(center)
    m = None
    for x in p12:
        if m is None or p1[x] + p2[x] < p1[m] + p2[m]:
            m = x
    steps = p1[m] + p2[m]
    return steps

assert(solve(parse_input("R8,U5,L5,D3"), parse_input("U7,R6,D4,L4")) == 30)
assert(solve(parse_input("R75,D30,R83,U83,L12,D49,R71,U7,L72"), parse_input("U62,R66,U55,R34,D71,R55,D58,R83")) == 610)
assert(solve(parse_input("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51"), parse_input("U98,R91,D20,R16,D67,R40,U7,R15,U6,R7")) == 410)

with open('input.txt') as f:
    wire1 = parse_input(f.readline())
    wire2 = parse_input(f.readline())

print(solve(wire1, wire2))