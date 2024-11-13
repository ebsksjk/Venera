# How to compile for Venera (just sticking this here to make you aware)
```bash
# assuming you have your files and the linker script in the same directory
gcc -c [files.c] -o out.o -masm=intel -m32 -std=gnu99 -fno-builtin -ffreestanding -nostartfiles -nostdlib -fno-stack-protector -static -fno-pic -fno-pie
# the last two are important, as relocation modes are not implemented entirely yet
ld -m elf_i386 -r -T [Venera.ld] out.o -o [output]
# now the output elf needs to be included in /isoFiles
```_