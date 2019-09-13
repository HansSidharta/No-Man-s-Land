﻿// MIT License

// Copyright (c) 2019 Eldemarkki

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

Shader "Marching Cubes/Triplanar Shader" {
	Properties{
		XColor("X Color", Color) = (0,0,0,0)
		YColor("Y Color", Color) = (0,0,0,0)
		NegativeYColor("Negative Y Color", Color) = (0,0,0,0)
		ZColor("Z Color", Color) = (0,0,0,0)
		YColorAmount("Y Color Amount", float) = 0.4
	}

		SubShader{

			Tags { "RenderType" = "Opaque"}

			Pass {
				CGPROGRAM
				#pragma target 3.0
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				fixed4 XColor;
				fixed4 YColor;
				fixed4 NegativeYColor;
				fixed4 ZColor;
				fixed YColorAmount;

				struct appdata {
					fixed3 normal : NORMAL;
					float4 vertex : POSITION;
				};

				struct v2f {
					half4 color : COLOR;
					float4 vertex : SV_POSITION;
				};

				v2f vert(appdata v)
				{
					fixed x = abs(v.normal.x);
					fixed yNormal = v.normal.y;
					fixed y = abs(yNormal) * YColorAmount;
					fixed z = abs(v.normal.z);

					fixed total = (x + y + z);
					x /= total;
					y /= total;
					z /= total;

					// X
					fixed4 col = (XColor * x);

					// Y
					if (yNormal < 0)
						col += NegativeYColor * y;
					else
						col += YColor * y;

					// Z
					col += (ZColor * z);

					v2f o;

					o.color = col.rgba;
					o.vertex = UnityObjectToClipPos(v.vertex);

					return o;
				}



				fixed4 frag(v2f i) : SV_TARGET{
					return i.color;
				}

				ENDCG
			}
	}

}