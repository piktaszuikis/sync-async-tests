# Apie

Paprastas **nim** HTTP serveris, gražinantis tekstą "OK".

Panaudota [httpbeast](https://github.com/dom96/httpbeast) biblioteka, kuri pagal
[Web Framework Benchmarks](https://www.techempower.com/benchmarks/#section=data-r21&test=composite) yra 
greičiausia nim kalba parašyta web biblioteka.

HTTP portas: 3003.

# Paleidimas
1. Susiinstaliuoti nim (`pacman -S nim`)
2. `nimble c -r beast.nim --threads:on -d:danger`
