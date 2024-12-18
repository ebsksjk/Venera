# Intro
Das **M**anagementsytem **f**�r **S**ystensicherheit ist die Nutzerverwaltung von Venera.

Aufgrund des FAT32-Dateisystems ist eine Unix-artige Nutzerrechteverwaltung nicht m�glich. 
Daher wird die Nutzerverwaltung von Venera �ber eine eigene "Datenbank" realisiert.
In `0:\Venera\` liegt eine Datei namens "users.db", in der die Nutzerdaten gespeichert sind. Das Format ist hierbei wie folgt:
```
uid:username:name:password:home
```
Hierbei sind:
- UID eine eindeutige Nummer f�r jeden Benutzer.
- Username der Nutzername des Benutzers.
- Name der Klarname des Benutzers.
- Password das Passwort des Benutzers.
- Home der Pfad zum Home-Verzeichnis des Benutzers.

Home-Verzeichnisse liegen per default in `0:\Users\<username>\`.

Das MfS nutzt als rudiment�re Sicherheit, dass ein angemeldeter Nutzer nur Zugriff auf sein eigenes Home-Verzeichnis hat.
Zugriff auf andere Verzeichnisse wird verweigert.

# Befehle
## mfs.useradd
Mit dem Befehl `useradd` k�nnen neue Benutzer hinzugef�gt werden. Der Befehl hat folgende Syntax:
```
useradd -u <username> [-n <name>] 
```

Der Befehl fragt interaktiv nach einem Passwort f�r den neuen Benutzer.

## mfs.userdel
Mit dem Befehl `userdel` k�nnen Benutzer gel�scht werden. Der Befehl hat folgende Syntax:
```
userdel <username>
```

## mfs.usermod
Mit dem Befehl `usermod` k�nnen Benutzer bearbeitet werden. Der Befehl hat folgende Syntax:
```
usermod <username>
```

Jedes der Argumente ist hierbei optional.

## mfs.userinfo
Mit dem Befehl `userinfo` k�nnen Informationen �ber einen Benutzer abgerufen werden. Der Befehl hat folgende Syntax:
```
userinfo [<username>]
```

Wird kein Argument �bergeben, so wird der aktuelle Benutzer angezeigt.

# Klassen
## user.cs
Die Klasse `User` repr�sentiert einen Benutzer. Sie hat folgende Eigenschaften:
- `UID` die eindeutige Nummer des Benutzers.
- `Username` der Nutzername des Benutzers.
- `Name` der Klarname des Benutzers.
- `Password` das Passwort des Benutzers.
- `Home` der Pfad zum Home-Verzeichnis des Benutzers.
- `LastLogin` das Datum des letzten Logins des Benutzers.
- `db` der Pfad zur users.db.

Die Klasse hat folgende Methoden:
- `User(string username, string name, string password)` erstellt einen neuen Benutzer und speichert seine Informationen in der users.db.
- `User(int uid)` l�dt einen Benutzer aus der users.db.

## login.cs
Die Klasse `Login` repr�sentiert die Anmeldung. Sie hat folgende Eigenschaften:
- `User` der angemeldete Benutzer.
- `sesssionStart` die Zeit, zu der die Anmeldung begonnen hat.

Die Klasse hat folgende Methoden:
- `Login(string username, string password)` meldet einen Benutzer an.
- `loop()` f�hrt die Anmeldungsschleife aus.

Login l�uft in einer Ebene �berhalb von Sokolsh, so dass einem Nutzer beim Start des Systems ein
Anmeldefenster angezeigt wird.
Wird Sokolsh per `exit` beendet, so wird der Nutzer automatisch abgemeldet, und die 
Anmeldeaufforderung wird erneut angezeigt.