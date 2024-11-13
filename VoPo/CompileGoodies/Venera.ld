ENTRY(main)
OUTPUT_FORMAT(elf32-i386)
OUTPUT_ARCH(i386)

SECTIONS {
	. = 0x0;
	.text : {*(.text*)}
	.data : {*(.data*)}
	.bss : {*(.bss*)}
	/DISCARD/ : {*(.comment) *(.note*) *(.eh_frame)}
}