import re
from collections import namedtuple
from recordclass import recordclass
import math

Reaction = namedtuple("Reaction", ["inputs", "output"])
Chemical = namedtuple("Chemical", ["quantity", "name"])


def parse(line):
    *inputs, output = [Chemical(int(q), n)
                       for q, n in re.findall(r"(\d+) (\w+)", line)]
    return Reaction(inputs, output)


assert(parse("7 A, 1 E => 1 FUEL") == Reaction(
    [Chemical(7, "A"), Chemical(1, "E")], Chemical(1, "FUEL")))

with open('input.txt') as f:
    reactions = {}
    for line in f.readlines():
        r = parse(line)
        reactions[r.output.name] = r


def solve(reactions):
    needs = [Chemical(1, "FUEL")]
    waste = {name: 0 for name in reactions.keys()}
    ore = 0

    while len(needs) > 0:
        # print(*needs)
        new_needs = []
        for need in needs:
            if need.name == "ORE":
                ore += need.quantity
            else:
                in_waste = waste[need.name]
                if in_waste >= need.quantity:
                    waste[need.name] -= need.quantity
                    continue

                waste[need.name] = 0
                r = reactions[need.name]
                real_need = need.quantity - in_waste
                q = math.ceil(real_need / r.output.quantity)

                new_waste = q * r.output.quantity - real_need
                waste[r.output.name] += new_waste
                for c in r.inputs:
                    new_needs.append(Chemical(c.quantity * q, c.name))

        needs = new_needs

    return ore


# print(*reactions, sep="\n")
print(solve(reactions))
