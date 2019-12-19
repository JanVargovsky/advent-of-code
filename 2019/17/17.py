from collections import namedtuple
from queue import Queue
import re

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


Int2 = namedtuple("Int2", ["x", "y"])


def find_robot(camera):
    for y in range(len(camera)):
        for x in range(len(camera[y])):
            if camera[y][x] in '^v<>':
                return Int2(x, y)


def is_valid_position(camera, position):
    return position.x >= 0 and position.y >= 0 and position.y < len(camera) and position.x < len(camera[position.y])


def find_direction(camera, robot, directions, visited):
    for i, direction in enumerate(directions):
        position = Int2(robot.x + direction.x, robot.y + direction.y)
        if is_valid_position(camera, position) and \
                camera[position.y][position.x] == "#" and \
                not position in visited:
            return i, direction

    return None, None


def find_path(camera):
    robot = find_robot(camera)
    directions = [
        Int2(1, 0),  # >
        Int2(0, 1),  # v
        Int2(-1, 0),  # <
        Int2(0, -1)  # ^
    ]
    visited = set([robot])
    direction_index = ">v<^".index(camera[robot.y][robot.x])
    path = []

    while True:
        new_direction_index, direction = find_direction(
            camera, robot, directions, visited)
        if not direction:
            break

        if directions[direction_index] == directions[new_direction_index - 1]:
            path.append("R")
        elif directions[direction_index - 1] == directions[new_direction_index]:
            path.append("L")
        direction_index = new_direction_index

        i = 0
        while True:
            new_robot = Int2(robot.x + direction.x, robot.y + direction.y)
            if not is_valid_position(camera, new_robot) or \
                    camera[new_robot.y][new_robot.x] != "#":
                path.append(str(i))
                break
            i += 1
            # path.append("F")
            robot = new_robot
            visited.add(robot)

    return "".join(path)


assert(find_path("""#######...#####
#.....#...#...#
#.....#...#...#
......#...#...#
......#...###.#
......#.....#.#
^########...#.#
......#.#...#.#
......#########
........#...#..
....#########..
....#...#......
....#...#......
....#...#......
....#####......""".splitlines()) == "R,8,R,8,R,4,R,4,R,8,L,6,L,2,R,4,R,4,R,8,R,8,R,8,L,6,L,2".replace(",", ""))


def compress(text):
    for a in range(4, 20):
        for b in range(4, 20):
            for c in range(4, 20):
                ra = text[:a]
                compressed = text.replace(ra, "")
                rb = compressed[:b]
                compressed = compressed.replace(rb, "")
                rc = compressed[:c]
                compressed = compressed.replace(rc, "")

                if len(compressed) == 0:
                    compressed = text.replace(ra, "A").replace(
                        rb, "B").replace(rc, "C")
                    if all(c in "ABC" for c in compressed):
                        return compressed, ra, rb, rc

    raise Exception()


assert(compress('L4R8L6L10L6R8R10L6L6L4R8L6L10L6R8R10L6L6L4L4L10L4L4L10L6R8R10L6L6L4R8L6L10L6R8R10L6L6L4L4L10') ==
       ("ABABCCBABC", "L4R8L6L10", "L6R8R10L6L6", "L4L4L10"))


def to_commands(command):
    return ",".join(re.split("(?<=\\d)(?=\\D)|(?=\\d)(?<=\\D)", command))


assert(to_commands("L1R2L10R10") == "L,1,R,2,L,10,R,10")


def readlines(it):
    result = []
    for c in map(chr, it):
        if c == "\n":
            line = "".join(result)
            print("output:", line)
            yield line
            result = []
        else:
            result.append(c)
    if result:
        line = "".join(result)
        print("output:", line)
        yield line


def send(data):
    global input
    print("input:", data)
    if data is int:
        input.put(data)
    else:
        for d in data:
            input.put(ord(d))
    input.put(ord("\n"))


program[0] = 2
camera = []
input = Queue()
intcode = readlines(run(program, input.get))
while True:
    line = next(intcode)
    if line == "":
        break
    camera.append(line)

path = find_path(camera)
print("path:", path)
main, function_a, function_b, function_c = compress(path)
main = ",".join(main)
function_a = to_commands(function_a)
function_b = to_commands(function_b)
function_c = to_commands(function_c)
assert(next(intcode) == "Main:")
send(main)
assert(next(intcode) == "Function A:")
send(function_a)
assert(next(intcode) == "Function B:")
send(function_b)
assert(next(intcode) == "Function C:")
send(function_c)
assert(next(intcode) == "Continuous video feed?")
send("n")

*_, dust = intcode
print("dust:", ord(dust))
