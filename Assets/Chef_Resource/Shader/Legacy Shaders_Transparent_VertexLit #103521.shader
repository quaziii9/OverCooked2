//////////////////////////////////////////
//
// NOTE: This is *not* a valid shader file
//
///////////////////////////////////////////
Shader "Legacy Shaders/Transparent/VertexLit" {
Properties {
_Color ("Main Color", Color) = (1,1,1,1)
_SpecColor ("Spec Color", Color) = (1,1,1,0)
_Emission ("Emissive Color", Color) = (0,0,0,0)
_Shininess ("Shininess", Range(0.1, 1)) = 0.7
_MainTex ("Base (RGB) Trans (A)", 2D) = "white" { }
}
SubShader {
 LOD 100
 Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
 Pass {
  LOD 100
  Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "Vertex" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
  Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
  ColorMask RGB 0
  ZWrite Off
  Fog {
   Mode Off
  }
  GpuProgramID 104301
Program "vp" {
SubProgram "d3d11 hw_tier00 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 hw_tier01 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 hw_tier02 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 hw_tier00 " {
Keywords { "POINT" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 hw_tier01 " {
Keywords { "POINT" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 hw_tier02 " {
Keywords { "POINT" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 hw_tier00 " {
Keywords { "SPOT" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 hw_tier01 " {
Keywords { "SPOT" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 hw_tier02 " {
Keywords { "SPOT" }
"// shader disassembly not supported on DXBC"
}
}
Program "fp" {
SubProgram "d3d11 hw_tier00 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 hw_tier01 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 hw_tier02 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 hw_tier00 " {
Keywords { "POINT" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 hw_tier01 " {
Keywords { "POINT" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 hw_tier02 " {
Keywords { "POINT" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 hw_tier00 " {
Keywords { "SPOT" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 hw_tier01 " {
Keywords { "SPOT" }
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 hw_tier02 " {
Keywords { "SPOT" }
"// shader disassembly not supported on DXBC"
}
}
}
 Pass {
  LOD 100
  Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "VertexLM" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
  Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
  ColorMask RGB 0
  ZWrite Off
  GpuProgramID 39354
Program "vp" {
SubProgram "d3d11 hw_tier00 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 hw_tier01 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 hw_tier02 " {
"// shader disassembly not supported on DXBC"
}
}
Program "fp" {
SubProgram "d3d11 hw_tier00 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 hw_tier01 " {
"// shader disassembly not supported on DXBC"
}
SubProgram "d3d11 hw_tier02 " {
"// shader disassembly not supported on DXBC"
}
}
}
}
}