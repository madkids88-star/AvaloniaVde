# AGENTS.md — правила проекта AvaloniaVde

## Целевые платформы
- **Windows** (`AvaloniaVde.Desktop`) — ежедневная отладка.
- **Android** (`AvaloniaVde.Android`) — релиз (Android 12+).
- **Linux** (`AvaloniaVde.Linux`) — хост для Ubuntu / Raspberry Pi.
- iOS (`AvaloniaVde.iOS`) и Browser (`AvaloniaVde.Browser`) — есть в шаблоне, но **не трогать** без явного запроса.

Вся UI-логика, ViewModels, Views и ассеты — в ядре `AvaloniaVde` (`net10.0`). Платформенные проекты — только точки входа.

## Кроссплатформенные правила (обязательно)
- **Шрифты**: только встроенный Inter (`WithInterFont()`). Жирность — через `FontWeight="Bold"`. Никаких системных шрифтов (`Segoe UI`, `Arial`, `Roboto` и т.п.) — их нет на всех ОС. Не встраивать проприетарные шрифты (Arial и др.) из-за лицензии.
- **Иконки/картинки**: PNG/SVG в `Assets/` через `AvaloniaResource`. Пути — `/Assets/...`. `.ico` — только для `Window.Icon`. Android-ресурсы — в `AvaloniaVde.Android/Resources/drawable`.
- **Пути**: только URI ассетов (`/Assets/...`, `avares://...`). Никаких `C:\...` и системных каталогов в UI.
- **Вёрстка**: `Stretch="Uniform"`, без хардкода пикселей под конкретный экран/DPI.
- **Платформенный код**: не писать `#if WINDOWS` без `#else`. Платформенные API — через общие интерфейсы с реализациями на каждую ОС.

## Сборка
- Desktop: `dotnet build AvaloniaVde/AvaloniaVde.Desktop -c Debug`
- Android (установка на устройство): `dotnet build AvaloniaVde/AvaloniaVde.Android -c Debug -t:Install`
- Linux (smoke-тест на Windows): `dotnet build AvaloniaVde/AvaloniaVde.Linux -c Debug`

## Кодировка
- Все исходники (`.cs`, `.axaml`) — **UTF-8 with BOM**, CRLF. Защита: `.editorconfig` в корне + `<CodePage>65001</CodePage>` в `Directory.Build.props`. Не перезаписывать файлы скриптами без `-Encoding utf8` / `UTF8Encoding($true)`.

## Прочее
- Пакеты версионируются централизованно в `Directory.Packages.props` (Central Package Management). При добавлении пакета — `PackageReference` без версии + `PackageVersion` там.
- Перед завершением задачи собирать затронутые проекты и исправлять предупреждения/ошибки.
