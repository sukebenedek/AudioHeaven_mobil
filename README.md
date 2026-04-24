# 🎵 AudioHeaven - Mobile Application

**AudioHeaven** is a modern, community-driven music streaming platform's mobile client, built using the **.NET MAUI** framework.  
The application aims to provide a clean and intuitive interface for users to discover and play music wherever they go.

---

## ✨ Key Features (Mobile)

- **Dynamic Homepage**  
  Access personalized recommendations, listening history, and the most recent uploads.

- **Universal Search**  
  Search for artists (users), songs, and albums simultaneously in a single interface.

- **Smart Player**  
  Full integration with the Android Media Control panel, allowing playback control from the notification bar and the lock screen.

- **Queue Management**  
  Dynamic reordering of the playback queue and the ability to remove items.

- **Library**  
  Access and manage your personal playlists.

- **Secure Authentication**  
  Laravel Sanctum-based token login, utilizing hardware-protected SecureStorage.

---

## 🛠 Technological Stack

- **Framework:** .NET MAUI (C# / XAML)  
- **Architecture:** MVVM (CommunityToolkit.Mvvm)  
- **Media Playback:** CommunityToolkit.Maui.MediaElement  
- **UI Components:** CommunityToolkit.Maui, Plugin.Maui.BottomSheet  
- **Backend Connection:** REST API (Laravel)  

---

## 🚀 Execution and Setup

### Backend Preparation

To connect the mobile client, the backend must be running and accessible on the local network:

```bash
php artisan serve --host 0.0.0.0 --port 8000
```
## 🚀 Running the Mobile App

1. Clone the repository  
2. Open the project in Visual Studio 2022 (MAUI workload required)  
3. Set the correct IP address in the network configuration (for localhost testing)  
4. Run on an Android emulator or a physical device  

**Note:** The application is also available in APK format among the GitHub releases or on the project's website.

---

## 🎨 Design

The UI/UX designs for the mobile application were created in Figma in parallel with the web interface to ensure a consistent experience.  
The mobile version focuses on content consumption, so music uploading is currently only available via the web interface.

---

## 👥 Creators

- **Süke Benedek** – Mobile Development  
- **Csöngető Csongor** – Backend Development  
- **Tófalvi Zalán** – Frontend & Design  
