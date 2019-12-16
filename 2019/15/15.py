from queue import Queue

opcodes = {
    # Opcode: arity
    99: 0,  # halt
    1: 3,  # add
    2: 3,  # multiply
    3: 1,  # input
    4: 1,  # output
    5: 2,  # jump if true
    6: 2,  # jump if false
    7: 3,  # less than
    8: 3,  # equals
    9: 1,  # relative base offset
}


def get_mode(opcode, p):
    return opcode // 10**(2 + p) % 10


assert(get_mode(1002, 0) == 0)
assert(get_mode(1002, 1) == 1)
assert(get_mode(1002, 2) == 0)


def load_parameters(memory, ip, raw, opcode, offset):
    values = []
    pointers = []
    for i in range(opcodes[opcode]):
        mode = get_mode(raw, i)
        if mode == 0:  # position mode
            values.append(memory[memory[ip + i]])
        elif mode == 1:  # immediate mode
            values.append(memory[ip + i])
        elif mode == 2:  # relative mode
            values.append(memory[offset + memory[ip + i]])
        pointers.append(
            offset + memory[ip + i] if mode == 2 else memory[ip + i])
    return values, pointers


def run(memory, input):
    memory += [0] * 10000
    ip = 0
    offset = 0
    while True:
        raw = memory[ip]
        opcode = raw % 100
        if opcode == 99:
            break
        ip += 1
        values, pointers = load_parameters(memory, ip, raw, opcode, offset)
        ip += opcodes[opcode]

        if opcode == 1:
            memory[pointers[2]] = values[0] + values[1]
        elif opcode == 2:
            memory[pointers[2]] = values[0] * values[1]
        elif opcode == 3:
            memory[pointers[0]] = input()
        elif opcode == 4:
            # print(values[0])
            yield values[0]
        elif opcode == 5:
            if values[0] != 0:
                ip = values[1]
        elif opcode == 6:
            if values[0] == 0:
                ip = values[1]
        elif opcode == 7:
            memory[pointers[2]] = 1 if values[0] < values[1] else 0
        elif opcode == 8:
            memory[pointers[2]] = 1 if values[0] == values[1] else 0
        elif opcode == 9:
            offset += values[0]
        else:
            raise Exception()


with open('input.txt') as f:
    program = [int(i) for i in f.readline().split(',')]

"""
 N
W E
 S
"""
NORTH = 1
SOUTH = 2
WEST = 3
EAST = 4

DIRECTION_NAMES = [
    None,
    "North ^",
    "South v",
    "West <",
    "East >"
]

WALL = 0
EMPTY = 1
OXYGEN = 2


def print_map(map, droid_x, droid_y, min_x, max_x, min_y, max_y):
    output = []
    for y in range(min_y - 1, max_y + 2):
        for x in range(min_x - 1, max_x + 2):
            if x == 0 and y == 0:
                symbol = "S"
            elif x == droid_x and y == droid_y:
                symbol = "D"
            elif not (x, y) in map:
                symbol = " "
            else:
                status = map[(x, y)]
                if status == WALL:
                    symbol = "#"
                elif status == EMPTY:
                    symbol = "."
                elif status == OXYGEN:
                    symbol = "o"
            output.append(symbol)
        output.append("\n")
    print(*output, sep="")


def new_position(movement, x, y):
    if movement == NORTH:
        return (x, y - 1)
    elif movement == SOUTH:
        return (x, y + 1)
    elif movement == WEST:
        return (x - 1, y)
    elif movement == EAST:
        return (x + 1, y)


def opposite_movement(movement):
    if movement == NORTH:
        return SOUTH
    elif movement == SOUTH:
        return NORTH
    elif movement == WEST:
        return EAST
    elif movement == EAST:
        return WEST


def solve(program):
    q = Queue()
    x = 0
    y = 0
    map = {}
    movements = []
    movements.append((SOUTH, False))
    movements.append((NORTH, False))
    movements.append((EAST, False))
    movements.append((WEST, False))

    droid = run(program, q.get)
    min_x = -1
    min_y = -1
    max_x = 1
    max_y = 1
    i = 0
    l = 0
    while len(movements) > 0:
        min_x = min(x, min_x)
        max_x = max(x, max_x)
        min_y = min(y, min_y)
        max_y = max(y, max_y)
        i += 1
        print(i, l)
        # print_map(map, x, y, min_x, max_x, min_y, max_y)
        movement, backtrack = movements.pop()

        if not backtrack and new_position(movement, x, y) in map:
            continue

        # print(f"moving {DIRECTION_NAMES[movement]}")
        q.put(movement)
        output = next(droid)
        map[(x, y)] = EMPTY
        if output == OXYGEN:
            print_map(map, x, y, min_x, max_x, min_y, max_y)
            return l + 1
        elif output == WALL:
            # print("wall")
            map[new_position(movement, x, y)] = WALL
        elif output == EMPTY:
            x, y = new_position(movement, x, y)
            op = opposite_movement(movement)

            if backtrack:
                l -= 1
                continue
            else:
                l += 1
            # print(f"moved {DIRECTION_NAMES[movement]}")

            movements.append((op, True))  # backtrack
            movements.append((NORTH, False))
            movements.append((SOUTH, False))
            movements.append((WEST, False))
            movements.append((EAST, False))


print(solve(program))
