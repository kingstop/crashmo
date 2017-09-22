// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge Beta 0.34   
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/  
// Note: Manually altering this data may prevent you from opening it in Shader Forge  
/*SF_DATA;ver:0.34;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,blpr:1,bsrc:3,bdst:7,culm:0,dpts:2,wrdp:False,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:3,x:32038,y:32598|diff-227-RGB,alpha-226-OUT;n:type:ShaderForge.SFN_Fresnel,id:226,x:32459,y:32705;n:type:ShaderForge.SFN_Color,id:227,x:32398,y:32551,ptlb:Color,ptin:_Color,glob:False,c1:1,c2:1,c3:1,c4:1;proporder:227;pass:END;sub:END;*/  
  
Shader "Bodhi/CrashShader" {  
	//------------------------------------【属性值】------------------------------------  
	Properties
	{
		//颜色值  
		_ColorWithAlpha("ColorWithAlpha", Color) = (0.9, 0.1, 0.1, 0.5)
	}

		//------------------------------------【唯一的子着色器】------------------------------------  
		SubShader
	{
		//设置Queue为透明，在所有非透明几何体绘制之后再进行绘制  
		Tags{ "Queue" = "Transparent" }

		//--------------------------------唯一的通道-------------------------------  
		Pass
	{
		//不写入深度缓冲,为了不遮挡住其他物体  
		ZWrite Off

		//选取Alpha混合方式  
		Blend  SrcAlpha SrcAlpha
		//Blend SrcAlpha OneMinusSrcAlpha  

		//===========开启CG着色器语言编写模块============  
		CGPROGRAM

		//编译指令:告知编译器顶点和片段着色函数的名称  
#pragma vertex vert   
#pragma fragment frag  

		//变量声明  
		uniform float4 _ColorWithAlpha;

	//--------------------------------【顶点着色函数】-----------------------------  
	// 输入：POSITION语义（坐标位置）  
	// 输出：SV_POSITION语义（像素位置）  
	//---------------------------------------------------------------------------------  
	float4 vert(float4 vertexPos : POSITION) : SV_POSITION
	{
		//坐标系变换  
		//输出的顶点位置（像素位置）为模型视图投影矩阵乘以顶点位置，也就是将三维空间中的坐标投影到了二维窗口  
		return UnityObjectToClipPos(vertexPos);
	}

		//--------------------------------【片段着色函数】-----------------------------  
		// 输入：无  
		// 输出：COLOR语义（颜色值）  
		//---------------------------------------------------------------------------------  
		float4 frag(void) : COLOR
	{
		//返回自定义的RGBA颜色  
		return _ColorWithAlpha;
	}

		//===========结束CG着色器语言编写模块===========  
		ENDCG
	}
	}
}  