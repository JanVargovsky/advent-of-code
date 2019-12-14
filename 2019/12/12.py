from collections import namedtuple
import re
from itertools import combinations
from recordclass import recordclass
from numpy import sign

Coord3 = recordclass("Coord3", ["x", "y", "z"])
Moon = namedtuple("Moon", ["pos", "vel"])


def parse(line):
    x, y, z = map(int, re.findall(r"[-\d]+", line))
    return Coord3(x, y, z)


assert(parse("<x=-1, y=0, z=2>") == Coord3(-1, 0, 2))
assert(parse("<x=2, y=-10, z=-7>") == Coord3(2, -10, -7))


def apply_gravity(moons):
    for m1, m2 in combinations(moons, 2):
        for i in range(len(m1.pos)):
            delta = sign(m2.pos[i] - m1.pos[i])
            m1.vel[i] += delta
            m2.vel[i] -= delta

def apply_velocity(moons):
    for m in moons:
        for i in range(len(m.pos)):
            m.pos[i] += m.vel[i]

def moon_energy(moon):
    return sum(map(abs, moon.pos)) * sum(map(abs, moon.vel)) 

def total_energy(moons):
    return sum(map(moon_energy, moons))


with open('input.txt') as f:
    moons = [Moon(parse(line), Coord3(0, 0, 0)) for line in f.readlines()]

print(*moons, sep="\n")
for _ in range(1000):
    apply_gravity(moons)
    apply_velocity(moons)
print(*moons, sep="\n")
print(total_energy(moons))
