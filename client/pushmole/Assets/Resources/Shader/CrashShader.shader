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
	//------------------------------------������ֵ��------------------------------------  
	Properties
	{
		//��ɫֵ  
		_ColorWithAlpha("ColorWithAlpha", Color) = (0.9, 0.1, 0.1, 0.5)
	}

		//------------------------------------��Ψһ������ɫ����------------------------------------  
		SubShader
	{
		//����QueueΪ͸���������з�͸�����������֮���ٽ��л���  
		Tags{ "Queue" = "Transparent" }

		//--------------------------------Ψһ��ͨ��-------------------------------  
		Pass
	{
		//��д����Ȼ���,Ϊ�˲��ڵ�ס��������  
		ZWrite Off

		//ѡȡAlpha��Ϸ�ʽ  
		Blend  SrcAlpha SrcAlpha
		//Blend SrcAlpha OneMinusSrcAlpha  

		//===========����CG��ɫ�����Ա�дģ��============  
		CGPROGRAM

		//����ָ��:��֪�����������Ƭ����ɫ����������  
#pragma vertex vert   
#pragma fragment frag  

		//��������  
		uniform float4 _ColorWithAlpha;

	//--------------------------------��������ɫ������-----------------------------  
	// ���룺POSITION���壨����λ�ã�  
	// �����SV_POSITION���壨����λ�ã�  
	//---------------------------------------------------------------------------------  
	float4 vert(float4 vertexPos : POSITION) : SV_POSITION
	{
		//����ϵ�任  
		//����Ķ���λ�ã�����λ�ã�Ϊģ����ͼͶӰ������Զ���λ�ã�Ҳ���ǽ���ά�ռ��е�����ͶӰ���˶�ά����  
		return UnityObjectToClipPos(vertexPos);
	}

		//--------------------------------��Ƭ����ɫ������-----------------------------  
		// ���룺��  
		// �����COLOR���壨��ɫֵ��  
		//---------------------------------------------------------------------------------  
		float4 frag(void) : COLOR
	{
		//�����Զ����RGBA��ɫ  
		return _ColorWithAlpha;
	}

		//===========����CG��ɫ�����Ա�дģ��===========  
		ENDCG
	}
	}
}  