from functools import reduce
from itertools import accumulate, cycle, islice

with open('input.txt') as f:
    sequence = [int(d) for d in f.readline().rstrip()]


def apply_phase(sequence):
    new_sequence = []
    pattern = [1, 0, -1, 0]

    for offset in range(len(sequence)):
        d = 0
        for i in range(offset, len(sequence)):
            pattern_index = ((i - offset) // (offset + 1)) % len(pattern)
            # print(f"{sequence[i]}*{pattern[pattern_index]}", end=" ")
            d += sequence[i] * pattern[pattern_index]
        # print(f"={abs(d) % 10}")
        new_sequence.append(abs(d) % 10)

    return new_sequence


assert(apply_phase([1, 2, 3, 4, 5, 6, 7, 8]) == [4, 8, 2, 2, 6, 1, 5, 8])


def solve(sequence, phases):
    offset = reduce(lambda acc, d: acc * 10 + d, sequence[:7])
    length = len(sequence) * 10000 - offset
    sequence = islice(cycle(reversed(sequence)), length)

    for phase in range(phases):
        sequence = [d % 10 for d in accumulate(sequence)]

    return sequence[-8:][::-1]


assert(solve([0, 3, 0, 3, 6, 7, 3, 2, 5, 7, 7, 2, 1, 2, 9, 4, 4, 0, 6, 3,
              4, 9, 1, 5, 6, 5, 4, 7, 4, 6, 6, 4], 100) == [8, 4, 4, 6, 2, 0, 2, 6])
assert(solve([0, 2, 9, 3, 5, 1, 0, 9, 6, 9, 9, 9, 4, 0, 8, 0, 7, 4, 0, 7,
              5, 8, 5, 4, 4, 7, 0, 3, 4, 3, 2, 3], 100) == [7, 8, 7, 2, 5, 2, 7, 0])
assert(solve([0, 3, 0, 8, 1, 7, 7, 0, 8, 8, 4, 9, 2, 1, 9, 5, 9, 7, 3, 1,
              1, 6, 5, 4, 4, 6, 8, 5, 0, 5, 1, 7], 100) == [5, 3, 5, 5, 3, 7, 3, 1])

print(*(solve(sequence, 100)), sep="")
