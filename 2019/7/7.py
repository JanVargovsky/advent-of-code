from queue import Queue
import itertools

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
}


def get_mode(opcode, p):
    return opcode // 10**(2 + p) % 10


assert(get_mode(1002, 0) == 0)
assert(get_mode(1002, 1) == 1)
assert(get_mode(1002, 2) == 0)


def compute(memory, inputs):
    ip = 0
    input = Queue()
    for i in inputs:
        input.put(i)
    while True:
        raw = memory[ip]
        opcode = raw % 100
        if opcode == 99:
            break
        ip += 1
        values = []
        pointers = []
        for i in range(opcodes[opcode]):
            mode = get_mode(raw, i)
            values.append(memory[memory[ip + i]]
                          if mode == 0 else memory[ip + i])
            pointers.append(memory[ip + i])
        ip += opcodes[opcode]

        if opcode == 1:
            memory[pointers[2]] = values[0] + values[1]
        elif opcode == 2:
            memory[pointers[2]] = values[0] * values[1]
        elif opcode == 3:
            memory[pointers[0]] = input.get()
        elif opcode == 4:
            output = values[0]
            # print(values[0])
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
    return output


def compute_amplifiers(program, inputs):
    output = 0
    for input in inputs:
        output = compute(program.copy(), [input, output])
    return output


assert(compute_amplifiers([3, 15, 3, 16, 1002, 16, 10, 16,
                           1, 16, 15, 15, 4, 15, 99, 0, 0], [4, 3, 2, 1, 0]) == 43210)
assert(compute_amplifiers([3, 23, 3, 24, 1002, 24, 10, 24, 1002, 23, -1, 23,
                           101, 5, 23, 23, 1, 24, 23, 23, 4, 23, 99, 0, 0], [0, 1, 2, 3, 4]) == 54321)
assert(compute_amplifiers([3, 31, 3, 32, 1002, 32, 10, 32, 1001, 31, -2, 31, 1007, 31, 0, 33,
                           1002, 33, 7, 33, 1, 33, 31, 31, 1, 32, 31, 31, 4, 31, 99, 0, 0, 0], [1, 0, 4, 3, 2]) == 65210)

with open('input.txt') as f:
    program_original = [int(i) for i in f.readline().split(',')]


output = 0
for inputs in itertools.permutations([0, 1, 2, 3, 4]):
    program = program_original.copy()
    output = max(output, compute_amplifiers(program, inputs))

print(output)
