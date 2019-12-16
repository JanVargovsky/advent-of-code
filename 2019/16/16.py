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
    for phase in range(phases):
        sequence = apply_phase(sequence)
    return sequence[:8]


assert(solve([1, 2, 3, 4, 5, 6, 7, 8], 2) == [3, 4, 0, 4, 0, 4, 3, 8])
assert(solve([1, 2, 3, 4, 5, 6, 7, 8], 3) == [0, 3, 4, 1, 5, 5, 1, 8])
assert(solve([1, 2, 3, 4, 5, 6, 7, 8], 4) == [0, 1, 0, 2, 9, 4, 9, 8])
assert(solve([8, 0, 8, 7, 1, 2, 2, 4, 5, 8, 5, 9, 1, 4, 5, 4, 6, 6, 1, 9,
              0, 8, 3, 2, 1, 8, 6, 4, 5, 5, 9, 5], 100) == [2, 4, 1, 7, 6, 1, 7, 6])


print(*(solve(sequence, 100)), sep="")
