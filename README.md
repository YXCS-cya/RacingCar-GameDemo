# Unity-Based Racing Simulation Game

## 🎮 Project Overview
This project is a **Unity-based Racing Simulation Game** featuring AI-controlled vehicles and dynamic physics.  
I served as **Project Manager and Core Developer**, independently implementing all control scripts, AI logic, and debugging tasks (3D modeling by teammates).  

The system integrates computer graphics, physics simulation, and AI pathfinding techniques, aiming to:  
1. **Design realistic tracks and scenes** using Unity and Maya  
2. **Simulate vehicle physics** including wheel colliders and suspension systems  
3. **Implement AI Pathfinding** with cubic Bézier curves for route generation  
4. **Support multiple camera perspectives** — third-person, side, and aerial views

---
## Technical Implementation

### Core Modules
#### 1. Scene Construction
- **Track Modeling**: Maya polygon subdivision → curve sculpting → shrink wrap → vertex optimization  
- **Scene Integration**: Import models into Unity → add colliders → apply materials and lighting  

#### 2. Vehicle Control
- **Physics Engine**: Utilize `WheelCollider` to simulate wheel friction and suspension  
- **Control Scripts**: Use `InputManager` for unified input handling; `CarMoveControl` manages steering, acceleration, and braking.  
  ```csharp
  // Steering logic example (partial code)
  float steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.5f / radius);
  wheels[0].steerAngle = steerAngle;
- **Camera Follow System**: Achieved smooth transitions via `LateUpdate()`
    

#### 3. AI Pathfinding

- **Path Generation**: Dynamically generated driving routes using cubic Bézier curves.
![image](https://github.com/user-attachments/assets/bc02a08f-0550-4c2e-ac7a-be99382c1163)

- **Tracking Logic**: Implemented a Tracker system that precomputes path points and enables the AI vehicle to follow the route in real time.

Demo Note: The AI pathfinding module was successfully developed and tested in a demo version.
However, due to collider conflicts between imported teammate models, the vehicle experienced slight vibration during simulation.
To ensure runtime stability, this module was not integrated into the final release (car.exe).
(See demo video: AI+BezierCurve.mp4
)

    

---

## Development Challenges & Solutions

### 1. Model Optimization

- **Issue**: Gaps appeared in the racetrack model exported from Maya
    
- **Solution**:
    
    - Manually merged vertices
        
    - Repeated “Average Vertex” and “Shrink Wrap” operations for correction
        

### 2. Physics Jitter

- **Issue**: Vehicle instability during high-speed movement
    
- **Solution**:
    
    - Moved physics calculations to `FixedUpdate()`
        
    - Moved camera tracking logic to `LateUpdate()` to decouple from physics updates
        



![Racing Car Demo](https://github.com/user-attachments/assets/4b35ca07-8256-424c-91c6-8615f222be0b)

---

## Project Reflection

This project successfully implemented core functions of a racing simulation game in Unity, providing valuable insights in the following areas:

1. **Technical Aspects**: Gained in-depth understanding of physics engine tuning and Bézier curve applications in gameplay.
    
2. **Collaboration**: Highlighted the importance of modular design and version control.
    
3. **Optimization**: In order to balance system performance, although AI path-finding system had successfully tested in my local environment, our team still chose not to install it into final build, since the uncontrollable shaking remained, and we made a vedio demo instead.
---

## 🛠️ Build & Run Note
The demo is developed and built in **Unity 2022.3**.  
You can open the project in Unity and run it directly from the editor.  
Alternatively, a compiled version (`car.exe`) is available in the Release section for quick preview.

