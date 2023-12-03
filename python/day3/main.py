from dataclasses import dataclass
from typing import List, Optional, cast
import string

class Space: pass

class SymSpace(Space): pass

@dataclass(frozen=True)
class NumSpace(Space):
    num: int

matrix: List[List[Space]] = list()
symbols = string.punctuation.replace('.', '')

def scanpos(y: int, x: int) -> Optional[NumSpace]:
    try:
        hit = matrix[y][x]
        if isinstance(hit, NumSpace): return hit
    except IndexError: pass
    return None

def scansym(y: int, x: int) -> List[NumSpace]:
    checks = [(y -1, x -1), (y -1, x), (y, x -1),
              (y +1, x +1), (y +1, x), (y, x +1),
              (y -1, x +1), (y +1, x -1)]
    return cast(List[NumSpace], list(filter(lambda x: x, map(lambda p: scanpos(*p), checks))))

with open('input.txt', 'rt') as f:
    number: str = ''
    # generate matrix
    for (y, line) in enumerate(f):
        matrix.append(list())
        for (x, char) in enumerate(line):
            if char.isdigit():
                # start of number
                number += char
            elif number:
                # end of number
                parsed = int(number)
                for _ in range(len(number)):
                    matrix[y].append(NumSpace(parsed))
                number = ''
            
            if char in symbols:
                matrix[y].append(SymSpace())
            elif char == '.':
                matrix[y].append(Space())

# scan matrix
acc = 0
for (y, line) in enumerate(matrix):
    for (x, space) in enumerate(line):
        if isinstance(space, SymSpace):
            print(f'sym: {y} {x}')
            for num in set(scansym(y, x)):
                acc += num.num

print(acc)
