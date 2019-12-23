from collections import namedtuple, defaultdict
from itertools import chain
from queue import Queue

Int2 = namedtuple("Int2", ["x", "y"])


def find_inner_corners(m):
    horizontal = m[len(m) // 2]
    vertical = "".join([m[y][len(m[0]) // 2] for y in range(len(m))])

    left = horizontal.index(' ', 2)
    if horizontal[left - 1] not in ".#":
        left -= 2

    right = horizontal.rindex(' ', 2, len(horizontal) - 2)
    if horizontal[right + 1] not in ".#":
        right += 2

    top = vertical.index(' ', 2)
    if vertical[top - 1] not in ".#":
        top -= 2

    bottom = vertical.rindex(' ', 2, len(vertical) - 2)
    if vertical[bottom + 1] not in ".#":
        bottom += 2

    return top, right, bottom, left


assert(find_inner_corners("""         A           
         A           
  #######.#########  
  #######.........#  
  #######.#######.#  
  #######.#######.#  
  #######.#######.#  
  #####  B    ###.#  
BC...##  C    ###.#  
  ##.##       ###.#  
  ##...DE  F  ###.#  
  #####    G  ###.#  
  #########.#####.#  
DE..#######...###.#  
  #.#########.###.#  
FG..#########.....#  
  ###########.#####  
             Z       
             Z       """.splitlines()) == (7, 13, 11, 7))


def find_vertical_portals(m, x, start, end):
    for y in range(start, end + 1):
        if m[y][x] == '.':
            yield Int2(x, y)


def find_horizontal_portals(m, y, start, end):
    for x in range(start, end + 1):
        if m[y][x] == '.':
            yield Int2(x, y)


def find_portals(m):
    top, right, bottom, left = find_inner_corners(m)
    outer = {}
    inner = {}

    # outer left
    for p in find_vertical_portals(m, 2, 3, len(m) - 3):
        outer[m[p.y][:2]] = p
    # outer right
    for p in find_vertical_portals(m, len(m[0]) - 3, 3, len(m) - 3):
        outer[m[p.y][-2:]] = p
    # outer top
    for p in find_horizontal_portals(m, 2, 3, len(m[0]) - 3):
        outer[m[0][p.x] + m[1][p.x]] = p
    # outer bottom
    for p in find_horizontal_portals(m, len(m) - 3, 3, len(m[0]) - 3):
        outer[m[len(m) - 2][p.x] + m[len(m) - 1][p.x]] = p

    # inner left
    for p in find_vertical_portals(m, left-1, top, bottom):
        inner[m[p.y][p.x+1:p.x+3]] = p
    # inner right
    for p in find_vertical_portals(m, right+1, top, bottom):
        inner[m[p.y][p.x-2:p.x]] = p
    # inner top
    for p in find_horizontal_portals(m, top-1, left, right):
        inner[m[p.y+1][p.x] + m[p.y+2][p.x]] = p
    # inner bottom
    for p in find_horizontal_portals(m, bottom+1, left, right):
        inner[m[p.y-2][p.x] + m[p.y-1][p.x]] = p

    portals = {}
    for name in inner.keys():
        portals[outer[name]] = inner[name]
        portals[inner[name]] = outer[name]
    portals["AA"] = outer["AA"]
    portals["ZZ"] = outer["ZZ"]

    return portals


def solve(m):
    p = find_portals(m)
    start = p["AA"]
    end = p["ZZ"]

    distances = defaultdict(lambda: 1e10)
    distances[start] = 0
    s = [[start]]
    directions = (Int2(1, 0), Int2(0, 1), Int2(-1, 0), Int2(0, -1))

    while s:
        *path, current = s.pop()

        if current in p and path[-1] != p[current]:
            new = p[current]
            if len(path) + 1 >= distances[new]:
                continue
            distances[new] = len(path) + 1
            s.append(path + [current, new])
        else:
            for direction in directions:
                new = Int2(current.x + direction.x, current.y + direction.y)
                if m[new.y][new.x] != ".":
                    continue
                if len(path) + 1 >= distances[new]:
                    continue
                distances[new] = len(path) + 1
                s.append(path + [current, new])

    return distances[end]


with open('input.txt') as f:
    maze = [line.rstrip("\n") for line in f.readlines()]

assert(solve("""         A           
         A           
  #######.#########  
  #######.........#  
  #######.#######.#  
  #######.#######.#  
  #######.#######.#  
  #####  B    ###.#  
BC...##  C    ###.#  
  ##.##       ###.#  
  ##...DE  F  ###.#  
  #####    G  ###.#  
  #########.#####.#  
DE..#######...###.#  
  #.#########.###.#  
FG..#########.....#  
  ###########.#####  
             Z       
             Z       """.splitlines()) == 23)
assert(solve("""                   A               
                   A               
  #################.#############  
  #.#...#...................#.#.#  
  #.#.#.###.###.###.#########.#.#  
  #.#.#.......#...#.....#.#.#...#  
  #.#########.###.#####.#.#.###.#  
  #.............#.#.....#.......#  
  ###.###########.###.#####.#.#.#  
  #.....#        A   C    #.#.#.#  
  #######        S   P    #####.#  
  #.#...#                 #......VT
  #.#.#.#                 #.#####  
  #...#.#               YN....#.#  
  #.###.#                 #####.#  
DI....#.#                 #.....#  
  #####.#                 #.###.#  
ZZ......#               QG....#..AS
  ###.###                 #######  
JO..#.#.#                 #.....#  
  #.#.#.#                 ###.#.#  
  #...#..DI             BU....#..LF
  #####.#                 #.#####  
YN......#               VT..#....QG
  #.###.#                 #.###.#  
  #.#...#                 #.....#  
  ###.###    J L     J    #.#.###  
  #.....#    O F     P    #.#...#  
  #.###.#####.#.#####.#####.###.#  
  #...#.#.#...#.....#.....#.#...#  
  #.#####.###.###.#.#.#########.#  
  #...#.#.....#...#.#.#.#.....#.#  
  #.###.#####.###.###.#.#.#######  
  #.#.........#...#.............#  
  #########.###.###.#############  
           B   J   C               
           U   P   P               """.splitlines()) == 58)

print(solve(maze))
