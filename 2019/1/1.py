def calculate_fuel(mass):
    return mass // 3 - 2

assert(calculate_fuel(12) == 2)
assert(calculate_fuel(14) == 2)
assert(calculate_fuel(1969) == 654)
assert(calculate_fuel(100756) == 33583)

with open('input.txt') as f:
    print(sum(map(lambda l: calculate_fuel(int(l)), f.readlines())))
