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
    prev = False
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
            x = input()
            if x == -1:
                if prev:
                    yield -1
                prev = True
            else:
                prev = False
            memory[pointers[0]] = x
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


def send(destination, packet):
    global inputs
    try:
        for data in packet:
            print(f"sending {data} to {destination}")
            inputs[destination].put(data)
    except TypeError:
        print(f"sending {packet} to {destination}")
        inputs[destination].put(packet)


def receive(address):
    global inputs
    q = inputs[address]
    if q.empty():
        return -1
    data = q.get()
    print(f"{address} received {data}")
    return data


def initialize_nic(address, nic):
    q = Queue()
    q.put(address)
    computer = run(nic, lambda: receive(address))
    return q, computer


N = 50
inputs = []
nics = []
empty = []
for i in range(N):
    input, nic = initialize_nic(i, program.copy())
    inputs.append(input)
    nics.append(nic)
    empty.append(False)

nat = (None, None)
last_nat_y = None

while True:
    for i, nic in enumerate(nics):
        destination = next(nic, -1)
        if destination != -1:
            empty[i] = False
            x, y = next(nic), next(nic)
            if destination == 255:
                nat = (x, y)
            else:
                send(destination, [x, y])
        else:
            empty[i] = True
    
    idle = all(empty)
    if idle:
        if nat[1] == last_nat_y:
            print(y)
            break
        last_nat_y = nat[1]
        send(0, nat)
