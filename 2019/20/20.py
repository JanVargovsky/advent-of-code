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

    return portals, inner, outer


def solve(m):
    p, inner_names, outer_names = find_portals(m)
    inner = set(inner_names.values())
    start = p["AA"]
    end = p["ZZ"]

    distances = defaultdict(lambda: 1e10)
    distances[(start, 0)] = 0
    q = Queue()
    q.put(([start], 0))
    directions = (Int2(1, 0), Int2(0, 1), Int2(-1, 0), Int2(0, -1))

    while not q.empty():
        (*path, current), level = q.get_nowait()
        if current == end and level == 0:
            return distances[(end, 0)]

        if current in p and path[-1] != p[current]:
            new = p[current]
            new_level = level + (1 if current in inner else -1)
            if new_level < 0 or new_level >= 30 or len(path) + 1 >= distances[(new, new_level)]:
                continue

            distances[(new, new_level)] = len(path) + 1
            q.put((path + [current, new], new_level))
        else:
            for direction in directions:
                new = Int2(current.x + direction.x, current.y + direction.y)
                if m[new.y][new.x] != ".":
                    continue
                if len(path) + 1 >= distances[(new, level)]:
                    continue
                distances[(new, level)] = len(path) + 1
                q.put((path + [current, new], level))

    return distances[(end, 0)]


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
             Z       """.splitlines()) == 26)
assert(solve("""             Z L X W       C                 
             Z P Q B       K                 
  ###########.#.#.#.#######.###############  
  #...#.......#.#.......#.#.......#.#.#...#  
  ###.#.#.#.#.#.#.#.###.#.#.#######.#.#.###  
  #.#...#.#.#...#.#.#...#...#...#.#.......#  
  #.###.#######.###.###.#.###.###.#.#######  
  #...#.......#.#...#...#.............#...#  
  #.#########.#######.#.#######.#######.###  
  #...#.#    F       R I       Z    #.#.#.#  
  #.###.#    D       E C       H    #.#.#.#  
  #.#...#                           #...#.#  
  #.###.#                           #.###.#  
  #.#....OA                       WB..#.#..ZH
  #.###.#                           #.#.#.#  
CJ......#                           #.....#  
  #######                           #######  
  #.#....CK                         #......IC
  #.###.#                           #.###.#  
  #.....#                           #...#.#  
  ###.###                           #.#.#.#  
XF....#.#                         RF..#.#.#  
  #####.#                           #######  
  #......CJ                       NM..#...#  
  ###.#.#                           #.###.#  
RE....#.#                           #......RF
  ###.###        X   X       L      #.#.#.#  
  #.....#        F   Q       P      #.#.#.#  
  ###.###########.###.#######.#########.###  
  #.....#...#.....#.......#...#.....#.#...#  
  #####.#.###.#######.#######.###.###.#.#.#  
  #.......#.......#.#.#.#.#...#...#...#.#.#  
  #####.###.#####.#.#.#.#.###.###.#.###.###  
  #.......#.....#.#...#...............#...#  
  #############.#.#.###.###################  
               A O F   N                     
               A A D   M                     """.splitlines()) == 396)


with open('input.txt') as f:
    maze = [line.rstrip("\n") for line in f.readlines()]

print(solve(maze))
