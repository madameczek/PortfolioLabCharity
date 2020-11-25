# Charity

## Pomysł na projekt. User story

Użytkownik ma w domu rzeczy, których nie używa, ale są one w dobrym stanie i chce przekazać je osobom, którym się mogą przydać - nie wie jednak jak w prosty sposób to zrobić.
Jest wiele dostępnych rozwiązań, ale wiele z nich wymaga dodatkowego wysiłku lub nie budzą one zaufania.
W zweryfikowane miejsca trzeba pojechać, a nie ma na to czasu lub nie ma jak tam pojechać. Natomiast kontenery pod domem lub lokalne zbiórki są niezweryfikowane i nie wiadomo czy te rzeczy faktycznie trafią do potrzebujących.

Powyższy opis wraz z layoutem w czystym HTMLu jest wsadem do wykonania aplikacji.

## Stan realizacji (co działa)

- Utworzenie zlecenia wysłania darów
  - Walidacja wprowadzanych danych przez atrybuty modelu oraz dodatkową logiką
- Wysłanie potwierdzenia zlecenia na email zarejestrowanego użytkownika
- Rejestracja użytkownika
- Potwierdzenie rejestacji przez email z tokenem
- [Działające demo w chmurze Azure](https://drugie-zycie.azurewebsites.net)

## Do dokończenia

- Zarządznie kontem użytkownika
- Obsługa formularza kontaktowego

## Użyte języki

- C#
- JavaScript
- HTML/CSS

## Frameworki i biblioteki

- .NET Core MVC
- .NET Core with Identity
- Entity Framework
- Azure Cloud
- MailKit
- Serilog: logowanie do pliku, serwera syslog, bazy danych
- konfiguracja aplikacji w plikach .json

## Bazy danych

- MS SQL
- SQLite w chmurze

---
Jeśli chcesz o coś spytać, tu jest moja [wizytówka](https://www.adameczek.pl "My Homepage") z danymi kontaktowymi.
