# VoPo
Der Versatile Optimization and Prioritization Orchestrator ist die Prozesskontrolle von Venera.
Prozesse sind externe, ausf�hrbare Dateien, die von Venera gestartet werden k�nnen. Interne Befehle werden aufgrund der Systemstruktur nicht als Prozesse gewertet.

Externe Programme werden als ELF-Dateien abgelegt, und k�nnen gelesen und ausgef�hrt werdenAufgrund der Limitierungen von CosmOS und der Schwierigkeit, Systemaufrufe zu implementieren, kann nur jeweils ein Prozess gleichzeitig laufen.
Einem Prozess werden die gesamten Systemressourcen w�hrend der Laufzeit zur Verf�gung gestellt.
Dies bedeutet, dass ein Prozess nicht unterbrochen werden kann, bis er beendet ist.

