def compute(memory):
    i = 0
    while memory[i] != 99:
        if memory[i] == 1:
            # add 1, 2 = positions, 3 = result position
            memory[memory[i + 3]] = memory[memory[i + 1]] + memory[memory[i + 2]]
            i += 4
        elif memory[i] == 2:
            memory[memory[i + 3]] = memory[memory[i + 1]] * memory[memory[i + 2]]
            i += 4
        else:
            i += 1
    return memory

assert(compute([1,9,10,3,2,3,11,0,99,30,40,50]) == [3500,9,10,70,2,3,11,0,99,30,40,50])
assert(compute([1,0,0,0,99]) == [2,0,0,0,99])
assert(compute([2,3,0,3,99]) == [2,3,0,6,99])
assert(compute([2,4,4,5,99,0]) == [2,4,4,5,99,9801])
assert(compute([1,1,1,4,99,5,6,0,99]) == [30,1,1,4,2,5,6,0,99])

with open('input.txt') as f:
    program = [int(i) for i in f.readline().split(',')]
    program[1] = 12
    program[2] = 2
    print(compute(program)[0])
