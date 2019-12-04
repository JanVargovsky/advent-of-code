count = 0
for i in range (172930, 683082):
    s = str(i)
    if int(s[0]) > int(s[1]) \
        or int(s[1]) > int(s[2]) \
        or int(s[2]) > int(s[3]) \
        or int(s[3]) > int(s[4]) \
        or int(s[4]) > int(s[5]):
        continue
    adjacent = False
    for a in range(5):
        if s[a] is s[a+1]:
            adjacent = True
            break
    if not adjacent:
        continue
    count += 1
print(count)
