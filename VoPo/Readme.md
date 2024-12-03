# VoPo
Der Versatile Optimization and Prioritization Orchestrator ist die Prozesskontrolle von Venera.
Prozesse sind externe, ausführbare Dateien, die von Venera gestartet werden können. Interne Befehle werden aufgrund der Systemstruktur nicht als Prozesse gewertet.

Externe Programme werden als ELF-Dateien abgelegt, und können gelesen und ausgeführt werdenAufgrund der Limitierungen von CosmOS und der Schwierigkeit, Systemaufrufe zu implementieren, kann nur jeweils ein Prozess gleichzeitig laufen.
Einem Prozess werden die gesamten Systemressourcen während der Laufzeit zur Verfügung gestellt.
Dies bedeutet, dass ein Prozess nicht unterbrochen werden kann, bis er beendet ist.

