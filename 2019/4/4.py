def password(i):
    s = str(i)
    for i in range(5):
        if s[i] > s[i+1]:
            return False
    
    i = 0
    while i < 5:
        start = i
        while i < 5 and s[i] == s[i + 1]:
            i += 1
        if (i - start + 1) == 2:
            return True
        i += 1

    return False

# assert(password("111111") == True)
assert(password("223450") == False)
assert(password("123789") == False)

assert(password("112233") == True)
assert(password("123444") == False)
assert(password("111122") == True)

count = 0
for i in range(172930, 683082):
    if password(i):
        count += 1
print(count)
