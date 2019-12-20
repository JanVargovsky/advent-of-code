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


def is_beam(x, y):
    input = [y, x]
    intcode = run(program.copy(), input.pop)
    return next(intcode)


x = 99
y = 0
while True:
    x += 1
    while not is_beam(x, y):
        y += 1
    if is_beam(x - 99, y) and is_beam(x - 99, y + 99):
        break

print((x - 99) * 10000 + y)
