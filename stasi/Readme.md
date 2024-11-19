# Intro
Das **S**ystem zur **T**ransparenz und **A**dministration von **S**icherheits- und **I**dentitätsdaten ist die Nutzerverwaltung von Venera.

Aufgrund des FAT32-Dateisystems ist eine Unix-artige Nutzerrechteverwaltung nicht möglich. 
Daher wird die Nutzerverwaltung von Venera über eine eigene "Datenbank" realisiert.
In `0:\Venera\` liegt eine Datei namens "users.db", in der die Nutzerdaten gespeichert sind. Das Format ist hierbei wie folgt:
```
uid:username:name:password:home
```
Hierbei sind:
- UID eine eindeutige Nummer für jeden Benutzer.
- Username der Nutzername des Benutzers.
- Name der Klarname des Benutzers.
- Password das Passwort des Benutzers.
- Home der Pfad zum Home-Verzeichnis des Benutzers.

Home-Verzeichnisse liegen per default in `0:\Users\<username>\`.

Das S.T.A.S.I nutzt als rudimentäre Sicherheit, dass ein angemeldeter Nutzer nur Zugriff auf sein eigenes Home-Verzeichnis hat.
Zugriff auf andere Verzeichnisse wird verweigert.

# Befehle
## stasi.useradd
Mit dem Befehl `useradd` können neue Benutzer hinzugefügt werden. Der Befehl hat folgende Syntax:
```
useradd -u <username> [-n <name>] 
```

Der Befehl fragt interaktiv nach einem Passwort für den neuen Benutzer.

## stasi.userdel
Mit dem Befehl `userdel` können Benutzer gelöscht werden. Der Befehl hat folgende Syntax:
```
userdel <uid>
```

## stasi.usermod
Mit dem Befehl `usermod` können Benutzer bearbeitet werden. Der Befehl hat folgende Syntax:
```
usermod <uid> [-u <username> -n <name> -p <password> -h <home>]
```

Jedes der Argumente ist hierbei optional.

## stasi.uid
Mit dem Befehl `uid` können Informationen über einen Benutzer abgerufen werden. Der Befehl hat folgende Syntax:
```
uid [<username>]
```

Wird kein Argument übergeben, so wird der aktuelle Benutzer angezeigt.

# Klassen
## user.cs
Die Klasse `User` repräsentiert einen Benutzer. Sie hat folgende Eigenschaften:
- `UID` die eindeutige Nummer des Benutzers.
- `Username` der Nutzername des Benutzers.
- `Name` der Klarname des Benutzers.
- `Password` das Passwort des Benutzers.
- `Home` der Pfad zum Home-Verzeichnis des Benutzers.
- `LastLogin` das Datum des letzten Logins des Benutzers.
- `	db` der Pfad zur users.db.

Die Klasse hat folgende Methoden:
- `User(string username, string name, string password)` erstellt einen neuen Benutzer und speichert seine Informationen in der users.db.
- `User(int uid)` lädt einen Benutzer aus der users.db.

## login.cs
Die Klasse `Login` repräsentiert die Anmeldung. Sie hat folgende Eigenschaften:
- `User` der angemeldete Benutzer.
- `sesssionStart` die Zeit, zu der die Anmeldung begonnen hat.

Die Klasse hat folgende Methoden:
- `Login(string username, string password)` meldet einen Benutzer an.
- `Logout()` meldet den Benutzer ab.

Login läuft in einer Ebene überhalb von Sokolsh, so dass einem Nutzer beim Start des Systems ein
Anmeldefenster angezeigt wird.
Wird Sokolsh per `exit` beendet, so wird der Nutzer automatisch abgemeldet, und die 
Anmeldeaufforderung wird erneut angezeigt.