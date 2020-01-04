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


cards = 119315717514047
offset, increment = 0, 1
with open('input.txt') as f:
    for l in f.readlines():
        if l.startswith("deal into"):
            increment *= -1
            offset += increment
            offset %= cards
        elif l.startswith("cut "):
            n = int(l[len("cut "):])
            offset += increment * n
            offset %= cards
        elif l.startswith("deal with increment "):
            n = int(l[len("deal with increment "):])
            increment *= pow(n, cards - 2, cards)
            increment %= cards

n = 101741582076661
increment2 = pow(increment, n, cards)
offset2 = offset * (1 - increment2) * \
    pow((1 - increment) % cards, cards - 2, cards)
offset2 %= cards

card = 2020
card_index = offset2 + card * increment2
card_index %= cards

print(card_index)
