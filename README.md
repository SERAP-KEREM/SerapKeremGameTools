# **SerapKeremGameTools** 🚀

🎉 **Welcome to SerapKeremGameTools!**  
This Unity package is designed to **simplify**, **optimize**, and **accelerate** your game development process. With a suite of ready-to-use tools and scripts, you'll save time on repetitive tasks and focus on building an amazing gameplay experience.  

> **Note:** This package is actively being developed, with regular updates and feature additions. For the latest changes, check out the **[GitHub Repository](https://github.com/SERAP-KEREM/SerapKeremGameTools)**.

---

## 📦 **Overview**

**SerapKeremGameTools** provides essential tools for Unity developers, such as global managers, object pooling, audio and particle systems, and more. These features are built to improve your productivity and streamline common workflows in game development.  

---
## 🛠️ **Scripts and Functionality**

### **Monosingleton.cs**
The **MonoSingleton<T>** and **NonMonoSingleton<T>** classes implement the Singleton design pattern, ensuring that only one instance of a class exists, and managing that instance globally across scenes.

- **MonoSingleton<T>:** A MonoBehaviour-based Singleton class compatible with Unity's scene management, ensuring a single instance exists even through scene transitions.
- **NonMonoSingleton<T>:** A non-MonoBehaviour Singleton class, typically used for game managers or other non-Unity-specific needs.

[📜 MonoSingleton](https://github.com/SERAP-KEREM/SerapKeremGameTools/blob/main/Assets/SerapKeremGameTools/_Game/Scripts/Singleton/MonoSingleton.cs)  
[📜 NonMonoSingleton](https://github.com/SERAP-KEREM/SerapKeremGameTools/blob/main/Assets/SerapKeremGameTools/_Game/Scripts/Singleton/NonMonoSingleton.cs)

---

### **AudioManager.cs**
**AudioManager** helps you integrate sounds easily into your game. You can manage sounds with the **Audio** class and control them with **AudioPlayer**.

- Handles sound pooling for memory management.
- Controls parameters such as volume and pitch for each sound.

[🎶 Audio](https://github.com/SERAP-KEREM/SerapKeremGameTools/blob/main/Assets/SerapKeremGameTools/_Game/Scripts/Audio/Audio.cs)  
[🎶 AudioManager](https://github.com/SERAP-KEREM/SerapKeremGameTools/blob/main/Assets/SerapKeremGameTools/_Game/Scripts/Audio/AudioManager.cs)  
[🎶 AudioPlayer](https://github.com/SERAP-KEREM/SerapKeremGameTools/blob/main/Assets/SerapKeremGameTools/_Game/Scripts/Audio/AudioPlayer.cs)

---

### **ParticleManager.cs**
The **ParticleEffectManager** efficiently manages particle effects and ensures optimal performance by pooling them for reuse.

- Controls and customizes particles using the **ParticleEffectData** class.
- Ensures particle effects are reused, saving memory.

[💥 ParticleEffectData](https://github.com/SERAP-KEREM/SerapKeremGameTools/blob/main/Assets/SerapKeremGameTools/_Game/Scripts/ParticleEffect/ParticleEffectData.cs)  
[💥 ParticleEffectManager](https://github.com/SERAP-KEREM/SerapKeremGameTools/blob/main/Assets/SerapKeremGameTools/_Game/Scripts/ParticleEffect/ParticleEffectManager.cs)

---

### **TimeManager.cs**
The **TimeManager** controls the in-game time and can pause, resume, and handle countdowns, making it easy to manage time-based events in your game.

- Allows pausing and resuming gameplay.
- Controls countdowns for timed events.

[⏰ TimeManager](https://github.com/SERAP-KEREM/SerapKeremGameTools/blob/main/Assets/SerapKeremGameTools/_Game/Scripts/Time/TimeManager.cs)

---

### **PopUpManager.cs**
The **Pop-Up System** Manage pop-up texts and icons efficiently with object pooling for memory optimization.

- The **PopUp** class manages common functions like starting animations and resetting states.
- Includes **PopUpText** and **PopUpIcon** for handling specific pop-up types.

[🎉 PopUp](https://github.com/SERAP-KEREM/SerapKeremGameTools/blob/main/Assets/SerapKeremGameTools/_Game/Scripts/PopUpManager/PopUp.cs)  
[🎉 PopUpText](https://github.com/SERAP-KEREM/SerapKeremGameTools/blob/main/Assets/SerapKeremGameTools/_Game/Scripts/PopUpManager/PopUpText.cs)  
[🎉 PopUpIcon](https://github.com/SERAP-KEREM/SerapKeremGameTools/blob/main/Assets/SerapKeremGameTools/_Game/Scripts/PopUpManager/PopUpIcon.cs)  

---

### **InputManager.cs**
The **Input Manager** handles mouse input and object interactions. The **Selector** class uses raycasting to manage interactions with **ISelectable** and **ICollectable** objects.

- **PlayerInput** tracks mouse position and handles click events.
- **Selector** manages object selection through raycasting.

[🖱️ PlayerInput](https://github.com/SERAP-KEREM/SerapKeremGameTools/blob/main/Assets/SerapKeremGameTools/_Game/Scripts/InputManager/PlayerInput.cs)  
[🖱️ Selector](https://github.com/SERAP-KEREM/SerapKeremGameTools/blob/main/Assets/SerapKeremGameTools/_Game/Scripts/InputManager/Selector.cs)

---

### **PauseManager.cs**
The **PauseManager** manages in-game pausing and resuming. When paused, it freezes the game and displays a pause menu.

- **PauseManager** stops interactions and gameplay, allowing for a smooth pause and resume system.

[⏸️ PauseManager](https://github.com/SERAP-KEREM/SerapKeremGameTools/blob/main/Assets/SerapKeremGameTools/_Game/Scripts/PauseManager/PauseManager.cs)

---

### **ObjectPool.cs**
The **ObjectPool<T>** class implements object pooling to optimize performance by reusing objects instead of constantly instantiating new ones.

- Objects are returned to the pool once they are no longer needed, reducing memory allocations.
- This system greatly improves memory management and processing efficiency.

[🔄 ObjectPool](https://github.com/SERAP-KEREM/SerapKeremGameTools/blob/main/Assets/SerapKeremGameTools/_Game/Scripts/ObjectPool/ObjectPool.cs)

---

### **SaveLoadSystem.cs**
The **SaveManager** and **LoadManager** classes make saving and loading game data straightforward by using **PlayerPrefs**.

- **SaveManager** saves the current game state.
- **LoadManager** loads the saved data and provides default values.

[💾 SaveManager](https://github.com/SERAP-KEREM/SerapKeremGameTools/blob/main/Assets/SerapKeremGameTools/_Game/Scripts/Save%26Load/SaveManager.cs)  
[💾 LoadManager](https://github.com/SERAP-KEREM/SerapKeremGameTools/blob/main/Assets/SerapKeremGameTools/_Game/Scripts/Save%26Load/LoadManager.cs)

---



## 📥 **How to Install**  

### **Via Unity Package Manager**  
1. Open Unity and navigate to `Window > Package Manager`.  
2. Click the **+** button and select **Add package from Git URL...**.  
3. Paste this link:  
   ```
   https://github.com/SERAP-KEREM/SerapKeremGameTools.git
   ```  
4. Click **Add** and wait for Unity to import the package.  

---

## 🌟 **Contributing**  

We welcome contributions, suggestions, and bug reports!  
- Fork the repository.  
- Submit pull requests with detailed explanations.  
- Report issues on GitHub.  

---

## 📧 **Contact**  

- **Developer:** Serap KEREM  
- **GitHub:** [@SERAP-KEREM](https://github.com/SERAP-KEREM)  
- **Email:** serapkerem19@gmail.com 
- **LinkedIn:** [linkedin.com/in/serapkerem/](https://www.linkedin.com/in/serapkerem/)
- **Portfolio:** [serap-kerem.github.io/webpages/](https://serap-kerem.github.io/webpages/)  
- **Blog:** [serapkerem.software](https://serapkerem.software/)  

---

✨ **Start using SerapKeremGameTools and take your Unity projects to the next level!**

-----------------

