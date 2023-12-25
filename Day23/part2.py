from sympy import symbols, Eq, solve

kox, koy, koz, kovx, kovy, kovz, t0, t1, t2 = symbols('kox koy koz kovx kovy kovz t0 t1 t2')

eq1 = Eq(kox + t0 * kovx, 200027938836082 + t0 * 133)
eq2 = Eq(koy + t0 * kovy, 135313515251542 + t0 * 259)
eq3 = Eq(koz + t0 * kovz, 37945458137479 + t0 * 506)

eq4 = Eq(kox + t1 * kovx, 285259862606823 + t1 * 12)
eq5 = Eq(koy + t1 * kovy, 407476720802151 + t1 * -120)
eq6 = Eq(koz + t1 * kovz, 448972585175416 + t1 * -241)

eq7 = Eq(kox + t2 * kovx, 329601664688534 + t2 * -133)
eq8 = Eq(koy + t2 * kovy, 370686722303193 + t2 * -222)
eq9 = Eq(koz + t2 * kovz, 178908568819244 + t2 * 168)

solution = solve([eq1, eq2, eq3, eq4, eq5, eq6, eq7, eq8, eq9])

print(solution)