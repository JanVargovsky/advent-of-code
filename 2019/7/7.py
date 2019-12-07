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


def compute(memory, input):
    ip = 0
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


def compute_amplifiers(program, init_inputs):
    amplifiers = []
    inputs = []

    for i in range(5):
        inputs.append(Queue())
        inputs[i].put(init_inputs[i])
        amplifiers.append(compute(program.copy(), inputs[i]))
    inputs[0].put(0)

    while True:
        try:
            inputs[1].put(next(amplifiers[0]))
            inputs[2].put(next(amplifiers[1]))
            inputs[3].put(next(amplifiers[2]))
            inputs[4].put(next(amplifiers[3]))
            inputs[0].put(next(amplifiers[4]))
        except StopIteration:
            break
    return inputs[0].get()


assert(compute_amplifiers([3, 26, 1001, 26, -4, 26, 3, 27, 1002, 27, 2, 27, 1, 27, 26,
                           27, 4, 27, 1001, 28, -1, 28, 1005, 28, 6, 99, 0, 0, 5], [9, 8, 7, 6, 5]) == 139629729)
assert(compute_amplifiers([3, 52, 1001, 52, -5, 52, 3, 53, 1, 52, 56, 54, 1007, 54, 5, 55, 1005, 55, 26, 1001, 54,
                           -5, 54, 1105, 1, 12, 1, 53, 54, 53, 1008, 54, 0, 55, 1001, 55, 1, 55, 2, 53, 55, 53, 4,
                           53, 1001, 56, -1, 56, 1005, 56, 6, 99, 0, 0, 0, 0, 10], [9, 7, 8, 5, 6]) == 18216)

with open('input.txt') as f:
    program_original = [int(i) for i in f.readline().split(',')]


output = 0
for inputs in itertools.permutations([5, 6, 7, 8, 9]):
    program = program_original.copy()
    output = max(output, compute_amplifiers(program, inputs))

print(output)
