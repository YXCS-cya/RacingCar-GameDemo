# Unity-Based Racing Simulation Game

## 🎮 Project Overview
- **Project Title**: Unity-Based Racing Simulation Game  
- **Role**: Project Manager & Core Developer (independently completed all control and AI modules)  
- **Development Tools**: Unity, C#, Maya, Visual Studio  
- **Keywords**: AI Path Planning, Physics Engine, Game Script Optimization  
- **Completion Date**: December 2024  
> This project was primarily developed by myself: except for 3D modeling, all racing scripts, AI logic, and debugging were independently implemented.

---

## Summary
This project aims to develop a racing simulation game with AI opponents using the Unity engine, integrating computer graphics and virtual reality techniques.  
The main objectives include:  
1. **Scene Modeling**: Build diverse racetracks and environmental models with Maya  
2. **Physical Interaction**: Implement wheel collision detection, physical materials, and suspension systems  
3. **AI Pathfinding**: Apply cubic Bézier curves for dynamic route generation  
4. **Multiple Perspectives**: Support third-person, side, and aerial view switching  

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
        

---

![Racing Car Demo](https://github.com/user-attachments/assets/4b35ca07-8256-424c-91c6-8615f222be0b)

---

## Project Reflection

This project successfully implemented core functions of a racing simulation game in Unity, providing valuable insights in the following areas:

1. **Technical Aspects**: Gained in-depth understanding of physics engine tuning and Bézier curve applications in gameplay.
    
2. **Collaboration**: Highlighted the importance of modular design and version control.
    
3. **Optimization**: Balanced system performance by lowering AI update frequency to maintain stable frame rates.
