def calculate_fuel(mass):
    sum = 0
    while mass > 0:
        mass = max(0, mass // 3 - 2)
        sum += mass
    return sum

assert(calculate_fuel(12) == 2)
assert(calculate_fuel(14) == 2)
assert(calculate_fuel(1969) == 966)
assert(calculate_fuel(100756) == 50346)

with open('input.txt') as f:
    print(sum(map(lambda l: calculate_fuel(int(l)), f.readlines())))
