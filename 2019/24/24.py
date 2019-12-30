size = 5

def index(x, y):
    return y * size + x

def is_valid(x, y):
    return x >= 0 and y >= 0 and x < size and y < size

adjacent_tiles = [(1, 0), (0, 1), (-1, 0), (0, -1)]

def adjacent_bugs(grid, x, y):
    bug = 0
    for adjacent in adjacent_tiles:
        nx, ny = x + adjacent[0], y + adjacent[1]
        if is_valid(nx, ny) and grid[index(nx, ny)] == "#":
            bug += 1
    return bug


def tick(grid):
    new_grid = []
    for y in range(size):
        for x in range(size):
            if grid[index(x, y)] == "#" and adjacent_bugs(grid, x, y) != 1:
                new_grid.append(".") # dies
            elif grid[index(x, y)] == "." and adjacent_bugs(grid, x, y) in [1,2]:
                new_grid.append("#") # infested
            else:
                new_grid.append(grid[index(x, y)]) # remains
    return new_grid

def calculate_biodiversity_rating(grid):
    return sum([2 ** i for i, value in enumerate(grid) if value == "#"])
                
grid = []
with open('input.txt') as f:
    for l in f.readlines():
        grid += list(l.rstrip())

biodiversity_ratings = set()

while True:
    for i in range(size):
        print("".join(grid[i * size: i * size + size]))
    print("")
    biodiversity_rating = calculate_biodiversity_rating(grid)
    if biodiversity_rating in biodiversity_ratings:
        print(biodiversity_rating)
        break
    biodiversity_ratings.add(biodiversity_rating)
    grid = tick(grid)
