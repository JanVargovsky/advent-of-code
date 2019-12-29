def do_stack(cards):
    cards.reverse()
    return cards


assert(do_stack([0, 1, 2, 3, 4, 5, 6, 7, 8, 9])
       == [9, 8, 7, 6, 5, 4, 3, 2, 1, 0])


def do_cut(cards, n):
    return cards[n:] + cards[:n]


assert(do_cut([0, 1, 2, 3, 4, 5, 6, 7, 8, 9], 3)
       == [3, 4, 5, 6, 7, 8, 9, 0, 1, 2])
assert(do_cut([0, 1, 2, 3, 4, 5, 6, 7, 8, 9], -4)
       == [6, 7, 8, 9, 0, 1, 2, 3, 4, 5])


def do_increment(cards, n):
    new_cards = [-1] * len(cards)
    i = 0
    j = 0
    while i < len(cards):
        new_cards[j] = cards[i]
        i += 1
        j += n
        j %= len(cards)
    return new_cards


assert(do_increment([0, 1, 2, 3, 4, 5, 6, 7, 8, 9], 3)
       == [0, 7, 4, 1, 8, 5, 2, 9, 6, 3])


cards = list(range(10007))
with open('input.txt') as f:
    for l in f.readlines():
        if l.startswith("deal into"):
            cards = do_stack(cards)
        elif l.startswith("cut "):
            cards = do_cut(cards, int(l[len("cut "):]))
        elif l.startswith("deal with increment "):
            cards = do_increment(cards, int(l[len("deal with increment "):]))

print("Result: ", " ".join(map(str, cards)))
print(cards.index(2019))
