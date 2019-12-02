def compute(memory):
    ip = 0
    while memory[ip] != 99:
        if memory[ip] == 1:
            # add 1, 2 = positions, 3 = result position
            memory[memory[ip + 3]] = memory[memory[ip + 1]] + memory[memory[ip + 2]]
            ip += 4
        elif memory[ip] == 2:
            memory[memory[ip + 3]] = memory[memory[ip + 1]] * memory[memory[ip + 2]]
            ip += 4
        else:
            ip += 1
    return memory

assert(compute([1,9,10,3,2,3,11,0,99,30,40,50]) == [3500,9,10,70,2,3,11,0,99,30,40,50])
assert(compute([1,0,0,0,99]) == [2,0,0,0,99])
assert(compute([2,3,0,3,99]) == [2,3,0,6,99])
assert(compute([2,4,4,5,99,0]) == [2,4,4,5,99,9801])
assert(compute([1,1,1,4,99,5,6,0,99]) == [30,1,1,4,2,5,6,0,99])

with open('input.txt') as f:
    program_original = [int(i) for i in f.readline().split(',')]

for noun in range(0, 100):
    for verb in range(0, 100):
        program = program_original.copy()
        program[1] = noun
        program[2] = verb
        if compute(program)[0] == 19690720:
            print(100 * noun + verb)
