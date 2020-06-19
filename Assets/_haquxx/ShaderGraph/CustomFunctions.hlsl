void fmod(float a, float b, out float c)
{
    c = a - floor(a / b) * b;
}

void Triangle_float(float2 UV, float Scale, out float Triangle, out float2 TrianglePosition)
{
    float N = Scale;
    float2 p = UV;
    p.x *= 0.86602; // sqrt(3)/2倍
    float isTwo = fmod(floor(p.y * N), 2.0); // 偶数列目なら1.0
    float isOne = 1.0 - isTwo; // 、奇数列目なら1.0
    // xy座標0~1の正方形をタイル状に複数個並べる
    p = p * N;
    p.x += isTwo * 0.5; // 偶数列目を0.5ズラす
    float2 p_index = floor(p); // 正方形の番号
    float2 p_rect = frac(p);
    p = p_rect; // 正方形内部の座標
    float xSign = sign(p.x - 0.5); // タイルの右側なら+1.0, 左側なら-1.0
    p.x = abs(0.5 - p.x); // x=0.5を軸として左右対称にする
    float isInTriangle = step(p.x * 2.0 + p.y, 1.0); // 三角形の内部にある場合は1.0
    float isOutTriangle = 1.0 - isInTriangle; // 三角形の外側にある場合は1.0
    // 中央の三角形
    float w1 = max( p.x * 2.0 + p.y, 1.0 - p.y * 1.5 ); 
  
    // 右上(左上)の三角形
    p = float2(0.5, 1.0) - p;
    float w2 = max(p.x * 2.0 + p.y, 1.0 - p.y * 1.5 );
    // 三角形グラデーション
    Triangle = lerp(1.0 - w2, 1.0 - w1, isInTriangle) / 0.6;
  
    // 三角形の位置
    float2 triangleIndex = p_index + float2(
        isOutTriangle * xSign / 2.0 // 左上の部分は-0.5、右上の部分は+0.5
    + isOne / 2.0, // 基数列目の三角形は横に0.5ズレているので+0.5する
    0.0
    );
  
    // 三角形の座標
    TrianglePosition = triangleIndex / N;
}

float random(float2 st) {
    return frac(sin(dot(st.xy,
                        float2(12.9898,78.233)))*
                43758.5453123);
}

float noise (float2 st) {
    float2 i = floor(st);
    float2 f = frac(st);
    // Four corners in 2D of a tile
    float a = random(i);
    float b = random(i + float2(1.0, 0.0));
    float c = random(i + float2(0.0, 1.0));
    float d = random(i + float2(1.0, 1.0));
    float2 u = f * f * (3.0 - 2.0 * f);
    return lerp(a, b, u.x) +
            (c - a)* u.y * (1.0 - u.x) +
            (d - b) * u.x * u.y;
}

float fbm (float2 st) {
    int OCTAVES = 3;
    // Initial values
    float value = 0.0;
    float amplitude = .5;
    float frequency = 0.;
    // Loop of octaves
    for (int i = 0; i < OCTAVES; i++) {
        value += amplitude * noise(st);
        st *= 2.;
        amplitude *= .5;
    }
    return value;
}

float pattern_3 (float2 p) {
    // first domain warping
    float2 q = float2( 
                    fbm( p + float2(0.0,0.0) ),
                    fbm( p + float2(5.2,1.3) ) 
                    );
                            
    // second domain warping
    float2 r = float2( 
                    fbm( p + 4.0*q + float2(1.7,9.2) ),
                    fbm( p + 4.0*q + float2(8.3,2.8) ) 
                    );
    return fbm( p + 4.0*r );
}

float pattern (float2 p, float4 scale_1, float scale_2, float4 add_1, float4 add_2) {
    // first domain warping
    float2 q = float2( 
                    fbm( p + scale_1.x * add_1.xy ),
                    fbm( p + scale_1.y * add_1.zw ) 
                    );
                            
    // second domain warping
    float2 r = float2( 
                    fbm( p + scale_1.z * q + add_2.xy ),
                    fbm( p + scale_1.w * q + add_2.zw ) 
                    );
    return fbm( p + scale_2 * r );
}

void DomainWarping_float(float2 UV, float4 Fbm_AddFactor_1, float4 Fbm_AddFactor_2, float4 Fbm_ScaleFactor_1, float Fbm_ScaleFactor_2, out float Pattern)
{
    Pattern = pattern(UV, Fbm_ScaleFactor_1, Fbm_ScaleFactor_2, Fbm_AddFactor_1, Fbm_AddFactor_2);
}