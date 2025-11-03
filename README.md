# VideoApp (.NET 8, MVC, MediatR)

**Features**
- Upload MP4 (<=200MB per request)
- Catalogue listing (filename + size)
- Single-video playback on selection

**Tech**
- ASP.NET Core 8 MVC (no minimal APIs)
- MediatR (CQRS)
- jQuery (simple SPA interactions)
- Bootstrap (responsive)

**Run locally**
- Prereqs: .NET 8 SDK, VS 2022
- Open the solution → Run with IIS Express
- Or: `dotnet run` in the Web project folder, then open `https://localhost:####/`

**Upload notes**
- Uploads saved under `wwwroot/media`
- Repo ignores real media (`wwwroot/media/**`) – add your own test MP4s locally.
- For sample media in Git, use Git LFS.

**Time spent**
- ~6 hours
