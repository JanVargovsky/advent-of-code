from collections import namedtuple
import re
from itertools import combinations
from recordclass import recordclass
import numpy as np
from copy import deepcopy

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
            delta = np.sign(m2.pos[i] - m1.pos[i])
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

def check(initial, current, i):
    for m1, m2 in zip(initial, current):
        if m1.pos[i] != m2.pos[i] or m1.vel[i] != m2.vel[i]:
            return False
    return True

with open('input.txt') as f:
    moons = [Moon(parse(line), Coord3(0, 0, 0)) for line in f.readlines()]

initial_moons = deepcopy(moons)

step = 1
repeats = [None, None, None]
while None in repeats:
    apply_gravity(moons)
    apply_velocity(moons)
    for i in range(len(repeats)):
        if repeats[i] is None and check(initial_moons, moons, i):
            repeats[i] = step
    step += 1

print(repeats)
print(np.lcm.reduce(np.array(repeats, dtype=np.int64)))
