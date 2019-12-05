with open('input.txt') as f:
    l = [int(i) for i in f.readlines()]
f = 0
fs = set()
fs.add(f)

while True:
    for i in l:
        f += i
        if f in fs:
            print(f)
        fs.add(f)
