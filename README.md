# Simple Music Library API
A simple API for managing a music library.

## What I Built
This repository contains a single Visual Studio solution with two projects.

1) SimpleMusicLibraryAPI
2) SimpleMusicLibraryWebApplication

The first project contains a Web API to perform all the necessary CRUD operations to manage the music library. The second project is a simple web interface for accessing the API to manage the music library.

## Architecture Decisions
The purpose of this project is to demonstrate the ability to create a simple Web API and web interface. It is not meant for production use. As such, it was architected to have as few moving parts as possible.

I decided to use an in-memory database using Entity Framework to simplify the project and allow it to be used without the need to set up and configure a physical database.

## Deployment and Use
This project is only meant to be deployed locally for demonstration purposes.

1) Clone the repository.
2) Open the SimpleMusicLibraryAPI.sln in Visual Studio 2019.
3) Build and run the solution.
4) A browser will open for the web interface.

## Future Improvements
There are several things to be improved about this project. Here are just a few in no particular order.

1) Constraints and security measures around file uploads (e.g., size limits, file type rules, etc.)
2) Add a music player to the web interface instead of only allowing direct downloads of songs.
3) Confirmation before deleting a song.
4) Error handling.
5) Prevent the uploading of songs that are already in the library.
6) Styling of the web interface.
7) Add authentication and authorization.
8) Add a rating system for songs.
9) Allow album cover art to be uploaded and displayed for each song.
