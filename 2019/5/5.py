from queue import Queue

opcodes = {
    # Opcode: arity
    99: 0, # halt
    1: 3, # add
    2: 3, # multiply
    3: 1, # input
    4: 1, # output
    5: 2, # jump if true
    6: 2, # jump if false
    7: 3, # less than
    8: 3, # equals
}

def get_mode(opcode, p):
    return opcode // 10**(2 + p) % 10

assert(get_mode(1002, 0) == 0)
assert(get_mode(1002, 1) == 1)
assert(get_mode(1002, 2) == 0)

def compute(memory):
    ip = 0
    input = Queue()
    input.put(5)
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
            values.append(memory[memory[ip + i]] if mode == 0 else memory[ip + i])
            pointers.append(memory[ip + i])
        ip += opcodes[opcode]

        if opcode == 1:
            memory[pointers[2]] = values[0] + values[1]
        elif opcode == 2:
            memory[pointers[2]] = values[0] * values[1]
        elif opcode == 3:
            memory[pointers[0]] = input.get()
        elif opcode == 4:
            print(values[0])
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
    return memory

assert(compute([1002,4,3,4,33]) == [1002,4,3,4,99])
assert(compute([1101,100,-1,4,0]) == [1101,100,-1,4,99])

with open('input.txt') as f:
    program = [int(i) for i in f.readline().split(',')]
compute(program)
